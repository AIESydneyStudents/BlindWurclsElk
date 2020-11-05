using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjects_Japan : MonoBehaviour
{
    [Tooltip("Objects to toggle after all objects in japan have been collected")]
    public GameObject[] objects;

    // Have all the items been collected?
    public static bool collected = false;


    void Start()
    {
        // Invoke to allow the player to find the nearest chair before disabling it
        Invoke("Toggle", 0.1f);
    }

    private void Toggle()
    {
        foreach (var obj in objects)
        {
            // Toggle if collected == true
            obj.SetActive(obj.activeSelf ^ collected);
        }
    }
}