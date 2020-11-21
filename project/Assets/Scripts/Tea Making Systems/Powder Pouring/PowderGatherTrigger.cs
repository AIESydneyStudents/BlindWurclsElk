using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class PowderGatherTrigger : MonoBehaviour
{
    public Transform camPos;

    public GameObject bar;
    public BarScript barScript;
    public SpoonScript spoonScript;

    public Text helpText;
    [Space]

    public AudioSource sound;

    bool active = false;

    //area the player should click
    [Range(0, 1)]
    public float range = 0.2f;


    void Start()
    {
        //move cam
        StartCoroutine(MoveCam());

        helpText.text = "Collect tea powder and put it inot the bowl";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            bar.SetActive(true);
            active = true;

            //change help message
            helpText.text = "Click at the right time to collect the tea powder";
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spoon"))
        {
            bar.SetActive(false);
            active = false;

            //change help message
            helpText.text = "Collect tea powder and put it inot the bowl";
        }
    }


    void Update()
    {
        // Dont run if the trigger is not active
        if (!active)
        { return; }

        if (Input.GetMouseButtonDown(0))
        {
            // Use the absolute value from half way, since the 'sweet spot' is in the center
            float barValue = Mathf.Abs(barScript.scaledPos - 0.5f);

            // Hit the 'sweet spot'
            if (barValue <= range / 2f)
            {
                // Make spoon play anim and update state
                spoonScript.GetPowder();

                // Play sound effect
                sound.Play();

                // Disable bar and trigger; the spoon has powder
                bar.SetActive(false);
                active = false;
            }
            // Missed it
            else
            {
                //shake bar
                StartCoroutine(ShakeBar());
            }
        }
    }

    private IEnumerator ShakeBar()
    {
        //timmer
        float timePassed = 0;

        Transform barTrans = bar.transform;
        Vector3 barBasePos = barTrans.localPosition;
        float time = Time.time;

        while (timePassed < 0.5f)
        {
            //use sin and time to shake on Y axis
            barTrans.localPosition = barBasePos + new Vector3(0, Mathf.Sin((Time.time - time) * 25.2f) * 15f, 0);

            timePassed += Time.deltaTime;
            yield return null;
        }
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