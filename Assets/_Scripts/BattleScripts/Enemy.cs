using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{
	EnemyStats enemyStats;
	public Animator animator;
	Vector3 startPos;
	[HideInInspector]
	public CombatController combatController;
	
	string enemyName;
	public int enemyID;
	Image healthBar;
	public bool proceed;


	void Start() {
		enemyStats = EnemyStatCreator.LoadStatBlockData(0, "small");
		enemyName = "Enemy";
		updateStats();
	}

	public IEnumerator Attack() {
		startPos = transform.position;
		InvokeRepeating("moveToPlayer", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		animator.SetTrigger("Attack");
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		combatController.HitPlayer(enemyStats.damage, enemyStats.elementDamage,enemyStats.element, false);
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		InvokeRepeating("moveFromPlayer",0,Time.deltaTime);
		proceed = false;
	}

	void moveToPlayer(){
		if(Vector3.Distance(combatController.player.transform.position, transform.position)>4){
			//transform.Translate((combatController.player.transform.position-transform.position)*Time.deltaTime*enemyStats.quickness);
			transform.position = Vector3.MoveTowards(transform.position, combatController.player.transform.position, Time.deltaTime*enemyStats.quickness+2); //Quickness as movement speed
		}else{
			proceed = true;
			CancelInvoke("moveToPlayer");
		}
	}
	void moveFromPlayer(){
		if(Vector3.Distance(startPos, transform.position)>0.1){
			//transform.Translate((startPos-transform.position)*Time.deltaTime*enemyStats.quickness);
			transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime*enemyStats.quickness+2); //Quickness as movement speed
		}else{
			CancelInvoke("moveFromPlayer");
			combatController.enemyAttacked = true;
		}
	}
	public string GetHit (int damage, int elementDamage, Element element){
		if(element == 0){
			elementDamage = 0;
		}
		float damageTaken = damage+elementDamage;
		//Damage reduction calculations
		enemyStats.health -= damageTaken;
		// animator.SetTrigger("Ouch");
		updateStats();
		return enemyName+" took "+damageTaken+" damage!";
	}

	void updateStats(){
		combatController.updateEnemyStats(enemyStats.health, enemyStats.maxHealth, enemyStats.health/enemyStats.maxHealth, this);
	}
}
