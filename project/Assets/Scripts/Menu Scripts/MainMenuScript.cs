using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    public void OnPlayClick()
    {
        // Loading the player scene will cause it to load the train scene by default
        SceneManager.LoadScene("PlayerScene");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}