using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager_Test : CollectionManagerBase
{
    public float fadeTime = 1f;


    protected override void AllCollected()
    {
        //temp for debugging
        Debug.Log("All items collected");
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
}