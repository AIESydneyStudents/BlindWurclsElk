using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowderDepositTrigger : MonoBehaviour
{
    public GameObject powderPourGroup;
    public GameObject waterPourGroup;
    public TeaPotScript teapotScript;
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
                // Move spoon over bowl
                spoon.gameObject.transform.localPosition = transform.localPosition + new Vector3(-0.025f, 0, 0.05f);

                // Play anim
                spoon.AnimatePowder(true);

                // Play sound effect
                sound.Play();

                count++;

                
                // If full amount collected, end minigame
                if (count == numNeeded)
                {
                    //Game won. start next game
                    StartCoroutine(FinishMinigame());
                }
            }
        }
    }

    private IEnumerator FinishMinigame()
    {
        //hide bar
        barObj.SetActive(false);
        //disable spoon so it cant move after animation
        FindObjectOfType<SpoonScript>().enabled = false;

        yield return new WaitForSeconds(2.5f);

        
        teapotScript.enabled = true;

        //change active group
        powderPourGroup.SetActive(false);
        waterPourGroup.SetActive(true);
    }
}