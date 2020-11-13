using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialNode : BaseNode
{

	public GameObject obj;

	public Material mat;



	public override void Activate()
	{
		obj.GetComponent<Renderer>().material = mat;
	}
}