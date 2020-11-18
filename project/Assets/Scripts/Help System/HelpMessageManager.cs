using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMessageManager : MonoBehaviour
{
    public static HelpMessageManager instance;

    Transform player;


    [Tooltip("The agent instantiated after the player hasnt progressed")]
    public GameObject agent;

    [SerializeField]
	float timeToWait;
    [SerializeField]
    GameObject target;

    // Timmer for creating agents
    float timeSinceLastAgent = 0f;
    [Tooltip("How long to wait between agents")]
    public float agentDelay = 1f;


    void Awake()
    {
        instance = this;
    }

	void Start()
	{
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
    {
        // Time has passed since the player collected a progression item
        if (timeToWait <= 0f)
        {
            if (timeSinceLastAgent >= agentDelay)
			{
                // Create the agent at the player
                Instantiate(agent, player.position, player.rotation);

                //set agent's target
			}
			else
			{
                timeSinceLastAgent += Time.deltaTime;
			}
        }
		else
		{
            timeToWait -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Set how long to wait, and the target for agents to go to if the player fails to reach another progression item in time
    /// </summary>
    public void SetTarget(float delay, GameObject target)
    {
        // Update values
        timeToWait = delay;
        this.target = target;
        timeSinceLastAgent = 0f;
    }
}