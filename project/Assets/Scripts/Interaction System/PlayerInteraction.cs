using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // The position of the players camera is used for raycasting
    private Transform playerCam;
    // The interactable object being looked at
    private InteractionBase selected = null;

    // Used for the distance of the raycast
    [Tooltip("The range an interactable object can be used in")]
    public float interactableDistance = 5f;


    void Awake()
    {
        // Get the scene camera
        playerCam = FindObjectOfType<Camera>().transform;
    }

    void Update()
    {
        // If the player is looking at an interactable object in range
        if (Physics.Raycast(playerCam.position, playerCam.forward, out RaycastHit hit, interactableDistance) && hit.transform.CompareTag("Interactable"))
        {
            // Get the objects interaction script
            InteractionBase newSelection = hit.transform.GetComponent<InteractionBase>();

            // If the object hasnt already been selected
            if (selected != newSelection)
            {
                // If another object was selected, unselect it
                if (selected != null)
                {
                    selected.SetHighlight(false);
                }

                // Select the new object
                selected = newSelection;
                selected.SetHighlight(true);
            }
        }
        else
        {
            // If the player stops looking at an object, unselect it
            if (selected != null)
            {
                selected.SetHighlight(false);
                selected = null;
            }
        }


        // If the player clicks with something selected, use it
        if (Input.GetMouseButtonDown(0) && selected != null)
        {
            selected.Use();
        }
    }
}