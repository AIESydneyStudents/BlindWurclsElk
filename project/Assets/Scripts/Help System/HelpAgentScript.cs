using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelpAgentScript : MonoBehaviour
{
    public Vector3 targetPos;

    [Tooltip("When the object is this far from the target, it will be destroyed")]
    public float distance;

    NavMeshAgent agent;
    float speed;


    void Start()
    {
        // Set the AI target
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targetPos);

        speed = agent.speed;
        agent.angularSpeed = 0;
        agent.speed = 0;
    }

    void Update()
    {
        // When the object gets close enough to the target, destroy it
        if (Vector3.Distance(transform.position, targetPos) < distance)
		{
            Destroy(gameObject);
            return;
		}

        // Move agent manualy to prevent overshooting
        agent.Move((agent.steeringTarget - transform.position).normalized * speed * Time.deltaTime);
    }
}