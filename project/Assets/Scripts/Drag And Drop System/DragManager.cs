using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        //game over stuff
        Debug.Log("All tiles are locked; Game over!");
    }
}