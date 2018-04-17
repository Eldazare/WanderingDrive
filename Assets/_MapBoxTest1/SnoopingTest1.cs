using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class SnoopingTest1 : MonoBehaviour {


	/*
	This script only exists to test access to lat-long information on markers.
	AND to trigger touching markers.
	
	*/


	public Text box;
	//public GameObject map;
	public MarkerScript2 mark;
	public Vector2d vec;



	// Use this for initialization
	void Start () {

		box.text = "";

		mark = GameObject.FindGameObjectWithTag ("marker").GetComponent<MarkerScript2> ();
		vec = mark._location;

		//InvokeRepeating ("MoveMarker", 0.18f, 0.18f);

	}


	void MoveMarker () {
		mark._location.x += 0.00001f;
	}

	void OnTriggerEnter (Collider col) {
		if (col.gameObject.CompareTag ("marker")) {
			Debug.Log ("STRANGER DANGER!");
			box.text = "STRANGER DANGER!";
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.gameObject.CompareTag ("marker")) {
			box.text = "";
		}
	}
		
}
