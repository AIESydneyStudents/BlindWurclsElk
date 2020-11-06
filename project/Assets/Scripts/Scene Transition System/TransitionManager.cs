using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    /*  work out how the anim controller will work  */

    GameObject player;

    Scene currentScene;


    //  used by 2nd func
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
            SceneManager.LoadScene(0);
        }

        // If there are less than 2 scenes loaded, load the train carrage
        if (SceneManager.sceneCount < 2)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
    void Start()
    {
        //get the player
        player = GameObject.FindGameObjectWithTag("Player");

        //the player scene should be at index 0. this may need changing
        currentScene = SceneManager.GetSceneAt(1);
    }


    public void ChangeScene(string sceneName, /*enum for anim,*/ Vector3? position = null, Vector3? rotation = null, bool useSitting = false)
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


        //start anim
        //  it will call LoadNewScene
        //  if can get animation length, call func on delay

        //temp
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


        //set animator toggle


        //get new scene
        currentScene = SceneManager.GetSceneAt(1);
        //done, remove ref
        loadOp = null;
    }
}