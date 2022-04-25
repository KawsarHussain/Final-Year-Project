using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Door door;

    private void OnTriggerEnter(Collider other)
    {
        //checks if other component is a player
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if (!door.GetOpen()) door.Open(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //checks if other component is a player
        if (other.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if (door.GetOpen()) door.Close();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
