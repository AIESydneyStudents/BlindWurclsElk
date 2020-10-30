using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedObject_Test : MonoBehaviour
{
    /// <summary>
    /// Is the train carrage object assosiated with this collection active?
    /// </summary>
    public static bool active = false;


    void Start()
    {
        // Activate/deactivate the object with the static bool
        gameObject.SetActive(active);
    }
}