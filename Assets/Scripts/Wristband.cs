using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wristband : MonoBehaviour
{
    private bool thrown;
    public GameObject band;
    private GameObject attachedObject;
    private bool objectHit;
    private Rigidbody rb;

    void Start()
    {
        objectHit = false;
        rb = GetComponent<Rigidbody>();
        thrown = false;
        attachedObject = null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            return;
        }

        if (objectHit) return;
        else objectHit = true;

        rb.isKinematic = true;

        transform.SetParent(collision.transform);
        attachedObject = collision.gameObject;

    }

    public bool GetThrown() { return thrown; }

    public void UpdateThrown(bool state) { thrown = state; }

    public GameObject GetBandObject() { return band.gameObject; }
    public Vector3 GetCoords() { return band.transform.position; }

    public GameObject GetAttachedObject() { return attachedObject; }

    public void UpdateAttachedObject(GameObject newAttachedObject) { attachedObject = newAttachedObject; }

    public Vector3 GetAttachedObjectCoords() { return attachedObject.transform.position; }

    public void UpdateCoords(Vector3 newCoords) { band.transform.position = newCoords; }


}
