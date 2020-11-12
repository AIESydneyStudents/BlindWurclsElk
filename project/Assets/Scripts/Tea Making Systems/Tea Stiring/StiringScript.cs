using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiringScript : MonoBehaviour
{
    public RectTransform bar;

    // Distance to rotate around
    public float radius;
    // Point to rotate around
    public Vector3 centerPoint;

    // Used for smoothing
    float rotSpeed = 0f;
    float vel = 0f;



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


    void FixedUpdate()
    {
        // Get cursor position relative to this object
        Vector3 pos = CursorOnTransform(transform.position);
        // Make it relative to the center point and remove the height component
        pos -= centerPoint;
        pos.y = 0f;

        // Put the cursor position on a rail
        pos.Normalize();
        pos *= radius;
        pos += centerPoint;

        
        // Get the angle traveled
        float ang = Vector3.Angle(pos, transform.position);
        // Get the smaller angle
        if (ang > 180)
        { ang -= 180; }

        // Get speed, and smooth it
        ang *= Time.fixedDeltaTime;
        rotSpeed = Mathf.SmoothDamp(rotSpeed, ang, ref vel, 0.2f);


        // Update the transform
        transform.position = pos;
        // Update the bar
        bar.localScale = new Vector3(rotSpeed * 3f, 1, 1);
    }
}