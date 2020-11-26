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
        float scale = (1f / time);

        for (float timePassed = 0; timePassed < 1f; timePassed += Time.deltaTime * scale)
        {
            // Change color
            uiElements[itemID].color = new Color(timePassed, timePassed, timePassed);

            yield return null;
        }
    }

    private IEnumerator ShowObject(float time)
    {
        yield return new WaitForSeconds(time);

        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].gameObject.SetActive(false);
        }

        // Show the UI object
        uiObject.SetActive(true);
    }

    private IEnumerator TransitionToTrain(float time)
    {
        yield return new WaitForSeconds(time);

        // Transition to train with the player sitting in the booth
        TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-0.1f, 1f, 1.1f), null, true);
    }
}