using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventNode : BaseNode
{
    [Tooltip("When this node is activated, this event will be invoked")]
    public UnityEvent Event;


    public override void Activate()
    {
        Event.Invoke();
    }
}