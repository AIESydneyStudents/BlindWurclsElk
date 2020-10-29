using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSlotScript : MonoBehaviour
{
    [Tooltip("The tile value that is correct for this slot")]
    public char acceptedTileValue;

    // Is this slot in use?
    [HideInInspector]
    public bool used = false;
}