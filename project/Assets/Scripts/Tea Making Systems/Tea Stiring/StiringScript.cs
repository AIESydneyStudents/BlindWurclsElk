using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StiringScript : MonoBehaviour
{
    public Transform camPos;
    public Transform endCamPos;

    public GameObject barObj;
    public Image targetZone;
    RectTransform bar;

    public ParticleSystem particleSys;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.MainModule mainModule;
    float baseEmissionRate;
    float baseStartSize;

    public Image blackFadeImg;
    public GameObject endGameStuff;
    public GameObject teaMakingStuff;

    bool active = true;

    public Text helpText;

    public AudioSource soundPlayer;
    [Space]
    
    // Point to rotate around
    public Vector3 centerPoint;
    // Distance to rotate around
    public float radius;
    [Space]

    // How fast should the player be stirring?
    public float targetSpeed;
    //how far out can it be and still count?
    public float targetRange;


    // Used for smoothing
    float rotSpeed = 0f;
    float vel = 0f;

    float winTimmer = 0;
    // How long the player needs to stay in target to win
    public float timeToWin;

    // Colors used for the target zone
    public Color start = Color.red;
    public Color end = Color.green;


    void Start()
    {
        //move cam
        StartCoroutine(MoveCam());

        // Show the bar
        barObj.SetActive(true);
        targetZone.gameObject.SetActive(true);
        // Get the scalable part of the bar
        bar = barObj.transform.Find("BarScalable").GetComponent<RectTransform>();

        particleSys.gameObject.SetActive(true);
        // Get main and emission modules from particle system
        emissionModule = particleSys.emission;
        mainModule = particleSys.main;

        baseEmissionRate = emissionModule.rateOverTimeMultiplier;
        baseStartSize = mainModule.startSizeMultiplier;

        soundPlayer.Play();

        // Get center point in world space
        centerPoint = transform.TransformPoint(centerPoint);


        helpText.text = "Stir the whisk until the tea is ready to serve";
    }


    // Find the cursor position in world space relative to the object position
    private Vector3 CursorOnTransform(Vector3 objectPosition)
    {
        //find the cursor in world space
        Vector3 CameraToCursor()
        {
            return Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane)) - Camera.main.transform.position;
        }

        //transform relative to cam
        Vector3 camToTrans = objectPosition - Camera.main.transform.position;

        //magic
        return Camera.main.transform.position +
            CameraToCursor() *
            (Vector3.Dot(Camera.main.transform.forward, camToTrans) / Vector3.Dot(Camera.main.transform.forward, CameraToCursor()));

    }

	void Update()
	{
        if (!active)
        { return; }

        // If the roatation speed is within the target
		if (rotSpeed >= targetSpeed - targetRange && rotSpeed <= targetSpeed + targetRange)
		{
            winTimmer += Time.deltaTime;

            // If timmer is done, the player wins
            if (winTimmer >= timeToWin)
			{
                //fade to black for end sequence
                CoroutineRunner.RunCoroutine(EndGame());

                active = false;
			}
		}
        else if (winTimmer > 0f)
		{
            // Time goes down at slower rate
            winTimmer -= Time.deltaTime * 0.5f;
		}

        // Lerp between colors to indicate progress
        targetZone.color = Color.Lerp(start, end, winTimmer / timeToWin);

        //modify sound effect's pitch by speed
        soundPlayer.pitch = rotSpeed * 2f;

        //change particle emission rate and size
        emissionModule.rateOverTimeMultiplier = baseEmissionRate * rotSpeed;
        mainModule.startSizeMultiplier = baseStartSize * rotSpeed;
    }

	void FixedUpdate()
    {
        // Get cursor position relative to this object
        Vector3 pos = CursorOnTransform(transform.position);

        // Make it relative to the center point and remove the height component
        pos -= centerPoint;
        pos.y = 0f;

        // Clamp the position around the center point
        pos.Normalize();
        pos *= radius;
        pos += centerPoint;


        // Get the angle traveled around the center
        float ang = Vector3.Angle(pos - centerPoint, transform.position - centerPoint);

        // Get speed, and smooth it
        ang *= Time.fixedDeltaTime;
        rotSpeed = Mathf.SmoothDamp(rotSpeed, ang, ref vel, 0.2f);


        // Update the spoons position
        transform.position = pos;
        // Update the bar, clamping the value
        bar.localScale = new Vector3(Mathf.Clamp(rotSpeed, 0, 1), 1, 1);
    }


    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1);

        blackFadeImg.gameObject.SetActive(true);

        //fade out

        while (blackFadeImg.color.a < 1f)
        {
            blackFadeImg.color += new Color(0, 0, 0, Time.deltaTime * 0.5f);

            yield return null;
        }

        //*****************************************
        //show tea masters, remove minigame

        endGameStuff.SetActive(true);
        teaMakingStuff.SetActive(false);
        helpText.gameObject.SetActive(false);
        barObj.SetActive(false);

        //make the player look slightly downward to be able to see the table
        Camera.main.transform.position = endCamPos.position;
        Camera.main.transform.rotation = endCamPos.rotation;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        yield return new WaitForSeconds(0.1f);

        //***********************************
        //fade in

        while (blackFadeImg.color.a > 0f)
        {
            blackFadeImg.color -= new Color(0, 0, 0, Time.deltaTime * 0.5f);

            yield return null;
        }

        blackFadeImg.gameObject.SetActive(false);

        //dialogue
        DialogueManager.instance.StartDialogue(endGameStuff.GetComponent<DialogueSceneGraph>());

        //********************************
        //exit game to menu

        yield return new WaitForSeconds(10);

        //return to menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    private IEnumerator MoveCam()
    {
        float timePassed = 0f;

        Vector3 startPos = Camera.main.transform.position;
        Quaternion startRot = Camera.main.transform.rotation;

        Transform cam = Camera.main.transform;


        while (timePassed < 1f)
        {
            cam.position = Vector3.Lerp(startPos, camPos.position, timePassed);
            cam.rotation = Quaternion.Lerp(startRot, camPos.rotation, timePassed);

            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}