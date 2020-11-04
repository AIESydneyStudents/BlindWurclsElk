﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogueGraph : NodeGraph
{
	DialogueManager manager;

	// The dialogue node the graph is currently on
	public DialogueNode current;


	/// <summary>
	/// Start running the graph. A DialogueManager script must exist
	/// </summary>
	public void Run()
	{
		// Get manager instance
		manager = DialogueManager.instance;

		// Start with the dialogue node with no inputs (the root)
		current = nodes.Find(node => node is DialogueNode && node.Inputs.All(port => !port.IsConnected)) as DialogueNode;

		// Display dialogue, then activate the node
		manager.ChangeDialogue(current.text, current.audio);
		current.Activate();
	}

	/// <summary>
	/// Activate all nodes connected to current, and set current to the next dialogue node
	/// </summary>
	public void NextNode()
	{
		// Get the current output port, and reset current to be assigned to
		NodePort output = current.GetPort("output");
		current = null;


		// Get each connected port (the output port can be connected to multiple input ports)
		foreach (var port in output.GetConnections())
		{
			// Get the connected node and activate it
			BaseNode node = port.node as BaseNode;
			node.Activate();

			// If there is a connected dialogue node, it becomes current
			if (node is DialogueNode)
			{ current = node as DialogueNode; }
		}


		// Update dialogue, or end dialogue if there is no new dialogue node
		if (current != null)
		{ manager.ChangeDialogue(current.text, current.audio); }
		else
		{ manager.EndDialogue(); }
	}
}