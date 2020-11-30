using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SceneTransitionNode : BaseNode
{
	public string sceneName;
	[Space]
	public bool changePos = false;
	public Vector3 pos;
	[Space]
	public bool changeRot = false;
	public Vector3 rot;
	[Space]
	public bool useSitting = false;

	//should an animated cup be used for the transition
	public bool useCup = false;
	public GameObject cup;



	public override void Activate()
	{
		if (useCup)
		{
			//create cup as child of cam, and get its animator component
			Animator anim = Instantiate(cup, Camera.main.transform).GetComponent<Animator>();
			//set the transition manager's animator
			TransitionManager.instance.anim = anim;
		}
		
		//transition, passing optional params
		TransitionManager.instance.ChangeScene(sceneName, (changePos) ? (Vector3?)pos : null, (changeRot) ? (Vector3?)rot : null, useSitting);
	}
}