using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class TeaPotScript : MonoBehaviour
{
    // Y point of the surface in world space
    public float surfHeight;
    // Height of object from surface while dragging
    public float dragHeight = 1f;

    bool locked = false;


    public ParticleSystem waterParticleEffect;

    public GameObject teaPowderObj;
    public GameObject teaObj;

    public GameObject waterPouringObjects;
    public GameObject teaStirringObjects;



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
        if (!locked)
        {
            // Get the cursor pos relative to the object transform
            Vector3 pos = CursorOnTransform(transform.position);
            // Set hight to the surface + set drag height
            pos.y = surfHeight + dragHeight;

            pos.z -= 0.25f;

            // Clamp position to prevent it from going off screen
            pos.x = Mathf.Clamp(pos.x, -11.57f, -11.15f);
            pos.z = Mathf.Clamp(pos.z, -4.25f, -3.48f);

            // Set the object's position
            transform.position = pos + new Vector3(0, 0, 0.04f);
        }
    }
    
	void OnTriggerEnter(Collider other)
	{
        // Lock the tea pot and move it to a good position
        locked = true;
        transform.localPosition = new Vector3(-0.22f, 0.09f, -0.22f);

        // Play animation
        StartCoroutine(Anim());
	}

	private IEnumerator Anim()
	{
        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0, 0, 15);
        float time = 0;

        while (time < 1)
		{
            //play water effect halfway through
            if (time > 0.5f && !waterParticleEffect.isPlaying)
			{
                waterParticleEffect.Play();
            }

            //tilt down
            transform.rotation = Quaternion.Lerp(start, end, time);

            time += Time.deltaTime;
            yield return null;
		}

        //swap powder for tea
        teaPowderObj.SetActive(false);
        teaObj.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        time = 0;
        while (time < 1)
        {
            //stop water effect halfway through
            if (time > 0.5f && waterParticleEffect.isPlaying)
            {
                waterParticleEffect.Stop();
            }

            //tilt up
            transform.rotation = Quaternion.Lerp(end, start, time);

            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //move teapot

        //next minigame
    }
}