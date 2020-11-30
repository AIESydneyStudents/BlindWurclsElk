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
        float scale = (1f / time);

        for (float timePassed = 0; timePassed < 1f; timePassed += Time.deltaTime * scale)
        {
            // Change color
            uiElements[itemID].color = new Color(timePassed, timePassed, timePassed);

            yield return null;
        }
    }
}