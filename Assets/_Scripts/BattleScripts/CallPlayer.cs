using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallPlayer : MonoBehaviour {


	public PlayerCombatScript player;
	// Use this for initialization

	public void Proceed(){
		player.proceed = true;
	}
}
