using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wristband : MonoBehaviour
{
    private bool thrown;
    private Vector3 coords = new Vector3();
    private GameObject attachedObject;

    public Wristband()
    {
        thrown = false;
        attachedObject = null;
    }

    public bool GetThrown() { return thrown; } 

    public void UpdateThrown(bool state) { thrown = state; }

    public Vector3 GetCoords() { return coords; }

    public GameObject GetAttachedObject() { return attachedObject; }

    public void UpdateAttachedObject(GameObject newAttachedObject) { attachedObject = newAttachedObject; }

    public Vector3 GetAttachedObjectCoords() { return attachedObject.transform.position; }

    public void UpdateCoords(Vector3 newCoords) { coords = newCoords; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
