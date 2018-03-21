using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

	List<Enemy> enemyList;
	PlayerCombatScript player;
	string textBoxMessage;
	string getHitReturn;

	//public Text TextBox;

	void enemyAttacks(){
		for (int i = 0; i < enemyList.Count; i++) {
			Debug.Log ("Enemy number " + i + " pretends to attack.");
		}
	}
	// Use this for initialization
	void Start () {
		// Generate enemies
	}

	void textBoxUpdate(){
		// Do the thing
	}


	// public section
	public void Attack (Enemy target) {
		target.GetHit ();
		enemyAttacks ();
	}
	public void Ability (Enemy target, int ID) {
		// animation.SetTrigger("lel");
		// PlayerStats -> ID
		// deal all the damages
		enemyAttacks ();
	}
	public void PlayerFocus () {



		enemyAttacks ();
	}
	public void PlayerOverload () {



		enemyAttacks ();
	}




}
