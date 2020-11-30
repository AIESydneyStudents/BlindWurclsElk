using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider)), DisallowMultipleComponent]
public class InteractionBase : MonoBehaviour
{
    [Tooltip("The material used when the object is able to be interacted with")]
    public Material highlighted;
    [Tooltip("Can be used to display prompts while hovering over object")]
    public GameObject uiPanel;

    // The material used when not highlighted, and is set to the objects default
    private Material normal;
    private MeshRenderer render;

    private TriggerComponentBase[] components;


    void Awake()
    {
        // Set tag to allow the player to interact
        tag = "Interactable";

        // Get the mesh renderer and the current material
        if (TryGetComponent<MeshRenderer>(out render))
        {
            normal = render.material;
        }

        // If a highlighted material is not selected, use the default
        if (highlighted == null)
        {
            highlighted = normal;
        }
    }

    void Start()
    {
        // Get attached component scripts
        components = GetComponents<TriggerComponentBase>();
    }


    /// <summary>
    /// Highlight or un-highlight the object.
    /// Should be called when the player is able to interact
    /// </summary>
    /// <param name="highlight">If true, the object is highlighted</param>
    public void SetHighlight(bool highlight = true)
    {
        // Set object material
        if (render != null)
        {
            render.material = highlight ? highlighted : normal;
        }

        // Show or hide UI panel
        if (uiPanel != null)
        { uiPanel.SetActive(highlight); }
    }

    /// <summary>
    /// Activate the trigger
    /// </summary>
    public void Use()
    {
        // Activate each attached component if it is enabled
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i].enabled)
            {
                components[i].Activate();
            }
        }
    }
}