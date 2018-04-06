using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : MonoBehaviour{
	public PlayerStats playerStats = new PlayerStats(); //Player stat container
	Vector3 startPos;	//Player's starting position to move to and from Enemy
	public GameObject model, weapon, weaponSlot;
	public Transform stomach;
																		
	
	Vector3 enemyPos; //Enemy position to move to and from it
	[HideInInspector]
	public bool proceed; //Used in moving to and from the targeted enemy
	public MenuController menuController;
	public CombatController combatController;

												//Perfect values are: if timer > maxDuration - perfect| Tiers are: if timer > maxDuration * tier
	float blockTimer, dodgeTimer, blockDuration = 2f, dodgeDuration = 1f, perfectDodge = 0.5f, perfectBlock = 0.15f; //Defensive timers and the accuracy wanted
	float[] blockTiers = {0.90f,0.75f, 0.5f, 0.25f};
	bool focusedTurn, focusDefensiveBonus, skipTurn, overloadDamageTakenBonus, focusPlusOverloadTurn, focusPlustOverloadBonus; //Focus and overload logic booleans
	float focusDamageBuff=1.5f, focusplustoverloadDamageBuff = 2f, overloadDamageBuff = 1.5f, overloadDebuff = 1f;
	int overloadedTurn, focusBuffTurns;
	int attackRange = 2; //How close the player moves to the enemy
	bool defended, stunned, paralyzed;
	

	public Animator animator;
	
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
		float damageMod = 0;

		if(focusPlustOverloadBonus){
			damageMod += focusplustoverloadDamageBuff;
		}
		else{
			if(focusBuffTurns>0){
				damageMod += focusBuffTurns;
			}
			if(overloadedTurn>0){
				damageMod += overloadDamageBuff;
			}
		}

		float playerDamage = playerStats.mainHand.damage;
		float playerElementDamage = playerStats.mainHand.damage;
		if(playerStats.offHand != null){
			playerDamage+=playerStats.offHand.damage;
			playerElementDamage+=playerStats.offHand.elementDamage;
		}

		combatController.HitEnemy(playerDamage,playerElementDamage, playerStats.element, part, damageMod);

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

	IEnumerator AbilityRoutine(int abilityDamage,float abilityElementDamage, Element abilityElement, int part) {
		InvokeRepeating("moveToEnemy", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		proceed = false;
		animator.SetTrigger("Attack");
		yield return new WaitUntil(() =>proceed);
		float damageMod = 0;
		if(focusPlustOverloadBonus){
			damageMod += focusplustoverloadDamageBuff;
		}
		else{
			if(focusBuffTurns>0){
				damageMod += focusDamageBuff;
			}
			if(overloadedTurn>0){
				damageMod += overloadDamageBuff;
			}
		}
		combatController.HitEnemy(abilityDamage,abilityElementDamage, abilityElement, part, damageMod);
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
		focusBuffTurns = 3;
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
		if(focusBuffTurns > 0){
			focusBuffTurns--;
		}
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

	public string GetHit(float damage,float elementDamage, Element element, bool area){
		string returnedValue = "";	//Returning value to report in TextBox
		//Include modifiers to calculations: 
		//overloadDamageTakenBonus
		//focusDefensiveBonus
		CancelInvoke("BlockCountDown");
		CancelInvoke("DodgeCountDown");
		if(dodgeTimer>(dodgeDuration-perfectDodge)){
			if(area){
				returnedValue = "You dodged but took "+takeDamage(damage, elementDamage, element)+" area damage!";
			}
			else{
				//Dodged attack
				returnedValue = "You dodged the attack!";
				takeDamage(0,0,0);
			}
		}
		else if(blockTimer>0){
			Debug.Log("BlockTimer: "+blockTimer);
			if(blockTimer>blockDuration-perfectBlock){
				returnedValue = "You blocked the attack and took no damage!";
				takeDamage(0,0,0);
			}else{
				bool blocked = false;
				foreach (var blockModifier in blockTiers){
					Debug.Log("Blockmod*blockdura "+(blockModifier*blockDuration));
					if(blockTimer >= (blockModifier*blockDuration)){
						returnedValue = "You blocked the attack but took "+takeDamage(damage,elementDamage, element, blockModifier)+"!";
						blocked = true;
						break;
						
					}
				}
				if(!blocked){
					returnedValue = "Your block failed and you took "+takeDamage(damage,elementDamage, element)+" damage!";
				}
			}
		}
		else{
			//Damage taken calculations
			returnedValue = "You took " + takeDamage(damage,elementDamage, element) + " damage!";
		}
		combatController.ResetPlayerDefence();
		blockTimer = 0;
		dodgeTimer = 0;
		defended = false;
		if(playerStats.health <=0){
			combatController.LoseEncounter();
			returnedValue += "\nYou died!";
			
			
		}
		
		return returnedValue;
	}

	float takeDamage(float damage, float elementDamage, Element element){
		float damageTaken, damageModifier = 1, eleModifier = 1;
		if(overloadedTurn>0){
			damageModifier += overloadDebuff;
			eleModifier += overloadDebuff;
		}

		damageModifier -= playerStats.damageReduction;
		eleModifier -= ((float)playerStats.elementalWeakness[System.Convert.ToInt32(element)])/100;

		damage = damage*damageModifier;
		elementDamage = elementDamage*eleModifier;
		damageTaken = damage+elementDamage;
		playerStats.health -= damageTaken;
		GameObject popup = Instantiate(Resources.Load("CombatResources/DamagePopUp"),new Vector3(transform.position.x, transform.position.y+3, transform.position.z)-transform.right, Quaternion.identity) as GameObject;
		popup.GetComponent<TextMesh>().text = damageTaken.ToString("0");
		updateStats();
		return damageTaken;
	}

	float takeDamage(float damage, float elementDamage, Element element, float blockModifier){
		float damageTaken, damageModifier = 1, eleModifier = 1;
		if(overloadedTurn>0){
			damageModifier += overloadDebuff;
			eleModifier +=overloadDebuff;
		}
		damageModifier -= blockModifier;
		eleModifier -=blockModifier;

		damageModifier -= playerStats.damageReduction;
		eleModifier -= ((float)playerStats.elementalWeakness[System.Convert.ToInt32(element)])/100;

		damage = damage*damageModifier;
		elementDamage = elementDamage*eleModifier;
		damageTaken = damage+elementDamage;
		playerStats.health -= damageTaken;
		GameObject popup = Instantiate(Resources.Load("CombatResources/DamagePopUp"),new Vector3(transform.position.x, transform.position.y+3, transform.position.z)-transform.right, Quaternion.identity) as GameObject;
		popup.GetComponent<TextMesh>().text = damageTaken.ToString("0");
		updateStats();
		return damageTaken;
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
		blockTimer -= Time.deltaTime;
		if(blockTimer <= 0){
			blockTimer = 0;
			CancelInvoke("BlockCountDown");
		}
	}

	void DodgeCountDown(){
		dodgeTimer -= Time.deltaTime;
		if(dodgeTimer <= 0){
			dodgeTimer = 0;
			CancelInvoke("DodgeCountDown");
		}
	}

	public void Dodge(int direction){
		if(dodgeTimer<=0 && !defended){
			defended = true;
			//Dodge animation depending on direction | 1 = right and down | 0 = left and up
			dodgeTimer += dodgeDuration;
			InvokeRepeating("DodgeCountDown",0, Time.deltaTime);
		}
	}

	public void Block(){
		if(blockTimer<=0 && !defended){
			defended = true;
			//Block 
			blockTimer += blockDuration;
			InvokeRepeating("BlockCountDown", 0, Time.deltaTime);
		}
	}
}
