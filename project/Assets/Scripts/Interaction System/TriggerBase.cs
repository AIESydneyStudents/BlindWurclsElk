using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), DisallowMultipleComponent]
public class TriggerBase : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        // If the player enters the trigger, activate
        if (other.CompareTag("Player"))
        {
            // Activate each attached component
            foreach (TriggerComponentBase comp in GetComponents<TriggerComponentBase>())
            {
                comp.Activate();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If the player exits the trigger, deactivate
        if (other.CompareTag("Player"))
        {
            // Deactivate each attached component
            foreach (TriggerComponentBase comp in GetComponents<TriggerComponentBase>())
            {
                comp.Deactivate();
            }
        }
    }
}