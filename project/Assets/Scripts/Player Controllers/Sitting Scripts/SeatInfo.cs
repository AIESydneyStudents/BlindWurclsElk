using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SeatInfo : MonoBehaviour
{
    public Vector3 sittingPos;
    public Vector3 standingPos;

    [Tooltip("What is the rotation on the Y axis to face forward in the seat?")]
    public float forwardRotation = 0f;


    void Awake()
    {
        //get positions relative to the object
        sittingPos += transform.position;
        standingPos += transform.position;

        // Set the object's layer to 'seat'
        gameObject.layer = 8;
    }
}