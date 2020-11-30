using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneComponent : TriggerComponentBase
{
    public string sceneName;
    [Space]
    public bool changePos = false;
    public Vector3 pos;
    [Space]
    public bool changeRot = false;
    public Vector3 rot;
    [Space]
    public bool useSitting = false;



    public override void Activate()
    {
        TransitionManager.instance.ChangeScene(sceneName, (changePos) ? (Vector3?)pos : null, (changeRot) ? (Vector3?)rot : null, useSitting);
    }
}