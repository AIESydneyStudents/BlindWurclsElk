using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeaPouringScript : MonoBehaviour
{
    public Transform camPos;

    // Y point of the surface in world space
    public float surfHeight;
    // Height of object from surface while dragging
    public float dragHeight = 1f;

    bool locked = false;
    int count = 3;

    public Text helpText;


    public GameObject teaObj;
    public Transform teaFullPos;
    public Transform teaEmptyPos;

    public Vector3 pourOffset;

    public ParticleSystem pouringEffect;
    public AudioSource soundEffect;

    private TeaCupScript teacup;


    void Start()
    {
        //move cam
        StartCoroutine(MoveCam());

        helpText.text = "Pour tea into the cups";
    }

    void OnTriggerEnter(Collider other)
	{
        //hack to avoid other collisions without using new tag --- FIX IT
        if (!other.CompareTag("Coal"))
        { return; }


        locked = true;
        count--;
        teacup = other.GetComponent<TeaCupScript>();

        //set bowl offset to side of cup
        transform.position = other.transform.position + pourOffset;

        //play anim
        StartCoroutine(Anim());

        //disable the trigger
        other.enabled = false;
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
        if (locked)
        { return; }


        // Get the cursor pos relative to the object transform
        Vector3 pos = CursorOnTransform(transform.position);
        // Set hight to the surface + set drag height
        pos.y = surfHeight + dragHeight;

        // Clamp position to prevent it from going off screen
        pos.x = Mathf.Clamp(pos.x, -11.57f, -11.15f);
        pos.z = Mathf.Clamp(pos.z, -4f, -3f);

        // Set the object's position
        transform.position = pos;
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

    private IEnumerator Anim()
	{
        Quaternion start = transform.rotation;
        //increase angle as tea level goes down
        Quaternion end = start * Quaternion.Euler(13 * (3 - count), 0, 0);
        float time = 0;

        while (time < 2)
        {
            //play pour effect and make tea levels change halfway through
            if (time > 1 && !pouringEffect.isPlaying)
            {
                pouringEffect.Play();
                soundEffect.Play();
                StartCoroutine(LowerTea());

                //raise tea in cup
                teacup.RaiseTeaLevel();
            }

            //tilt down
            transform.rotation = Quaternion.Lerp(start, end, time * 0.5f);

            time += Time.deltaTime;
            yield return null;
        }


        yield return new WaitForSeconds(1f);

        time = 0;
        while (time < 2)
        {
            //stop pour effect halfway through
            if (time > 1f && pouringEffect.isPlaying)
            {
                pouringEffect.Stop();

                //hide the tea if empty
                if (count == 0)
                { teaObj.SetActive(false); }
            }

            //tilt up
            transform.rotation = Quaternion.Lerp(end, start, time * 0.5f);

            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        if (count != 0)
		{
            //unlock bowl
            locked = false;
        }
        else
		{
            //next minigame
        }
    }

    private IEnumerator LowerTea()
    {
        float time = 0;
        float scale = 1f / 3f;

        Vector3 startPos = teaObj.transform.localPosition;
        Vector3 startScale = teaObj.transform.localScale;

        //get tea position between full and empty using count
        Vector3 endPos = Vector3.Lerp(teaEmptyPos.localPosition, teaFullPos.localPosition, count * scale);
        Vector3 endScale = Vector3.Lerp(teaEmptyPos.localScale, teaFullPos.localScale, count * scale);


        while (time < 3)
        {
            //lerp pos and scale
            teaObj.transform.localPosition = Vector3.Lerp(startPos, endPos, time * scale);
            teaObj.transform.localScale = Vector3.Lerp(startScale, endScale, time * scale);

            time += Time.deltaTime;
            yield return null;
        }
    }
}