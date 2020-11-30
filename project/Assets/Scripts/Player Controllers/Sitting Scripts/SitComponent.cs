using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitComponent : TriggerComponentBase
{
    public SeatInfo seat;


    public override void Activate()
    {
        //get sitting controller from player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SittingController sitController = player.GetComponent<SittingController>();

        //enable the controller and start sitting (disables the PlayerController)
        sitController.enabled = true;
        sitController.StartSitting(seat);
    }
}