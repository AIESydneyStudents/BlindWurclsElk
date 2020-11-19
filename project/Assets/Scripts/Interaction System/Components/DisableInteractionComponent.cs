using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInteractionComponent : TriggerComponentBase
{
    public override void Activate()
    {
        // Get the interaction script
        InteractionBase script = GetComponent<InteractionBase>();

        // Stop highlighting and destroy the script
        script.SetHighlight(false);
        Destroy(script);
    }
}