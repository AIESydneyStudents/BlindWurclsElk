using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for composition, used by InteractionBase and TriggerBase
/// </summary>
public abstract class TriggerComponentBase : MonoBehaviour
{


    /// <summary>
    /// Called when the attached object is triggered/interacted with
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Called when exiting a trigger
    /// </summary>
    public virtual void Deactivate()
    { }
}