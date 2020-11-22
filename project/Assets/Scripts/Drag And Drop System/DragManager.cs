using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragManager : MonoBehaviour
{
    public static DragManager instance;

    // All tiles in the scene. Tiles add themselves to the list
    [HideInInspector]
    public List<DragScript> tiles = new List<DragScript>();

    public float slotSnapRange = 20f;


    private void Awake()
    {
        instance = this;

        // Unlock cursor and disable controller
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = false;

        FindObjectOfType<PauseMenuScript>().inMinigame = true;
    }

    public void CheckTiles()
    {
        bool allLocked = true;

        // Itterate through each tile
        foreach(var tile in tiles)
        {
            if (tile.canBeMoved)
            {
                // A tile is not locked, toggle bool
                allLocked = false;
                break;
            }
        }

        // If all tiles are locked, game over
        if (allLocked)
        { GameOver(); }
    }


    // Called when all tiles are locked
    private void GameOver()
    {
        // Lock cursor and renable controller
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;

        FindObjectOfType<PauseMenuScript>().inMinigame = false;


        //hide UI, display anim for getting tea piece


        // Unload this scene, and return to train with the player sitting in the booth
        SceneManager.UnloadSceneAsync("Britain_PuzzleScene");
        TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-3.5f, 1f, -3.2f), null, true);
    }
}