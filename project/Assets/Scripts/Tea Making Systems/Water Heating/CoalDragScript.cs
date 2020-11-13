using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalDragScript : MonoBehaviour
{
    // Selected object to move
    Rigidbody obj = null;

    //how high the surface is from 0
    public float surfHeight;
    //how high the object should be lifted while being moved
    public float dragHeight = 1f;


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
                }
            }
        }

        // Drop object
        if (Input.GetMouseButtonUp(0) && obj != null)
        {
            obj.useGravity = true;
            obj = null;
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