using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColorBallScript : MonoBehaviour
{
    // Particle effect prefab
    public GameObject particleEffect;

    public Texture2D splat;

    public Color color;




    void OnCollisionEnter(Collision collision)
    {
        if (particleEffect != null)
        {
            // Instantiate particle effect facing away from collision normal
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.Euler(collision.GetContact(0).normal + new Vector3(-90, 0, 0)));

            // Scale the effect to the size of the projectile
            effect.transform.localScale = transform.localScale;
            // Set the effect's color to this color
            var mainModule = effect.GetComponent<ParticleSystem>().main;
            mainModule.startColor = color;
        }


        //paint surf
        if (Physics.Raycast(transform.position, -collision.GetContact(0).normal, out RaycastHit hit))
        {
            if (hit.collider is MeshCollider)
            {
                PaintableScript script = hit.collider.gameObject.GetComponent<PaintableScript>();
                if (script != null)
                {
                    script.PaintOn(hit.textureCoord2, splat, color);
                }
            }
        }


        // Destroy the projectile
        Destroy(gameObject);
    }
}