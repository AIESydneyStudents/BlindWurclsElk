using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager_India : CollectionManagerBase
{
    public float fadeTime = 1f;
    public float delayToShowObject = 1f;
    public float delayToTransition = 2f;


    protected override void AllCollected()
    {
        //enable throwing minigame
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

    private IEnumerator TransitionToTrain(float time)
    {
        yield return new WaitForSeconds(time);

        // Transition to train with the player sitting in the booth
        TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-7f, 1f, 1.7f), true);
    }
}