using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMessageManager : MonoBehaviour
{
    public static HelpMessageManager instance;


    // Is the help system curently being used?
    bool active = false;

    [SerializeField]
    float time;
    [SerializeField]
    DialogueSceneGraph helpMessage;


    void Awake()
    {
        instance = this;

        // If a help message has been set in the inspector, use it
        if (time > 0f && helpMessage != null)
        { active = true; }
    }

    void Update()
    {
        // If not helping, do nothing
        if (!active)
        { return; }


        time -= Time.deltaTime;
        if (time <= 0f)
        {
            // Play help message
            DialogueManager.instance.StartDialogue(helpMessage);

            // Deactivate
            active = false;
        }
    }

    /// <summary>
    /// Set how long to wait, and a message to display if the player fails to reach another progression item in time
    /// </summary>
    public void SetMessage(float delay, DialogueSceneGraph helpMessage)
    {
        // If no help message was sent, do nothing
        if (helpMessage == null)
        {
            active = false;
            return;
        }

        // Update values
        time = delay;
        this.helpMessage = helpMessage;
        active = true;
    }
}