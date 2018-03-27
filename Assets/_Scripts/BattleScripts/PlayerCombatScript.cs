using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour{
	public PlayerStats playerStats; //Player stat container
	Vector3 startPos;	//Player's starting position to mvoe to and from Enemy
	public GameObject model, weapon;
	float blockTimer, blockDuration, dodgeTimer, dodgeDuration, timerAccuracy; //Defensive timers and the accuracy wanted
	Vector3 enemyPos; //Enemy position to move to and from it
	bool proceed; //Used in moving to and from the targeted enemy
	public MenuController menuController;
	public CombatController combatController;
	public bool defended, focusedTurn, overloadedTurn, focusDefensiveBonus, skipTurn, overloadDamageTakenBonus, focusPlusOverloadTurn, focusPlustOverloadBonus; //Focus and overload logic booleans
	int attackRange = 1; //How close the player moves to the enemy
	void Start(){
		//Generate player model and player stats
		playerStats = new PlayerStats();
	}
	
	public void Attack () {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine(AttackRoutine(menuController.targetedEnemy));
	}
	IEnumerator AttackRoutine(Enemy target) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		//animator.SetTrigger("Attack");
		yield return new WaitUntil(() =>proceed);
		combatController.HitEnemy(playerStats.damage, playerStats.elementalDamage, playerStats.element);
		proceed = false;
		InvokeRepeating("moveFromEnemy",0,Time.deltaTime);
	}

	public void Ability (int ID) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine(AbilityRoutine(playerStats.abilityDamage(ID), playerStats.abilityElementDamage(ID), playerStats.abilityElement(ID)));
	}
	IEnumerator AbilityRoutine(int abilityDamage, int abilityElementDamage, int abilityElement) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		//anim.SetTrigger("AbilityX");
		yield return new WaitUntil(() =>proceed);
		combatController.HitEnemy(abilityDamage, abilityElementDamage, abilityElement);
		proceed = false;
		InvokeRepeating("moveFromEnemy",0,Time.deltaTime);
	}
	
	public void Consumable(int slot) {
		EndPlayerTurn(false);
		focusDefensiveBonus = false;
	}

	public void PlayerFocus () {
		focusDefensiveBonus = true;
		menuController.focusEnabled = false;
		EndPlayerTurn(true);
	}
	public void PlayerOverload () {
		overloadedTurn = true;
		if(focusedTurn){
			focusPlusOverloadTurn = true;
			focusPlustOverloadBonus = true;
			EndPlayerTurn(true);
		}else{
			EndPlayerTurn(false);
		}
	}
	void EndPlayerTurn(bool focus){
		if(focusPlusOverloadTurn){
			menuController.focusEnabled = false;
			menuController.overloadEnabled = false;
			focusPlusOverloadTurn = focus;
			menuController.PlayersTurn();
		}else if(focusedTurn){
			menuController.PlayersTurn();
			focusedTurn = false;
		}else if(overloadedTurn){
			menuController.PlayersTurn();
			overloadedTurn = false;
		}else{
			focusedTurn = focus;
			focusPlustOverloadBonus = false;
			combatController.enemyAttacks();
		}
	}

	public string GetHit(int damage, int elementDamage, int element, bool area){
		string returnedValue;	//Returning value to report in TextBox
		float damageTaken = damage+elementDamage;
		//Include modifiers to calculations: 
		//overloadDamageTakenBonus
		//focusDefensiveBonus

		if(dodgeTimer>timerAccuracy){
			if(area){
				playerStats.health -= damage*playerStats.damageReduction;

				returnedValue = "You dodged took "+damage*playerStats.damageReduction+" areadamage";
			}
			else{
				returnedValue = "You dodged the attack!";
			}
		}
		else if(blockTimer>timerAccuracy){
			//Block calculations
			returnedValue = "You blocked the attack but took "+ damage +" damage!";
		}
		else{
			//Damage taken calculations
			playerStats.health -= damageTaken;
			returnedValue = "You took " + damageTaken + " damage!";
		}
		combatController.ResetPlayerDefence();
		menuController.updatePlayerHealth(playerStats.health, playerStats.maxHealth, playerStats.health/playerStats.maxHealth);
		return returnedValue;
	}

	void moveToEnemy(){
		if(Vector3.Distance(enemyPos, transform.position)>attackRange){
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
			EndPlayerTurn(false);
			focusDefensiveBonus = false;
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
		if(dodgeTimer<=0 && !defended){
			//Dodge animation depending on direction 1 = right and down, 0 = left and up
			dodgeTimer += dodgeDuration;
			InvokeRepeating("DodgeCountDown",0, timerAccuracy);
		}
	}

	public void Block(){
		if(blockTimer<=0 && !defended){
			//Block 
			blockTimer += blockDuration;
			InvokeRepeating("BlockCountDown", 0, timerAccuracy);
		}
	}
}
