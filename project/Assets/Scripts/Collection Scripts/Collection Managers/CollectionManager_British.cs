using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionManager_British : CollectionManagerBase
{
    public float fadeTime = 1f;
    public float delayToMinigame = 2f;


    protected override void AllCollected()
    {
        Invoke("StartMinigame", delayToMinigame);
    }

    private void StartMinigame()
	{
        // Stat minigame
        SceneManager.LoadScene("Britain_PuzzleScene", LoadSceneMode.Additive);

        // Enable the collected object in the train carrage
        CollectionObjects_British.collected = true;
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
}