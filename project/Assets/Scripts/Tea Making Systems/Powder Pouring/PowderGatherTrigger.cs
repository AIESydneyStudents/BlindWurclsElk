using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowderGatherTrigger : MonoBehaviour
{
    public GameObject bar;
    public BarScript barScript;
    public SpoonScript spoonScript;

    bool active = false;

    //area the player should click
    [Range(0, 1)]
    public float range = 0.2f;



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            bar.SetActive(true);
            active = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            bar.SetActive(false);
            active = false;
        }
    }


    void Update()
    {
        // Dont run if the trigger is not active
        if (!active)
        { return; }

        if (Input.GetMouseButtonDown(0))
        {
            // Use the absolute value from half way, since the 'sweet spot' is in the center
            float barValue = Mathf.Abs(barScript.scaledPos - 0.5f);

            // Hit the 'sweet spot'
            if (barValue <= range / 2f)
            {
                //play anim to get powder

                // Set bool on spoon script
                spoonScript.hasPowder = true;
            }
            // Missed it
            else
            {
                //shake bar, sound
            }
        }
    }
}