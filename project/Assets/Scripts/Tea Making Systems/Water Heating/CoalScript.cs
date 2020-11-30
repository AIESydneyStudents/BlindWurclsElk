using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalScript : MonoBehaviour
{
    Material mat;

	// Used by CoalDragScript to snap into the burner
	[HideInInspector]
	public bool inBurner = false;

	bool active = false;
	Color lastCol;
	Color newCol;
	float time;


	void Start()
	{
		// Get the material and set emission
		mat = GetComponent<Renderer>().material;
		mat.EnableKeyword("_EMISSION");
	}

	public void SetGlow(bool makeGlow)
	{
		inBurner = makeGlow;

		// Setup variables
		active = true;
		time = 0;
		lastCol = mat.GetColor("_EmissionColor");

		// Set new color to light red or nothing
		newCol = new Color((makeGlow) ? 0.25f : 0, 0, 0);
	}

	void Update()
	{
		if (!active)
		{ return; }


		if (time < 1f)
		{
			// Lerp emission color
			Color col = Color.Lerp(lastCol, newCol, time);
			mat.SetColor("_EmissionColor", col);

			time += Time.deltaTime;
		}
		else
		{
			// If finished, stop changing
			active = false;
		}
	}
}