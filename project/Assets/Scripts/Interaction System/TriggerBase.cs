using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), DisallowMultipleComponent]
public class TriggerBase : MonoBehaviour
{
    private TriggerComponentBase[] components;


    void Start()
    {
        // Get attached component scripts
        components = GetComponents<TriggerComponentBase>();
    }

    void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger, activate
        if (other.CompareTag("Player"))
        {
            // Activate each attached component
            for (int i = 0; i < components.Length; i++)
            {
                components[i].Activate();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player exits the trigger, deactivate
        if (other.CompareTag("Player"))
        {
            // Deactivate each attached component
            for (int i = 0; i < components.Length; i++)
            {
                components[i].Deactivate();
            }
        }
    }
}