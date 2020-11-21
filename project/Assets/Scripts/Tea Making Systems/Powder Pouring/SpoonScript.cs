using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class SpoonScript : MonoBehaviour
{
    // Y point of the surface in world space
    public float surfHeight;
    // Height of object from surface while dragging
    public float dragHeight = 1f;

    [HideInInspector]
    public bool hasPowder = false;
    bool locked = false;

    public GameObject teaObj;



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
        if (!locked)
		{
            // Get the cursor pos relative to the object transform
            Vector3 pos = CursorOnTransform(transform.position);
            // Set hight to the surface + set drag height
            pos.y = surfHeight + dragHeight;

            // Set the object's position
            transform.position = pos + new Vector3(0, 0, 0.04f);
        }


        // Show tea when the spoon has powder
        if (hasPowder)
        { teaObj.SetActive(true); }
        else 
        { teaObj.SetActive(false); }
    }

    public void GetPowder()
	{
        locked = true;

        // Play anim. Will also set hasPowder and locked
        StartCoroutine(AngleDown());
	}

    private IEnumerator AngleDown()
	{
        float time = 0;
        Quaternion start = transform.rotation;
        //rotate down
        Quaternion end = start * Quaternion.Euler(0, 0, 45);

        
        while (time < 1)
		{
            transform.rotation = Quaternion.Lerp(start, end, time);

            time += Time.deltaTime;
            yield return null;
		}

        // Show powder and start next animation
        hasPowder = true;
        StartCoroutine(AngleUp());
	}

    private IEnumerator AngleUp()
	{
        float time = 0;
        Quaternion start = transform.rotation;
        //rotate up
        Quaternion end = start * Quaternion.Euler(0, 0, -45);


        while (time < 1)
        {
            transform.rotation = Quaternion.Lerp(start, end, time);

            time += Time.deltaTime;
            yield return null;
        }

        // Unlock spoon
        locked = false;
    }
}