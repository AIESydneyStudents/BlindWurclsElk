using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode
{
	[TextArea]
	public string text;
	public AudioClip audio;

	[Tooltip("The durration of the audio clip is added")]
	public float extraPlayTime = 1;

	private float duration;


	protected override void Init()
	{
		base.Init();

		duration = extraPlayTime;
		// Add the length of the audio clip to the node durration
		if (audio != null)
		{
			duration += audio.length;
		}
	}


	public override void Activate()
	{
		// Only monobehaviours can run coroutines, so a helper script is used
		CoroutineRunner.RunCoroutine(Wait());
	}

	private IEnumerator Wait()
	{
		// Wait for the durration to be over before returning to the graph
		yield return new WaitForSeconds(duration);

		if ((graph as DialogueGraph).current == this)
		{
			(graph as DialogueGraph).NextNode();
		}
	}
}