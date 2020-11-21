using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalDragScript : MonoBehaviour
{
    // Object being moved
    Rigidbody obj = null;

    [Tooltip("The positions caols will be snapped to")]
    public Transform[] coalSnapPos;

    public BurnerTrigger burner;

    // Y point of the surface in world space
    public float surfHeight;
    // Height of object from surface while dragging
    public float dragHeight = 1f;
    [Space]

    public AudioSource soundSource;

    public AudioClip soundPickup;
    public AudioClip soundDrop;


    // Find the cursor position in world space relative to the object position
    private Vector3 CursorOnTransform(Vector3 objectPosition)
    {
        //find the cursor in world space
        Vector3 CameraToCursor()
        {
            return Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane)) - Camera.main.transform.position;
        }

        //transform relative to cam
        Vector3 camToTrans = objectPosition - Camera.main.transform.position;

        //magic
        return Camera.main.transform.position +
            CameraToCursor() *
            (Vector3.Dot(Camera.main.transform.forward, camToTrans) / Vector3.Dot(Camera.main.transform.forward, CameraToCursor()));
        
    }


    void Update()
    {
        // Find clicked object
        if (Input.GetMouseButtonDown(0))
        {
            // Cast ray from the cursor on the screen
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                // If a coal has been clicked, select it
                if (hit.transform.CompareTag("Coal"))
                {
                    obj = hit.transform.GetComponent<Rigidbody>();
                    obj.useGravity = false;

                    soundSource.PlayOneShot(soundPickup);
                }
            }
        }

        // Drop object
        if (Input.GetMouseButtonUp(0) && obj != null)
        {
            if (obj.GetComponent<CoalScript>().inBurner)
			{
                //snap coal into place
                obj.transform.position = coalSnapPos[burner.coalCount - 1].position;
			}
            else
			{
                // The coal should drop onto the table
                obj.useGravity = true;
            }

            // Remove ref to object and play sound
            obj = null;
            soundSource.PlayOneShot(soundDrop);
        }


        // While an object is selected, move it
        if (obj != null)
        {
            // Get the cursor pos relative to the object transform
            Vector3 pos = CursorOnTransform(obj.position);
            // Set hight to the surface + set drag height
            pos.y = surfHeight + dragHeight;

            // Set the object's position
            obj.position = pos;
        }
    }
}