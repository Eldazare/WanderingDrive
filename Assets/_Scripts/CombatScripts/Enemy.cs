using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Enemy : MonoBehaviour{
	EnemyStats enemyStats;
	Animator anim;
	Vector3 startPos;
	public PlayerCombatScript player;
	public CombatController combatController;

	AnimationClip idle;
	AnimationClip attack1;
	AnimationClip attack2;
	AnimationClip roar;
	AnimationClip defensive;
	int enemyID;
	Image healthBar;
	bool proceed;


	void Start() {
		startPos = transform.position;
		enemyStats = new EnemyStats(enemyID);
	}
	public IEnumerator Attack() {
		InvokeRepeating("moveToPlayer", 0, Time.deltaTime);
		//anim.SetTrigger("Attack");
		yield return new WaitUntil(() =>proceed);
		proceed = false;
		player.GetHit(enemyStats.damage,enemyStats.elementDamage, enemyStats.element, false);
		InvokeRepeating("moveFromPlayer",0,Time.deltaTime);
	}

	void moveToPlayer(){
		Debug.Log(Vector3.Distance(player.transform.position, transform.position));
		if(Vector3.Distance(player.transform.position, transform.position)>1){
			transform.Translate((player.transform.position-transform.position)*Time.deltaTime*5);
		}else{
			proceed = true;
			CancelInvoke("moveToPlayer");
		}
	}
	void moveFromPlayer(){
		if(Vector3.Distance(startPos, transform.position)>0.1){
			transform.Translate((startPos-transform.position)*Time.deltaTime*5);
		}else{
			CancelInvoke("moveFromPlayer");
			combatController.enemyAttacked = true;
		}
	}
	public string GetHit (int damage, int elementDamage, int element){

		float damageTaken = damage+elementDamage;
		//Damage reduction calculations
		enemyStats.health -= damageTaken;
		// animator.SetTrigger("Ouch");
		return "took x amount of damage and is angry!";
	}
}



	

