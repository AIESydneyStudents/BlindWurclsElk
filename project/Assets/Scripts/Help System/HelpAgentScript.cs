using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelpAgentScript : MonoBehaviour
{
    public Vector3 targetPos;

    [Tooltip("When the object is this far from the target, it will be destroyed")]
    public float distance;


    void Start()
    {
        // Set the AI target
        GetComponent<NavMeshAgent>().SetDestination(targetPos);
    }

    void Update()
    {
        // When the object gets close enough to the target, destroy it
        if (Vector3.Distance(transform.position, targetPos) < distance)
		{
            Destroy(gameObject);
		}
    }
}