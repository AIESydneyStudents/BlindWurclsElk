using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StiringScript : MonoBehaviour
{
    public Transform camPos;

    public GameObject barObj;
    public Image targetZone;
    RectTransform bar;

    public ParticleSystem particleSys;
    ParticleSystem.EmissionModule emissionModule;
    ParticleSystem.MainModule mainModule;
    float baseEmissionRate;
    float baseStartSize;

    public GameObject teaStirringObjects;
    public TeaPouringScript teaPourScript;
    public GameObject teaPouringObjects;

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
                //start next minigame
                CoroutineRunner.RunCoroutine(FinishMinigame());
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

    private IEnumerator FinishMinigame()
    {
        //hide bar
        barObj.SetActive(false);
        targetZone.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);


        particleSys.Stop();
        teaPourScript.enabled = true;

        //change active group
        teaStirringObjects.SetActive(false);
        teaPouringObjects.SetActive(true);
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