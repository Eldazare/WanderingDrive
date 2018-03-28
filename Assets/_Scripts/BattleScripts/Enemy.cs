using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Enemy : MonoBehaviour{
	EnemyStats enemyStats;
	Animator anim;
	Vector3 startPos;
	public CombatController combatController;

	AnimationClip idle;
	AnimationClip attack1;
	AnimationClip attack2;
	AnimationClip roar;
	AnimationClip defensive;

	string enemyName;
	public int enemyID;
	Image healthBar;
	bool proceed;


	void Start() {
		startPos = transform.position;
		enemyStats = EnemyStatCreator.LoadStatBlockData(0, "small");
		enemyName = "Enemy";
	}

	public IEnumerator Attack() {
		InvokeRepeating("moveToPlayer", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		proceed = false;
		combatController.HitPlayer(enemyStats.damage, enemyStats.elementDamage,enemyStats.element, false);
		InvokeRepeating("moveFromPlayer",0,Time.deltaTime);
	}

	void moveToPlayer(){
		if(Vector3.Distance(combatController.player.transform.position, transform.position)>1){
			transform.Translate((combatController.player.transform.position-transform.position)*Time.deltaTime*5);
		}else{
			//animator.SetTrigger("Attack");
			//animator calls animatorCallingEnemy(); and sets proceed to true and player takes damage
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
		int damageTaken = damage+elementDamage;
		//Damage reduction calculations
		enemyStats.health -= damageTaken;
		// animator.SetTrigger("Ouch");
		combatController.updateEnemyStats(enemyStats.health, enemyStats.maxHealth, enemyStats.health/enemyStats.maxHealth, this);
		return enemyName+" took "+damageTaken+" damage!";
	}

	public void animatorCallingEnemy(){
		proceed = true;
	}
}



	

