using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class TeaPotScript : MonoBehaviour
{
    // Y point of the surface in world space
    public float surfHeight;
    // Height of object from surface while dragging
    public float dragHeight = 1f;

    bool locked = false;
    bool selected = false;

    public Text helpText;

    public ParticleSystem waterParticleEffect;
    public AudioSource sound;

    public Transform teaObjFull;

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

	void Start()
	{
        helpText.text = "Grab the tea pot";
	}

	void Update()
    {
        if (locked)
        { return; }

        //move
        if (selected)
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
        //check for selection
        else
		{
            // Find clicked object
            if (Input.GetMouseButtonDown(0))
            {
                // Cast ray from the cursor on the screen
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100))
                {
                    // If this object has been clicked
                    if (hit.transform == transform)
                    {
                        selected = true;

                        helpText.text = "Pour water into the bowl";
                    }
                }
            }
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

        while (time < 2)
		{
            //play water effect and make tea rise halfway through
            if (time > 1 && !waterParticleEffect.isPlaying)
			{
                waterParticleEffect.Play();
                sound.Play();
                StartCoroutine(RaiseWater());
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
            //stop water effect halfway through
            if (time > 1f && waterParticleEffect.isPlaying)
            {
                waterParticleEffect.Stop();
            }

            //tilt up
            transform.rotation = Quaternion.Lerp(end, start, time * 0.5f);

            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //next minigame
        waterPouringObjects.SetActive(false);
        gameObject.SetActive(false);
        teaStirringObjects.SetActive(true);
    }

    private IEnumerator RaiseWater()
	{
        //show tea
        teaObj.SetActive(true);

        float time = 0;
        float scale = 1f / 3f;

        Vector3 startPos = teaObj.transform.position;
        Vector3 startScale = teaObj.transform.localScale;
        //teaObjFull for end

        while (time < 3)
		{
            //lerp pos and scale
            teaObj.transform.position = Vector3.Lerp(startPos, teaObjFull.position, time * scale);
            teaObj.transform.localScale = Vector3.Lerp(startScale, teaObjFull.localScale, time * scale);


            time += Time.deltaTime;
            yield return null;
		}

        //hide powder
        teaPowderObj.SetActive(false);
    }
}