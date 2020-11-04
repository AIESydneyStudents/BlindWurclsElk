using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTriggerComponent : TriggerComponentBase
{
    public TriggerComponentBase[] components;

    [Min(0)]
    public float delay;


    void Start()
    {
        // Disable each component
        foreach (var comp in components)
        {
            comp.enabled = false;
        }
    }


    public override void Activate()
    {
        StartCoroutine(ActivateOnDelay());
    }

    private IEnumerator ActivateOnDelay()
    {
        yield return new WaitForSeconds(delay);

        // Activate the components
        foreach (var comp in components)
        {
            comp.Activate();
        }
    }
}