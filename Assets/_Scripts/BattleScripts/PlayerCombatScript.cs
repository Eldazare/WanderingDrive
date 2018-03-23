using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour{
	public PlayerStats playerStats;
	GameObject playerHost;
	Vector3 startPos;
	public GameObject model, weapon;
	float blockTimer, blockDuration, dodgeTimer, dodgeDuration, timerAccuracy;
	Vector3 enemyPos;
	bool proceed;
	public MenuController menuController;
	public CombatController combatController;
	
	void Start(){
		//Generate player model
	}
	public void Attack () {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine(AttackRoutine(menuController.targetedEnemy));
	}

	public void Ability (int ID) {
		// animation.SetTrigger("lel");
		// PlayerStats -> ID
		// deal all the damages
	}
	void moveToEnemy(){
		if(Vector3.Distance(enemyPos, transform.position)>1){
			transform.Translate((enemyPos-transform.position)*Time.deltaTime*5);
		}else{
			proceed = true;
			CancelInvoke("moveToEnemy");
		}
	}
	void moveFromEnemy(){
		if(Vector3.Distance(startPos, transform.position)>0.1){
			transform.Translate((startPos-transform.position)*Time.deltaTime*5);
		}else{
			CancelInvoke("moveFromEnemy");
			combatController.enemyAttacks();
		}
	}
	IEnumerator AttackRoutine(Enemy target) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		//anim.SetTrigger("Attack");
		yield return new WaitUntil(() =>proceed);
		target.GetHit (playerStats.damage, playerStats.elementalDamage, playerStats.element);
		proceed = false;
		InvokeRepeating("moveFromEnemy",0,Time.deltaTime);
	}

	public void Consumable(int slot){

	}

	public void PlayerFocus () {


	}
	public void PlayerOverload () {
		

	}
	public string GetHit(int damage, int elementDamage, int element, bool area){
		if(dodgeTimer>0){
			if(area){
				playerStats.health -= damage*playerStats.damageReduction;

				return "took"+damage*playerStats.damageReduction+"damage";
			}
			else{
				return "dodged the attack!";
			}
		}
		else if(blockTimer>0){
			
			//Block calculations
			return "you blocked the attack but took "+ damage +"damage!";
		}
		else{
			//Damage taken calculations
			playerStats.health -= damage*playerStats.damageReduction;
			return "took"+damage*playerStats.damageReduction+"damage";
		}
	}

	void BlockCountDown(){
		blockTimer -= timerAccuracy;
		if(blockTimer <= 0){
			blockTimer = 0;
			CancelInvoke("BlockCountDown");
		}
	}

	void DodgeCountDown(){
		dodgeTimer -= timerAccuracy;
		if(dodgeTimer <= 0){
			dodgeTimer = 0;
			CancelInvoke("DodgeCountDown");
		}
	}

	public void Dodge(int direction){
		if(dodgeTimer<=0 && !IsInvoking("DodgeCountDown")){
			//Dodge
			dodgeTimer += dodgeDuration;
			InvokeRepeating("DodgeCountDown",0, timerAccuracy);
		}
	}

	public void Block(){
		if(blockTimer<=0 && !IsInvoking("BlockCountDown")){
			//Block
			blockTimer += blockDuration;
			InvokeRepeating("BlockCountDown", 0, timerAccuracy);
		}
	}
}
