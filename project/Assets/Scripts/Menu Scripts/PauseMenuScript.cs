using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    public static bool paused = false;

    public GameObject pauseMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                //hide pause menu
                pauseMenu.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                Time.timeScale = 1;
                paused = false;
            }
            else
            {
                //show pause menu
                pauseMenu.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                Time.timeScale = 0;
                paused = true;
            }
        }
    }

    public void OnResumeClick()
    {
        //hide pause menu
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
        paused = false;
    }

    public void OnExitClick()
    {
        Time.timeScale = 1;

        //show the main menu
    }
}