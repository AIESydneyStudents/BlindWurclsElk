using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjects_Japan : MonoBehaviour
{
    [Tooltip("Objects to toggle after all objects in japan have been collected")]
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
        for (int i = 0; i < objects.Length; i++)
        {
            // Toggle if collected == true
            objects[i].SetActive(objects[i].activeSelf ^ collected);
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