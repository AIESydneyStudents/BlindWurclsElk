using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager_Test : CollectionManagerBase
{
    public float fadeTime = 1f;
    public float delayToShowObject = 1f;
    public float delayToTransition = 2f;

    public GameObject uiObject;



    protected override void AllCollected()
    {
        StartCoroutine(ShowObject(delayToShowObject));
        StartCoroutine(TransitionToTrain(delayToTransition));

        // Enable the collected object in the train carrage.
        // such a script should be created for every collection (new script as it uses a static)
        CollectedObject_Test.active = true;
    }

    protected override void NewCollection(int itemID)
    {
        //fade the element into color
        StartCoroutine(FadeIn(itemID, fadeTime));
    }


    private IEnumerator FadeIn(int itemID, float time)
    {
        //used to set color
        float num = 0;
        float scale = (1f / time);


        while (num != 1f)
        {
            //increment by time, scaled
            num += Time.deltaTime * scale;

            //change color
            uiElements[itemID].color = new Color(num, num, num);

            yield return null;
        }
    }

    private IEnumerator ShowObject(float time)
    {
        //wait time
        yield return new WaitForSeconds(time);

        //hide each element
        foreach (var val in uiElements)
        { val.gameObject.SetActive(false); }

        //show object
        uiObject.SetActive(true);
    }

    private IEnumerator TransitionToTrain(float time)
    {
        yield return new WaitForSeconds(time);

        // Transition to train with the player sitting in the booth
        TransitionManager.instance.ChangeScene("TrainCarriage", new Vector3(-0.1f, 1f, 1.1f), null, true);

    }
}