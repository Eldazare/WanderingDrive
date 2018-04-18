using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {




	public GameObject Player;

	// This just tracks the player. In the pokemon-fashion.
	void LateUpdate () {
		this.transform.position = new Vector3 (Player.transform.position.x, this.transform.position.y, Player.transform.position.z - 20.0f);
	}





}
