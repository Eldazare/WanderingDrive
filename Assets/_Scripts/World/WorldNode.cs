using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode : MonoBehaviour {

	// TODO: How to link node visuals?
	// TODO: How to interact? Button.onClick()?
	// Maybe make List<List<Sprite>> and index it according to types and id:s.
	// 	Add these sprites to the ListList object by loading all in specific folders.

	public int nodeType;
	public int id;
	public int time; // in seconds

	public double latitude;
	public double longitude;

	public static Color[] colors = {Color.green, Color.red, Color.magenta, Color.black};

	public void GetNodeColor(){
		gameObject.GetComponent<MeshRenderer> ().material.color = colors [nodeType];
	}

	public void Interact(){
		NodeInteraction.HandleNodeInteraction (nodeType, id);
	}

	void OnMouseDown(){
		Debug.Log ("Hit: " + this.gameObject);
		Interact ();
	}
}
