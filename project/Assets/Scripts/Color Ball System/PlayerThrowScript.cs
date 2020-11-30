using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowScript : MonoBehaviour
{
    [Tooltip("Where the projectile is instantiated")]
    public Transform firePoint;
    [Tooltip("The object being thrown")]
    public GameObject projectile;

    [Min(0)]
    public float throwForce;

    [Space]
    [Tooltip("The color balls to be thrown. The number of balls to throw == the number of colors")]
    public Color[] colors;


    bool canThrow = true;
    // Used for getting a color
    int index;
    // Number of perjectiles thrown
    int projCount = 0;


    void Start()
    {
        // Cant throw without a projectile or fire point
        if (projectile == null || firePoint == null)
        { this.enabled = false; }

        // Get random starting color
        index = Random.Range(0, colors.Length);
    }

    void Update()
    {
        // When the player clicks and can throw
        if (Input.GetMouseButtonDown(0) && canThrow && projCount < colors.Length)
        {
            // Instantiate the projectile
            GameObject proj = Instantiate(projectile, firePoint.position, firePoint.rotation);
            // Apply force to it and set its color
            proj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, throwForce), ForceMode.Impulse);
            proj.GetComponent<ColorBallScript>().color = colors[index];


            // Add a listener for collision
            proj.GetComponent<ColorBallScript>().Contact.AddListener(OnProjHit);

            // The player cant throw multiple projectiles at a time
            canThrow = false;
        }
    }


    void OnProjHit(bool target)
    {
        // Has the player hit the mannequin?
        if (target)
        {
            // Move to next projectile color
            projCount++;
            index++;

            // Loop index to 0
            if (index >= colors.Length)
            { index = 0; }
        }


        // If the player has thrown all prjectiles, win state
        if (projCount == colors.Length)
        {
            this.enabled = false;
            // Transition to train with the player sitting in the booth
            TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-8f, 1f, 1f), null, true);
        }
        else 
        {
            canThrow = true;
        }
    }
}