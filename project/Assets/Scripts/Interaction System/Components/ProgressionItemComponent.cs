using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionItemComponent : TriggerComponentBase
{
    // Used to prevent a help message being displayed multiple times
    bool active = true;


    public DialogueSceneGraph helpMessage;

    [Min(0)]
    public float waitTime;


    public override void Activate()
    {
        if (active)
        {
            // Set the help message
            HelpMessageManager.instance.SetMessage(waitTime, helpMessage);
            active = false;
        }
    }
}