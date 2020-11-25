using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    public void OnPlayClick()
    {
        // Reset collections for new game
        CollectionObjects_British.collected = false;
        CollectionObjects_India.collected = false;
        CollectionObjects_Japan.collected = false;

        // Loading the player scene will cause it to load the train scene by default
        SceneManager.LoadScene("PlayerScene");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}