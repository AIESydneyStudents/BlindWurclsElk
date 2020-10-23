using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SittingController : MonoBehaviour
{
    public Camera playerCamera;

    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float lookYLimit = 45.0f;



    //can the player currently look around?
    bool isSitting = false;


    //the seat the player is currently in
    SeatInfo seat;


    //current rotation
    float rotationX = 0;
    float rotationY = 0;



    //call when script activated. not in start because it needs param
    public void StartSitting(SeatInfo seat = null)
    {
        //disable the player controller
        GetComponent<PlayerController>().enabled = false;


        // If no seat is passed, find nearby seat to use
        if (seat == null)
        {
            //only need 1 seat
            Collider[] res = new Collider[1];

            //do a sphere cast with a layer mask for 'Seats' layer
            if (Physics.OverlapSphereNonAlloc(transform.position, 1, res, 1 << 8) > 0)
            {
                seat = res[0].GetComponent<SeatInfo>();
            }
            //if no seat, cant sit
            else
            {
                //use player controller
                GetComponent<PlayerController>().enabled = true;
                this.enabled = false;

                return;
            }
        }


        //use seat info
        this.seat = seat;
        //reset rot
        rotationY = seat.forwardRotation;
        rotationX = playerCamera.transform.rotation.eulerAngles.x;
        // Negitive rotations loop to 360
        if (rotationX > 180)
        { rotationX -= 360; }


        //'anim' moves player to standing pos quickly, then to sitting pos, then sets isSitting to true
        StartCoroutine(EaseToSit(0.2f, 1f));
    }

    //call when script deactivated
    public void StopSiting()
    {
        //no control
        isSitting = false;

        //anim to stand pos (enables player controller at end)
        StartCoroutine(EaseToStand(1f));
    }


    // Placeholder for proper animation to move player into seat
    private IEnumerator EaseToSit(float tStandPos, float tSitPos)
    {
        Vector3 initPos = transform.position;

        //run for tStandPos
        for (float timePassed = 0f; timePassed < tStandPos; timePassed += Time.deltaTime)
        {
            //lerp from where the player is standing to the seats standing pos
            transform.position = Vector3.Lerp(initPos, seat.standingPos, timePassed);

            yield return null;
        }

        //*************************************************

        //lerp rotation vars
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(new Vector3(0, seat.forwardRotation, 0));

        //run for tSitPos
        for (float timePassed = 0f; timePassed < tSitPos; timePassed += Time.deltaTime)
        {
            //lerp from standing to sitting
            transform.position = Vector3.Lerp(seat.standingPos, seat.sittingPos, timePassed);
            //lerp from where the player is looking to forward int the seat
            transform.rotation = Quaternion.Lerp(startRot, endRot, timePassed);

            yield return null;
        }

        //**************************************************

        //anim done, can look
        isSitting = true;
    }

    private IEnumerator EaseToStand(float time)
    {
        //run for time
        for (float timePassed = 0f; timePassed < time; timePassed += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(seat.sittingPos, seat.standingPos, timePassed);

            yield return null;
        }


        //activate player controller
        GetComponent<PlayerController>().enabled = true;
        this.enabled = false;
    }

    

    void Update()
    {
        //if not sitting, dont do anything
        if (!isSitting)
        { return; }



        //stand up on space
        if (Input.GetKeyDown(KeyCode.Space))
        { StopSiting(); }



        //(up and down)
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        //(left and right)
        //y rotation needs to be clamped, relative to an abstract 'forward' (seat.forwardRotation)
        rotationY += -Input.GetAxis("Mouse X") * lookSpeed;
        rotationY = Mathf.Clamp(rotationY, seat.forwardRotation - lookYLimit, seat.forwardRotation + lookYLimit);


        //apply rotation
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation = Quaternion.Euler(0, -rotationY, 0);
    }
}