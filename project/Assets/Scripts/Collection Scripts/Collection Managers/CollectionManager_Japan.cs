using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager_Japan : CollectionManagerBase
{
    public float fadeTime = 1f;
    public float delayToShowObject = 1f;
    public float delayToTransition = 2f;

    public GameObject uiObject;


    protected override void AllCollected()
    {
        StartCoroutine(ShowObject(delayToShowObject));
        StartCoroutine(TransitionToTrain(delayToTransition));

        // Enable the collected object in the train carrage
        CollectionObjects_Japan.collected = true;
    }

    protected override void NewCollection(int itemID)
    {
        // Make the element fade in
        StartCoroutine(FadeIn(itemID, fadeTime));
    }


    private IEnumerator FadeIn(int itemID, float time)
    {
        //used to set color
        float num = 0;
        float scale = (1f / time);


        while (num != 1f)
        {
            num += Time.deltaTime * scale;

            // Change color
            uiElements[itemID].color = new Color(num, num, num);

            yield return null;
        }
    }

    private IEnumerator ShowObject(float time)
    {
        yield return new WaitForSeconds(time);

        // Hide the UI elements
        foreach (var elem in uiElements)
        { elem.gameObject.SetActive(false); }
        // Show the UI object
        uiObject.SetActive(true);
    }

    private IEnumerator TransitionToTrain(float time)
    {
        yield return new WaitForSeconds(time);

        // Transition to train with the player sitting in the booth
        TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-0.1f, 1f, 1.1f), true);
    }
}