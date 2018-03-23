using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Enemy {
	EnemyStats enemyStats;
	Animator animator;
	PlayerCombatScript player;

	AnimationClip idle;
	AnimationClip attack1;
	AnimationClip attack2;
	AnimationClip roar;
	AnimationClip defensive;
	int enemyID;
	Image healthBar;

	public Enemy Previous;
	public Enemy Next;


	IEnumerator Attack() {
		yield return new WaitForSeconds (2.0f);
	}
	void Start () {
		// Instantiate enemy stats and such


	}
	public string GetHit (){


		// animator.SetTrigger("Ouch");
		return "lel";
	}
}




[System.Serializable]
public class Wat {
	public int _ex1 = 5;
	public int _ex2 = 5;

	public Wat (int e1, int e2){
		_ex1 = e1;
		_ex2 = e2;
	}

	public void AlterEx1 (int x) {
		_ex1 += x;
	}
}
	

