using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform trans;
    Vector3 mouseOffset;

    // Is the object currently being moved?
    bool moving = false;


    [Tooltip("")]
    public char tileValue;

    // Has the object been locked?
    [HideInInspector]
    public bool canBeMoved = true;



    void Awake()
    {
        // Get this rect transform
        trans = GetComponent<RectTransform>();

        //add to manager list
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // If the object isnt locked
        if (canBeMoved)
        {
            // Get the offset for where the player has clicked the object
            mouseOffset = trans.position - (Vector3)eventData.position;

            // Allow the object to be moved
            moving = true;
        }
    }

    void Update()
    {
        // If the object is being moved, move it
        if (moving)
        {
            transform.position = Input.mousePosition + mouseOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Stop moving the object
        moving = false;


        //check for DragSlotScript in area
        //  is corrent tile val, canBeMoved = false

        //message manager
    }
}