using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {




	public GameObject Player;
	public float posX = 0.0f;
	public float posY = 30.0f;
	public float posZ = -20.0f;

	// This just tracks the player. In the pokemon-fashion.
	void LateUpdate () {
		this.transform.position = new Vector3 (Player.transform.position.x + posX, posY, Player.transform.position.z + posZ);
	}





}
