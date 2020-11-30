using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class MyBoolEvent : UnityEvent<bool>
{
}


[RequireComponent(typeof(Collider))]
public class ColorBallScript : MonoBehaviour
{
    // Particle effect prefab
    public GameObject particleEffect;

    public Texture2D splat;
    public Color color;

    [HideInInspector]
    public UnityEvent<bool> Contact = new MyBoolEvent();


    void OnCollisionEnter(Collision collision)
    {
        if (particleEffect != null)
        {
            // Instantiate particle effect facing away from collision normal
            GameObject effect = Instantiate(particleEffect, transform.localPosition, Quaternion.Euler(collision.GetContact(0).normal + new Vector3(-90, 0, 0)));

            // Scale the effect to the size of the projectile
            effect.transform.localScale = transform.localScale;
            // Set the effect's color to this color
            var mainModule = effect.GetComponent<ParticleSystem>().main;
            mainModule.startColor = color;
        }


        //paint surf
        if (Physics.Raycast(transform.localPosition, -collision.GetContact(0).normal, out RaycastHit hit))
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


        //invoke event
        Contact.Invoke(collision.gameObject.CompareTag("Mannequin"));

        // Destroy the projectile
        Destroy(gameObject);
    }
}