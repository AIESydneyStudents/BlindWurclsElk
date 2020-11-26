using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager_India : CollectionManagerBase
{
    public float fadeTime = 1f;
    public float delayToShowObject = 1f;
    public float delayToTransition = 2f;

    public GameObject trigger;


    protected override void AllCollected()
    {
        // Enable trigger to start minigame
        trigger.SetActive(true);

        // Enable the collected object in the train carrage
        CollectionObjects_India.collected = true;
    }

    protected override void NewCollection(int itemID)
    {
        // Make the element fade in
        StartCoroutine(FadeIn(itemID, fadeTime));
    }


    private IEnumerator FadeIn(int itemID, float time)
    {
        //used to set color
        float scale = (1f / time);

        for (float timePassed = 0; timePassed < 1f; timePassed += Time.deltaTime * scale)
        {
            // Change color
            uiElements[itemID].color = new Color(timePassed, timePassed, timePassed);

            yield return null;
        }
    }
}