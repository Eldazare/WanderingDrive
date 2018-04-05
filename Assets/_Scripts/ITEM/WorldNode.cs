using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode : MonoBehaviour {

	// TODO: How to link node visuals?
	// Maybe make List<List<Sprite>> and index it according to types and id:s.

	public int nodeType;
	public int id;

	public float latitude;
	public float longitude;


	public void Interact(){
		NodeInteraction.HandleNodeInteraction (nodeType, id);
	}
}
