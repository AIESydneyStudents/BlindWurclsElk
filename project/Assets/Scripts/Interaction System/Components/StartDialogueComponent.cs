using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StartDialogueComponent : TriggerComponentBase
{
    public DialogueSceneGraph dialogueGraph;
    [Tooltip("If left empty, the DialogeManager's default dialogue player is used")]
    public AudioSource dialoguePlayer = null;


    public override void Activate()
    {
        // Start dialogue
        DialogueManager.instance.StartDialogue(dialogueGraph, dialoguePlayer);
        // Destroy this script so the dialogue cant be played again
        Destroy(this);
    }
}