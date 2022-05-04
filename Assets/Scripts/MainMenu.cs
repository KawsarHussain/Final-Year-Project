using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartButton()
    {

    }

    public void PlayTut()
    {
        SceneManager.LoadScene(1);
    }

    public void PlayLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
