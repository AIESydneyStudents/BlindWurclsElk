using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    RectTransform trans;
    CanvasGroup canvasGroup;
    Vector3 mouseOffset;

    // Is the object currently being moved?
    bool moving = false;
    // The slot the tile is currently in, or null
    DragSlotScript usedSlot = null;


    // A slot of the same value will lock it
    public char tileValue;

    // Has the object been locked?
    [HideInInspector]
    public bool canBeMoved = true;
    


    void Awake()
    {
        // Get this rect transform and canvas group component
        trans = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        // Add to the manager's list of tiles
        DragManager.instance.tiles.Add(this);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        // If the object isnt locked
        if (canBeMoved)
        {
            // If in a slot, get out of it
            if (usedSlot != null)
            {
                usedSlot.used = false;
                usedSlot = null;
            }

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


        // Get range from manager
        float radius = DragManager.instance.slotSnapRange;

        // Get all slots
        DragSlotScript[] slots = FindObjectsOfType<DragSlotScript>();
        for (int i = 0; i < slots.Length; i++)
        {
            // If slot is not being used and is in range
            if (!slots[i].used && Vector3.Distance(slots[i].transform.position, trans.position) < radius)
            {
                // Use the slot
                trans.position = slots[i].transform.position;
                usedSlot = slots[i];
                slots[i].used = true;

                // If the slot accepts this tile, lock
                if (tileValue == slots[i].acceptedTileValue)
                {
                    canBeMoved = false;
                    canvasGroup.blocksRaycasts = false;
                }

                break;
            }
        }


        // Get manager to check for game over
        DragManager.instance.CheckTiles();
    }
}