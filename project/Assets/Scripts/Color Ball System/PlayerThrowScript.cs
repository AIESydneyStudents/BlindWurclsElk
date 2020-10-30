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
    [Min(0)]
    public int projCount;


    void Start()
    {
        // Cant throw without a projectile or fire point
        if (projectile == null || firePoint == null)
        { this.enabled = false; }
    }

    void Update()
    {
        // When the player clicks and has ammo
        if (Input.GetMouseButtonDown(0) && projCount > 0)
        {
            // Instantiate the projectile and apply a force to its rigidbody
            GameObject proj = Instantiate(projectile, firePoint.position, firePoint.rotation);
            proj.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, throwForce), ForceMode.Impulse);

            projCount--;
        }
    }
}