using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    private bool isPressed = false;
    [SerializeField] private float speed = 1f;

    [Header("Pressing Config")]
    [SerializeField] private Vector3 slideDirection = Vector3.up;
    [SerializeField] private float slideAmount = -0.18f; //slightly smaller than door size to prevent Z-fighting
    [SerializeField] private GameObject obj;

    private Vector3 startPosition;

    private Coroutine animationCoroutine;


    private void Awake()
    {
        startPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == obj) Press();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == obj) Depress();
    }

    public bool GetPressed() { return isPressed; }

    public void Press()
    {
        if (!isPressed)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }

        animationCoroutine = StartCoroutine(DoPress());
    }

    private IEnumerator DoPress()
    {
        Vector3 EndPosition = startPosition - slideAmount * slideDirection;
        Vector3 StartPosition = transform.position;

        float time = 0;
        isPressed = true;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Depress()
    {
        if (isPressed)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            animationCoroutine = StartCoroutine(DoDepress());
        }
    }

    private IEnumerator DoDepress()
    {
        Vector3 EndPosition = startPosition;
        Vector3 StartPosition = transform.position;

        float time = 0;
        isPressed = false;
        while (time < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

