using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool isOpen = true;
    [SerializeField] private bool isSlidingDoor = true;
    [SerializeField] private float speed = 1f;

    [Header("Sliding Config")]
    [SerializeField] private Vector3 slideDirection = Vector3.back;
    [SerializeField] private float slideAmount = 1.9f; //slightly smaller than door size to prevent Z-fighting

    private Vector3 startPosition;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        startPosition = transform.position;
    }

    public bool GetOpen() { return isOpen; }

    public void Open(Vector3 playerPos)
    {
        if (!isOpen)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }

        if (isSlidingDoor) animationCoroutine = StartCoroutine(DoSlidingOpen());
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 EndPosition = startPosition + slideAmount * slideDirection;
        Vector3 StartPosition = transform.position;

        float time = 0;
        isOpen = true;
        while(time < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }

    public void Close()
    {
        if (isOpen)
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
            if (isSlidingDoor) animationCoroutine = StartCoroutine(DoSlidingClose());
        }
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 EndPosition = startPosition;
        Vector3 StartPosition = transform.position;
        
        float time = 0;
        isOpen = false;
        while(time < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
