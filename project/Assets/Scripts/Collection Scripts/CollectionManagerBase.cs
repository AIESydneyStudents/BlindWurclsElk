using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CollectionManagerBase : MonoBehaviour
{
    // How many items still need to be collected?
    protected int count;

    [Tooltip("Add each UI element here. The number of UI elements is also the number of items to be collected")]
    public Image[] uiElements;


    private void Awake()
    {
        // Get the number of items that need to be collected
        count = uiElements.Length;
    }




    /// <summary>
    /// This is to be called by the collectable TriggerComponent
    /// </summary>
    public virtual void Collected(int itemID)
    {
        // Call abstract function for effect
        NewCollection(itemID);

        count--;
        // If all items have been collected, call AllCollected
        if (count == 0)
        { AllCollected(); }
    }


    /// <summary>
    /// Called when all items in the collection have been collected (NewCollection() is also called)
    /// </summary>
    protected abstract void AllCollected();

    /// <summary>
    /// Called each time an item is collected
    /// </summary>
    /// <param name="itemID">The ID for the item collected (use to index uiElements)</param>
    protected abstract void NewCollection(int itemID);
}