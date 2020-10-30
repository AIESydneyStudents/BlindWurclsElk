using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColorBallScript : MonoBehaviour
{
    // Particle effect prefab
    public GameObject particleEffect;





    void OnCollisionEnter(Collision collision)
    {
        if (particleEffect != null)
        {
            // Instantiate particle effect facing away from collision normal, and play effect
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.Euler(collision.GetContact(0).normal));
            effect.GetComponent<ParticleSystem>().Play();
        }
        

        //paint surf


        // Destroy the projectile
        Destroy(gameObject);
    }
}