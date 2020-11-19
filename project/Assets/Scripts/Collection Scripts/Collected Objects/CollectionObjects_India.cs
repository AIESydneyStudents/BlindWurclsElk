using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjects_India : MonoBehaviour
{
    [Tooltip("Objects to toggle after all objects in india have been collected")]
    public GameObject[] objects;

    // Have all the items been collected?
    public static bool collected = false;

    [Space]
    public DialogueSceneGraph[] secondDialogue;
    public StartDialogueComponent[] comp;


    void Start()
    {
        // Invoke to allow the player to find the nearest chair before disabling it
        Invoke("Toggle", 0.1f);
    }

    private void Toggle()
    {
        foreach (var obj in objects)
        {
            // Toggle if collected == true
            obj.SetActive(obj.activeSelf ^ collected);
        }

        if (collected)
        { 
            // Swap each dialogue graph with the second one
            for (int i = 0; i < comp.Length; i++)
            {
                comp[i].dialogueGraph = secondDialogue[i];
            }
        }
    }
}