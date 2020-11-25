using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [HideInInspector]
    public Animator anim;

    GameObject player;
    public Image img;

    public RectTransform eyetop;
    public RectTransform eyebot;


    Scene currentScene;


    //used by 2nd func
    AsyncOperation loadOp;
    Vector3 pos;
    Vector3 rot;
    bool useSitting;


    void Awake()
    {
        instance = this;


        // If the player scene is not loaded, hard load it
        if (SceneManager.GetSceneByName("PlayerScene").IsValid() && SceneManager.GetSceneByName("SB-TransitionBase").IsValid())
        {
            SceneManager.LoadScene(1);
        }

        // If there are less than 2 scenes loaded, load the train carrage
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    }
    void Start()
    {
        //get the player
        player = GameObject.FindGameObjectWithTag("Player");

        //the player scene should be at index 0. this may need changing
        currentScene = SceneManager.GetSceneAt(1);
        // Set it as the active scene to use its lighting settings
        SceneManager.SetActiveScene(currentScene);
    }


    public void ChangeScene(string sceneName, Vector3? position = null, Vector3? rotation = null, bool useSitting = false)
    {
        //stop dialogue
        if (DialogueManager.instance != null)
        { DialogueManager.instance.EndDialogue(); }
        
        //disable controllers
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<SittingController>().enabled = false;

        //start loading new scene
        loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        //dont activate on load
        loadOp.allowSceneActivation = false;

        //set values for other function
        pos = (position == null) ? player.transform.position : position.Value;
        rot = (rotation == null) ? player.transform.rotation.eulerAngles : rotation.Value;
        this.useSitting = useSitting;


        //determine delay for unloading current scene
        float transDelay;
        if (anim == null)
		{
            //use blink transition
            
            transDelay = 2;
        }
		else
		{
            //start animation
            anim.enabled = true;
            //get animation clip length
            //transDelay = anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;

            transDelay = 2.5f;
        }
        StartCoroutine(BlinkOut());
        //start transition after delay
        Invoke("StartLoadNewScene", transDelay);
    }

    void StartLoadNewScene()
    {
        StartCoroutine(LoadNewScene());
    }

    //called after anim intro
    public IEnumerator LoadNewScene()
    {
        //anim loop has started


        //start unloading currentScene
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
        //with screen covered, new scene can be loaded
        loadOp.allowSceneActivation = true;


        //while something is still happening
        while (!loadOp.isDone || !unloadOp.isDone)
        {
            yield return null;
        }

        
        //update pos and rot. the char controller overrides the transform, so it needs to be disabled first
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = pos;
        player.transform.rotation = Quaternion.Euler(rot);
        player.GetComponent<CharacterController>().enabled = true;

        //activate sitting controller
        if (useSitting)
        {
            SittingController sitController = player.GetComponent<SittingController>();
            sitController.enabled = true;
            sitController.StartSitting();
        }
        //activate player controller
        else
        {
            player.GetComponent<PlayerController>().enabled = true;
        }


        //end animation
        if (anim == null)
		{
            //open eyes
            
        }
        else
		{
            Destroy(anim.gameObject);
            //set animator toggle and remove ref
            //anim.SetTrigger("EndLoop");
            anim = null;
		}
        StartCoroutine(BlinkIn());

        // Get the new scene and set it as the active scene
        currentScene = SceneManager.GetSceneAt(1);
        SceneManager.SetActiveScene(currentScene);
        // Remove ref to async operation
        loadOp = null;
    }


    private IEnumerator BlinkOut()
	{
        yield return new WaitForSeconds(0.5f);

        //activate objects
        eyetop.gameObject.SetActive(true);
        eyebot.gameObject.SetActive(true);


        for (float time = 0; time < 1f; time += Time.deltaTime)
		{
            //scale y from 0 to 1.3
            Vector3 scale = new Vector3(1, Mathf.Lerp(0, 1.3f, time), 1);
            eyetop.localScale = scale;
            eyebot.localScale = scale;

            yield return null;
		}
	}
    private IEnumerator BlinkIn()
	{
        yield return new WaitForSeconds(0.5f);

        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            //scale y from 1.3 to 0
            Vector3 scale = new Vector3(1, Mathf.Lerp(1.3f, 0, time), 1);
            eyetop.localScale = scale;
            eyebot.localScale = scale;

            yield return null;
        }

        //activate objects
        eyetop.gameObject.SetActive(false);
        eyebot.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        img.gameObject.SetActive(true);

        while (img.color.a < 1f)
        {
            img.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);

            yield return null;
        }
    }
    private IEnumerator FadeIn()
    {
        while (img.color.a > 0f)
        {
            img.color -= new Color(0, 0, 0, Time.deltaTime * 0.5f);

            yield return null;
        }

        img.gameObject.SetActive(false);
    }
}