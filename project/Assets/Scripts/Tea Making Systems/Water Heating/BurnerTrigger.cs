using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class BurnerTrigger : MonoBehaviour
{
    public Transform camPos;
    
    public GameObject waterHeatGroup;
    public GameObject teaPot;
    public GameObject powderPourGroup;
    public GameObject mixingBowl;

    public Text helpText;
    [Space]

    public ParticleSystem emberParticleSys;
    ParticleSystem.MainModule emberParticleMain;
    Material emberParticleMat;

    public ParticleSystem steamParticleSys;
    ParticleSystem.MainModule steamParticleMain;

    [HideInInspector]
    public int coalCount = 0;
    float barValue = 0;


    public GameObject barObj;
    public GameObject barIndicator;
    RectTransform barScalable;

    [Tooltip("The total number of coals in the scene")]
    public int totalCoalCount = 3;
    [Tooltip("How many coals does the player need to win")]
    public int goalCoalCount = 2;

    // Used to determine if the minigame has been won
    float winTimmer = 0;


    void Start()
    {
        // Make it so the cursor isnt locked after unpausing
        FindObjectOfType<PauseMenuScript>().inMinigame = true;

        //move cam
        StartCoroutine(MoveCam());


        // Enable the bar
        barObj.SetActive(true);
        barIndicator.SetActive(true);
        // Get the part of the bar to scale
        barScalable = barObj.transform.Find("BarScalable").GetComponent<RectTransform>();

        //get particle system variables
        emberParticleMain = emberParticleSys.main;
        emberParticleMat = emberParticleSys.GetComponent<ParticleSystemRenderer>().material;
        steamParticleMain = steamParticleSys.main;

        // Set help text
        helpText.text = "Place the appropriate ammount of coals into the hot plate to heat the teapot";
        helpText.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        // If a coal has been added, update count and ease bar
        if (other.CompareTag("Coal"))
        {
            coalCount++;
            StartCoroutine(EaseBar(2));

            // Make particles grow orange
            emberParticleMain.startColor = new Color(1, .58f, 0f);
            emberParticleMat.EnableKeyword("_EMISSION");

            // Make coals glow
            other.GetComponent<CoalScript>().SetGlow(true);
            // Update steam
            StartCoroutine(SetSteamVisiblity());
        }
    }

    void OnTriggerExit(Collider other)
    {
        // If a coal has been removed, update count and ease bar
        if (other.CompareTag("Coal"))
        {
            coalCount--;
            StartCoroutine(EaseBar(2));

            if (coalCount == 0)
			{
                // Reset particles to red
                emberParticleMain.startColor = new Color(1, 0, 0);
                emberParticleMat.DisableKeyword("_EMISSION");
            }

            // Make coal stop glowing
            other.GetComponent<CoalScript>().SetGlow(false);
            // Update steam
            StartCoroutine(SetSteamVisiblity());
        }
    }


    void Update()
    {
        // Update bar scale
        barScalable.localScale = new Vector3(barValue, 1, 1);


        // If the target count has been reached, start win timmer
        if (coalCount == goalCoalCount)
        {
            winTimmer += Time.deltaTime;

            if (winTimmer >= 4f)
            {
                // Game won. start next minigame
                StartCoroutine(FinishMinigame());
                this.enabled = false;
            }
        }
        else
        {
            // Reset timmer
            winTimmer = 0f;
        }
    }

    private IEnumerator EaseBar(float time)
    {
        int targetCoalCount = coalCount;
        float targetbarValue = (float)coalCount / (float)totalCoalCount;

        // Ammount to add over time
        float dif = (targetbarValue - barValue) / time;

        bool isPositive = dif > 0f;

        // While not at the target value
        while (!(isPositive) ? barValue >= targetbarValue : barValue <= targetbarValue)
        {
            barValue += dif * Time.deltaTime;

            // If count has changed, stop
            if (targetCoalCount != coalCount)
            { break; }

            yield return null;
        }
    }

    private IEnumerator FinishMinigame()
    {
        //hide bar
        barObj.SetActive(false);
        barIndicator.SetActive(false);

        // move teapot back
        float time = 0;
        while (time < 0.5f)
		{
            teaPot.transform.localPosition -= new Vector3(0.26f * (Time.deltaTime * 2), 0, 0);

            time += Time.deltaTime;
            yield return null;
		}

        //move teapot down
        time = 0;
        while (time < 0.5f)
		{
            teaPot.transform.localPosition -= new Vector3(0, 0.1157f * (Time.deltaTime * 2), 0);

            time += Time.deltaTime;
            yield return null;
        }


        //change active group
        waterHeatGroup.SetActive(false);
        powderPourGroup.SetActive(true);
        //show bowl for powder pouring
        mixingBowl.SetActive(true);
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

    private IEnumerator SetSteamVisiblity()
	{
        float time = 0f;
        Color startCol = steamParticleMain.startColor.color;
        Color endCol = new Color(1, 1, 1, 0.1f * coalCount);

        while (time < 1f)
        {
            steamParticleMain.startColor = Color.Lerp(startCol, endCol, time);

            time += Time.deltaTime;
            yield return null;
        }
    }
}