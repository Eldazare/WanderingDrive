using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour{
	public PlayerStats playerStats = new PlayerStats(); //Player stat container
	Vector3 startPos;	//Player's starting position to move to and from Enemy
	public GameObject model, weapon, weaponSlot;
	public Transform stomach;
	float blockTimer, blockDuration = 3, dodgeTimer, dodgeDuration = 2, timerAccuracy = 0.8f; //Defensive timers and the accuracy wanted
	Vector3 enemyPos; //Enemy position to move to and from it
	[HideInInspector]
	public bool proceed; //Used in moving to and from the targeted enemy
	public MenuController menuController;
	public CombatController combatController;
	bool focusedTurn, focusDefensiveBonus, skipTurn, overloadDamageTakenBonus, focusPlusOverloadTurn, focusPlustOverloadBonus; //Focus and overload logic booleans
	int attackRange = 2; //How close the player moves to the enemy
	int overloadedTurn;
	public Animator animator;
	void Start(){
		//Generate player model and player stats
		//playerStats = new PlayerStats();
	}
	
	public void Attack (int part) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine(AttackRoutine(menuController.targetedEnemy, part));
	}
	IEnumerator AttackRoutine(Enemy target, int part) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		animator.SetTrigger("Attack");
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		combatController.HitEnemy(playerStats.weapon.damage, playerStats.weapon.elementDamage, playerStats.weapon.element, part);
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		InvokeRepeating("moveFromEnemy",0,Time.deltaTime);
		proceed = false;
	}

	public void Ability (int ID, int part) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine(AbilityRoutine(playerStats.abilityDamage(ID), playerStats.abilityElementDamage(ID), playerStats.abilityElement(ID), part));
	}
	IEnumerator AbilityRoutine(int abilityDamage, int abilityElementDamage, Element abilityElement, int part) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		animator.SetTrigger("Attack");
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		combatController.HitEnemy(playerStats.weapon.damage, playerStats.weapon.elementDamage, playerStats.weapon.element, part);
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		InvokeRepeating("moveFromEnemy",0,Time.deltaTime);
		proceed = false;
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
		overloadedTurn = 3;
		combatController.enemyTurns = 2;
		menuController.focusEnabled = false;
		menuController.overloadEnabled = false;
		if(focusedTurn){
			focusPlusOverloadTurn = true;
			focusPlustOverloadBonus = true;
			EndPlayerTurn(true);
		}else{
			EndPlayerTurn(true);
		}
	}

	void EndPlayerTurn(bool setBool){
		if(focusPlusOverloadTurn){
			menuController.focusEnabled = false;
			menuController.overloadEnabled = false;
			focusPlusOverloadTurn = setBool;
			menuController.PlayersTurn();
		}else if(focusedTurn){
			focusedTurn = false;
			menuController.overloadEnabled = false;
			menuController.PlayersTurn();
		}else if(overloadedTurn > 0){
			overloadedTurn--;
			if(overloadedTurn == 0){
				menuController.focusEnabled = true;
				menuController.overloadEnabled = true;
				combatController.enemyAttacks();
			}else{
				menuController.PlayersTurn();
			}
		}else{
			focusedTurn = setBool;
			menuController.focusEnabled = !setBool;
			menuController.overloadEnabled = true;
			focusPlustOverloadBonus = false;
			combatController.enemyAttacks();
		}
	}

	public string GetHit(int damage, int elementDamage, Element element, bool area){
		if(element == 0){
			elementDamage = 0;
		}
		string returnedValue;	//Returning value to report in TextBox
		float damageTaken = damage+elementDamage;
		//Include modifiers to calculations: 
		//overloadDamageTakenBonus
		//focusDefensiveBonus

		if(dodgeTimer>timerAccuracy*dodgeTimer){
			if(area){
				playerStats.health -= damage*playerStats.damageReduction;

				returnedValue = "You dodged took "+damage*playerStats.damageReduction+" areadamage";
			}
			else{
				returnedValue = "You dodged the attack!";
			}
		}
		else if(blockTimer>timerAccuracy*blockTimer){
			//Block calculations
			damageTaken = damageTaken*0.5f;
			returnedValue = "You blocked the attack but took "+ damage +" damage!";
		}
		else{
			//Damage taken calculations
			playerStats.health -= damageTaken;
			returnedValue = "You took " + damageTaken + " damage!";
		}
		combatController.ResetPlayerDefence();
		updateStats();

		return returnedValue;
	}

	public void updateStats(){
		menuController.updatePlayerHealth(playerStats.health, playerStats.maxHealth, playerStats.health/playerStats.maxHealth);
	}

	void moveToEnemy(){
		if(Vector3.Distance(enemyPos, transform.position)>attackRange){
			transform.Translate(((enemyPos-transform.position)+(enemyPos-transform.position).normalized)*Time.deltaTime*5);
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
			combatController.cameraScript.ResetCamera();
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
		if(dodgeTimer<=0){
			//Dodge animation depending on direction 1 = right and down, 0 = left and up
			dodgeTimer += dodgeDuration;
			InvokeRepeating("DodgeCountDown",0, timerAccuracy);
		}
	}

	public void Block(){
		if(blockTimer<=0){
			//Block 
			blockTimer += blockDuration;
			InvokeRepeating("BlockCountDown", 0, timerAccuracy);
		}
	}
}
