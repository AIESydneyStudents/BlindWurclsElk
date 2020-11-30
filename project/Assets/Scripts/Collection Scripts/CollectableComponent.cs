using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableComponent : TriggerComponentBase
{
    [Min(0)]
    public int itemID = 0;

    public CollectionManagerBase collectionManager;


    public override void Activate()
    {
        // Call func to indicate that this item has been collected
        collectionManager.Collected(itemID);

        // Destroy the object now that its been collected
        Invoke("DestroyObject", 0f);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}