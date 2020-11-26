using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationComponent : TriggerComponentBase
{
    public Animator anim;


    public override void Activate()
    {
        anim.enabled = true;
    }
}