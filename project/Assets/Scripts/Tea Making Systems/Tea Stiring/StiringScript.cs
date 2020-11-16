using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StiringScript : MonoBehaviour
{
    public GameObject barObj;
    public GameObject barZone;
    public RectTransform bar;

    public GameObject winText;
    public AudioSource sound;

    // Distance to rotate around
    public float radius;
    // Point to rotate around
    public Vector3 centerPoint;

    // Used for smoothing
    float rotSpeed = 0f;
    float vel = 0f;

    public float targetSpeed;
    //how far out can it be and still count?
    public float targetRange;

    float winTimmer = 0;
    public float timeToWin;


    public Image colorChanger;
    public Color start = Color.red;
    public Color end = Color.green;


    void Start()
    {
        //show bar
        barObj.SetActive(true);
        barZone.SetActive(true);

        sound.Play();


        // Get center point in world space
        centerPoint = transform.TransformPoint(centerPoint);
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
        // If the roatation speed is within the target
		if (rotSpeed >= targetSpeed - targetRange && rotSpeed <= targetSpeed + targetRange)
		{
            winTimmer += Time.deltaTime;

            if (winTimmer >= timeToWin)
			{
                //Debug.Log("win");
                winText.SetActive(true);
			}
		}
        else if (winTimmer > 0f)
		{
            // Time goes down at slower rate
            winTimmer -= Time.deltaTime * 0.5f;
		}

        // Lerp between colors depending on progress
        colorChanger.color = Color.Lerp(start, end, winTimmer / timeToWin);

        sound.pitch = rotSpeed * 2f;
	}

	void FixedUpdate()
    {
        // Get cursor position relative to this object
        Vector3 pos = CursorOnTransform(transform.position);

        // Make it relative to the center point and remove the height component
        pos -= centerPoint;
        pos.y = 0f;

        // Put the cursor position on a rail
        pos.Normalize();
        pos *= radius;
        pos += centerPoint;


        // Get the angle traveled around the center
        float ang = Vector3.Angle(pos - centerPoint, transform.position - centerPoint);

        // Get speed, and smooth it
        ang *= Time.fixedDeltaTime;
        rotSpeed = Mathf.SmoothDamp(rotSpeed, ang, ref vel, 0.2f);


        // Update the transform
        transform.position = pos;
        // Update the bar, clamping the value
        bar.localScale = new Vector3(Mathf.Clamp(rotSpeed, 0, 1), 1, 1);
    }
}