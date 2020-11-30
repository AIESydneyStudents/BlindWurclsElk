using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
	private static CoroutineRunner instance;


	public static void RunCoroutine(IEnumerator coroutine)
	{
		if (instance == null)
		{
			// Create a new GameObject and give it an instance of this script
			GameObject obj = new GameObject("CoroutineRunner");
			instance = obj.AddComponent<CoroutineRunner>();
		}

		// Run the coroutine
		instance.StartCoroutine(coroutine);
	}
}