using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Image img;


    public void OnPlayClick()
    {
        // Reset collections for new game
        CollectionObjects_British.collected = false;
        CollectionObjects_India.collected = false;
        CollectionObjects_Japan.collected = false;

        //fade to black
        StartCoroutine(FadeOut());
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }


    private IEnumerator FadeOut()
	{
        img.gameObject.SetActive(true);

        while (img.color.a < 1f)
        {
            img.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);

            yield return null;
        }

        // Loading the player scene will cause it to load the train scene by default
        SceneManager.LoadScene("PlayerScene");
    }
}