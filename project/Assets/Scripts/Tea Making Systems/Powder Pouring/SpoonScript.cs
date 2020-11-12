using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class SpoonScript : MonoBehaviour
{
    //how high the surface is from 0
    public float surfHeight;
    //how high the object should be lifted while being moved
    public float dragHeight = 1f;

    [HideInInspector]
    public bool hasPowder = false;



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
        // Get the cursor pos relative to the object transform
        Vector3 pos = CursorOnTransform(transform.position);
        // Set hight to the surface + set drag height
        pos.y = surfHeight + dragHeight;

        // Set the object's position
        transform.position = pos;


        //temp make it green if has powder
        if (hasPowder)
        { GetComponent<MeshRenderer>().material.color = Color.green; }
        else 
        { GetComponent<MeshRenderer>().material.color = Color.white; }
    }
}