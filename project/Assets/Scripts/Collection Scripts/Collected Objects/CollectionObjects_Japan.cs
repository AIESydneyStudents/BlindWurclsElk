using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjects_Japan : MonoBehaviour
{
    [Tooltip("Objects to toggle after all objects in japan have been collected")]
    public GameObject[] objects;

    // Have all the items been collected?
    public static bool collected = false;


    void Awake()
    {
        foreach (var obj in objects)
        {
            // Toggle if collected == true
            obj.SetActive(obj.activeSelf ^ collected);
        }
    }
}