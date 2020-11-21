using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public static bool paused = false;
    // When in tea making minigames, the cursor should not be locked
    [HideInInspector]
    public bool inMinigame = false;

    public GameObject pauseMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                //hide pause menu
                pauseMenu.SetActive(false);

                if (!inMinigame)
				{
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

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

        if (!inMinigame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Time.timeScale = 1;
        paused = false;
    }

    public void OnExitClick()
    {
        Time.timeScale = 1;
        paused = false;

        //show the main menu
        SceneManager.LoadScene(0);
    }
}