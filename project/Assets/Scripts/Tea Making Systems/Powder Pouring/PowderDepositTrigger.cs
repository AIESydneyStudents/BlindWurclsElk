using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowderDepositTrigger : MonoBehaviour
{
    public GameObject powderPourGroup;
    public GameObject teaStirGroup;
    public GameObject barObj;

    public AudioSource sound;

    // Number of times powder needs to be collected to win
    public int numNeeded;
    // Number of times powder has been collected so far
    int count = 0;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            SpoonScript spoon = other.GetComponent<SpoonScript>();

            if (spoon.hasPowder)
            {
                //play anim

                // Play sound effect
                sound.Play();

                spoon.hasPowder = false;
                count++;


                // If full amount collected, end minigame
                if (count == numNeeded)
                {
                    //Game won. start next game
                    powderPourGroup.SetActive(false);
                    teaStirGroup.SetActive(true);
                    barObj.SetActive(false);
                }
            }
        }
    }
}