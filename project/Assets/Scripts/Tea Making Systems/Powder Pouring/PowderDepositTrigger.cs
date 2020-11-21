using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PowderDepositTrigger : MonoBehaviour
{
    public GameObject powderPourGroup;
    public GameObject teaStirGroup;
    public GameObject teaObj;
    public GameObject barObj;

    public GameObject teaPowderObj;
    public ParticleSystem particleEffect;

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
                particleEffect.Play();

                // Play sound effect
                sound.Play();

                spoon.hasPowder = false;
                count++;

                teaPowderObj.SetActive(true);

                //make larger
                teaPowderObj.transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);


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

        yield return new WaitForSeconds(1);

        //show tea in bowl
        teaObj.SetActive(true);
        teaPowderObj.SetActive(false);

        //change active group
        powderPourGroup.SetActive(false);
        teaStirGroup.SetActive(true);
    }
}