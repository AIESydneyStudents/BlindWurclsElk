using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

/// <summary>
/// Abstract base node class used for all DialogueGraph nodes
/// </summary>
public abstract class BaseNode : Node
{
	// The values of the input and output ports is irelevent, they are only
	// used to connect nodes together

	[Input(ShowBackingValue.Never, ConnectionType.Override)]
	public byte input;
	[Output(ShowBackingValue.Never, ConnectionType.Multiple)]
	public byte output;


	/// <summary>
	/// Used for initilisation
	/// </summary>
	protected override void Init()
	{
		base.Init();
	}

	// Ports are not read for values, so this is unused
	public override object GetValue(NodePort port)
	{
		return base.GetValue(port);
	}


	/// <summary>
	/// Called when the node has been reached in the dialogue graph
	/// </summary>
	public abstract void Activate();
}