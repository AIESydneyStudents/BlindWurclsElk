using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSittingComponent : TriggerComponentBase
{
    public override void Activate()
    {
        // Disable the players sitting controller
        GameObject.FindGameObjectWithTag("Player").GetComponent<SittingController>().enabled = false;
        // Make cursor avalable
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //rotate cam down
        Camera.main.transform.localRotation = Quaternion.Euler(30, 0, 0);
    }
}