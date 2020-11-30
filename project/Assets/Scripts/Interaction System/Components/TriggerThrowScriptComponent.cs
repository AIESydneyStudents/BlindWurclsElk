using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerThrowScriptComponent : TriggerComponentBase
{
    public override void Activate()
    {
        // Enable the players throw script
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerThrowScript>().enabled = true;
    }
}