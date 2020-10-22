using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // The current dialogue graph (gotten from a passed DialogueSceneGraph)
    private DialogueGraph graph;
    // The audio source sound is to be played from
    private AudioSource dialoguePlayer;
    

    [HideInInspector]
    public static DialogueManager instance;

    // The UI panel that dialogue is displayed on
    public GameObject uiPanel;
    // The UI element for dialogue text
    public Text uiText;

    [Tooltip("If an audio source is not given via script, this will be used instead")]
    public AudioSource defaultDialoguePlayer;



    void Awake()
    {
        // This is a singleton, with only one instance
        instance = this;
    }


    /// <param name="dialogueGraph">The scene graph containing the dialogue to be used</param>
    /// <param name="audioPlayer">The audio source that audio is to be played from</param>
    public void StartDialogue(DialogueSceneGraph dialogueGraph, AudioSource audioPlayer = null)
    {
        // Make the dialogue panel visible
        uiPanel.SetActive(true);

        // Use the audio source passed, or the default one. Dont loop
        dialoguePlayer = (audioPlayer == null) ? defaultDialoguePlayer : audioPlayer;
        if (dialoguePlayer != null)
        { dialoguePlayer.loop = false; }
        
        // Get the graph from the scene graph
        graph = dialogueGraph.graph;
        graph.Run();
    }

    /// <summary>
    /// Update the current dialogue
    /// </summary>
    public void ChangeDialogue(string text, AudioClip audio = null)
    {
        // Display text and play audio if passed
        uiText.text = text;

        if (audio != null && dialoguePlayer != null)
        {
            dialoguePlayer.clip = audio;
            dialoguePlayer.Play();
        }
    }

    /// <summary>
    /// Exit dialogue, hidding the dialogue panel
    /// </summary>
    public void EndDialogue()
    {
        // Hide the dialogue panel and stop playing audio
        uiPanel.SetActive(false);
        if (dialoguePlayer != null)
        { dialoguePlayer.Stop(); }
    }
}