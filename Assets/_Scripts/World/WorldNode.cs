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
		NodeInteraction.GiveInteractedNodeForward (this.gameObject);
		NodeInteraction.HandleNodeInteraction (nodeType, id);
	}

	void OnMouseDown(){
		Debug.Log ("Hit:" +this.nodeType+ " / " + this.id);
		Interact ();
	}

	void OnTriggerStay(Collider col) {
		if (col.gameObject.CompareTag("marker")) {
			Vector3 otherVec = col.gameObject.transform.position;
			Vector3 thisVec = this.gameObject.transform.position;
			Vector3 diff = new Vector3 (thisVec.x - otherVec.x, 0, thisVec.z - otherVec.z);

			float scaling = 0.5f;
			diff = -diff.normalized;

			col.GetComponent<Rigidbody> ().AddForce (diff*scaling);
		}
	}
}
