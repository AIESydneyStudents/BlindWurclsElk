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
	public bool useSitting = false;



	public override void Activate()
	{
		TransitionManager.instance.ChangeScene(sceneName, (changePos) ? (Vector3?)pos : null, useSitting);
	}
}