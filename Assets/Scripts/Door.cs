using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    private bool isOpen = false;
    [SerializeField] private bool isSlidingDoor = true;
    [SerializeField] private float speed = 1f;
    [SerializeField] private GameObject[] requirements;
    [SerializeField] private bool[] isTriggered;
    private int counter = 0;

    [Header("Sliding Config")]
    [SerializeField] private Vector3 slideDirection = Vector3.right;
    [SerializeField] private float slideAmount = 14.9f; //slightly smaller than door size to prevent Z-fighting

    private Vector3 startPosition;

    private Coroutine animationCoroutine;

    private void Awake()
    {
        isTriggered = new bool[requirements.Length];
        for (int i = 0; i < isTriggered.Length; i++)
        {
            isTriggered[i] = false;
        }
        startPosition = transform.position;
    }

    private void Update()
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            isTriggered[i] = requirements[i].GetComponent<PressurePlate>().GetPressed();
            if (isTriggered[i]) counter += 1;
            else counter -= 1;
        }
        if (counter == requirements.Length) Open(GameObject.Find("Robot Kyle").transform.position);
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
