using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectComponent : TriggerComponentBase
{
    public GameObject obj;

    public override void Activate()
    {
        obj.SetActive(true);
    }
}