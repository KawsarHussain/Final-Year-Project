using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Popup Config")]
    [SerializeField] private GameObject[] popups;
    [SerializeField] private int popUpIndex;


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == popUpIndex) popups[i].SetActive(true);
            else popups[i].SetActive(false);
        }

        if (popUpIndex == 0) if (Input.GetKeyDown(KeyCode.W)) popUpIndex++;

 
        if (popUpIndex == 1) if (Input.GetKeyDown(KeyCode.S)) popUpIndex++;


        if (popUpIndex == 2) if (Input.GetKeyDown(KeyCode.A)) popUpIndex++;


        if (popUpIndex == 3) if (Input.GetKeyDown(KeyCode.D)) popUpIndex++;


        if (popUpIndex == 4) if (Input.GetKeyDown(KeyCode.LeftShift)) popUpIndex++;


        if (popUpIndex == 5 || popUpIndex == 6) if (Input.GetKeyDown(KeyCode.Mouse0)) popUpIndex++;
    }
}
