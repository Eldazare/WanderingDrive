using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerPopUpColor{
	Damage, Healing, Stamina, Status
};
public class PlayerCombatScript : MonoBehaviour {
	public PlayerStats playerStats = new PlayerStats (); //Player stat container
	Vector3 startPos; //Player's starting position to move to and from Enemy
	public GameObject weapon, weaponSlot; //Drag weaponSlot from hierarchy under player model, right hand
	public Transform stomach;

	Vector3 enemyPos; //Enemy position to move to and from it
	[HideInInspector]
	public bool proceed; //Used in moving to and from the targeted enemy
	public MenuController menuController; //Drag from hierarchy
	public CombatController combatController; //Drag from hierarchy

	//Perfect values are: if timer > maxDuration - perfect| Tiers are: if timer > maxDuration * tier
	float blockTimer, dodgeTimer, blockDuration = 2f, dodgeDuration = 0.5f, perfectDodge = 0.35f, perfectBlock = 0.15f; //Defensive timers and the accuracy wanted
	float[] blockTiers = { 0.75f, 0.5f, 0.25f };
	bool focusedTurn, skipTurn, overloadDamageTakenBonus; //Focus and overload logic booleans
	float focusDamageBuff = 1.5f, overloadDamageBuff = 1.5f, overloadDebuff = 2f, attackedPenalty = 0.5f, focusAccuracyBuff = 20f;
	int overloadedTurn, focusBuffTurns;
	int attackRange = 6; //How close the player moves to the enemy
	public bool defended, attacked;
	public int abilityID;
	float distanceCovered, movingLength, startTime, lerpSpeed = 4f, overFocusTurn;

	//Player buffs that reset every turn and buffs apply them everyturn
	public List<_Buff> playerBuffs = new List<_Buff> ();
	public float accuracyBuff, buffDamageMultiplier, buffElementDamageMultiplier, healthRegen, staminaRegen, blind, buffDamageReduction;
	public int buffFlatDamage, buffFlatElementDamage, buffArmor;
	public bool stunned, confused, frozen, paralyzed, hold;
	public List<int> buffElementalWeakness = new List<int> { 0, 0, 0, 0, 0, 0 };

	//Buffs and debuffs end here!
	public Animator animator;
	public float damageDone, elementDamageDone;
	public Element elementDone;
	public List<Attack> attackList = new List<Attack> ();
	public bool enemyTurn;

	public void Attack (int part) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine (AttackRoutine (menuController.targetedEnemy, part));
	}
	public void ApplyPlayerBuffs () {
		StartCoroutine (ApplyBuffs ());
	}
	IEnumerator ApplyBuffs () {
		buffDamageMultiplier = 0;
		buffArmor = 0;
		buffElementDamageMultiplier = 0;
		buffFlatDamage = 0;
		buffFlatElementDamage = 0;
		healthRegen = 0;
		staminaRegen = 0;
		blind = 0;
		accuracyBuff = 0;
		stunned = false;
		confused = false;
		frozen = false;
		paralyzed = false;
		hold = false;
		for (int i = 0; i < buffElementalWeakness.Count; i++) {
			buffElementalWeakness[i] = 0;
		}
		foreach (_Buff item in playerBuffs) {
			if (item != null) {
				if (item.turnsRemaining == 0) {
					playerBuffs[playerBuffs.IndexOf (item)] = null;
				} else {
					yield return new WaitForSeconds (item.DoYourThing ());
				}
				item.turnsRemaining--;
			}
		}
		if(staminaRegen != 0){
			playerStats.stamina += staminaRegen;
			if (playerStats.stamina < playerStats.maxStamina) {
				playerStats.stamina = playerStats.maxStamina;
			}
		}
		

		if (healthRegen > 0) {
			playerStats.health += healthRegen;
			if (playerStats.health > playerStats.maxHealth) {
				healthRegen -= playerStats.health - playerStats.maxHealth;
				playerStats.health = playerStats.maxHealth;
			}
			yield return new WaitForSeconds (1f);
			GameObject popup = Instantiate (Resources.Load ("CombatResources/HealPopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh> ().text = healthRegen.ToString ("0");
			UpdateStats ();
			yield return new WaitForSeconds (1f);
		}
		menuController.proceed = true;
	}
	IEnumerator AttackRoutine (Enemy target, int part) {
		attacked = false;
		startTime = Time.time;
		movingLength = Vector3.Distance (transform.position, enemyPos);
		InvokeRepeating ("MoveToEnemy", 0, Time.deltaTime);
		yield return new WaitUntil (() => proceed);
		proceed = false;
		List<ComboPieceAbstraction> tempList = new List<ComboPieceAbstraction> ();
		combatController.GetComponent<ComboManager> ().StartCombo (tempList);
		yield return new WaitUntil (() => proceed);
		proceed = false;
		animator.SetTrigger ("Attack");
		yield return new WaitUntil (() => proceed);
		proceed = false;
		float damageMod = combatController.comboMulti;
		float eleDamageMod = combatController.comboMulti;
		if (overFocusTurn > 0) {
			damageMod *= focusDamageBuff * overloadDamageBuff;
			eleDamageMod *= focusDamageBuff * overloadDamageBuff;
		} else {
			if (focusBuffTurns > 0) {
				damageMod *= focusDamageBuff;
				eleDamageMod *= focusDamageBuff;
				accuracyBuff += focusAccuracyBuff;
			}
			if (overloadedTurn > 0) {
				damageMod *= overloadDamageBuff;
				eleDamageMod *= overloadDamageBuff;
			}
		}
		eleDamageMod += buffElementDamageMultiplier;
		damageMod += buffDamageMultiplier;

		if (playerStats.mainHand != null) {
			if (playerStats.mainHand.damage > 0) {
				WeaponAttack (playerStats.mainHand, playerStats.offHand, damageMod, eleDamageMod, part);
				attacked = true;
			}
		}
		if (playerStats.offHand != null) {
			if (playerStats.offHand.damage > 0) {
				WeaponAttack (playerStats.offHand, playerStats.mainHand, damageMod, eleDamageMod, part);
			}
		}
		yield return new WaitUntil (() => proceed);
		startTime = Time.time;
		movingLength = Vector3.Distance (transform.position, startPos);
		InvokeRepeating ("MoveFromEnemy", 0, Time.deltaTime);
		proceed = false;
	}

	void WeaponAttack (WeaponStats weapon, WeaponStats otherWep, float damageMod, float eleDamageMod, int part) {
		if (weapon.damage > 0) {
			float playerDamage = weapon.damage;
			float playerElementDamage = weapon.elementDamage;
			if (otherWep != null) {
				playerDamage += otherWep.damageBonus;
				playerElementDamage += otherWep.elementDamageBonus;
			}
			playerDamage += buffFlatDamage;
			playerElementDamage += buffFlatElementDamage;
			if (attacked) {
				damageMod *= attackedPenalty;
				eleDamageMod *= attackedPenalty;
			}
			playerDamage *= damageMod;
			playerElementDamage *= eleDamageMod;
			if (blind > 0) {
				if ((Random.Range (0, 100) + weapon.accuracyBonus + accuracyBuff )> blind) {
					combatController.HitEnemy (-1, 0, 0, part, weapon.accuracyBonus + accuracyBuff, weapon.weaknessType);
				} else {
					combatController.HitEnemy (playerDamage, playerElementDamage, weapon.element, part, weapon.accuracyBonus + accuracyBuff, weapon.weaknessType);
				}
			} else {
				combatController.HitEnemy (playerDamage, playerElementDamage, weapon.element, part, weapon.accuracyBonus + accuracyBuff, weapon.weaknessType);
			}
		}
	}
	void WeaponAttackCalculation (WeaponStats weapon, WeaponStats otherWep, float damageMod, float eleDamageMod) {
		float playerDamage = weapon.damage;
		float playerElementDamage = weapon.elementDamage;
		if (otherWep != null) {
			playerDamage += otherWep.damageBonus;
			playerElementDamage += otherWep.elementDamageBonus;
		}
		playerDamage += buffFlatDamage;
		playerElementDamage += buffFlatElementDamage;
		playerDamage *= damageMod;
		playerElementDamage *= eleDamageMod;
		Attack attack = new Attack (playerDamage, playerElementDamage, weapon.element);
		attackList.Add (attack);
	}
	public void CalculateDamage (AttackMode attackmod) {
		attackList.Clear ();
		float damageMod = 1;
		float eleDamageMod = 1;
		damageDone = 0;
		elementDamageDone = 0;
		switch (attackmod) {
			//Generate weapon attacks
			case AttackMode.Attack:
				if (overFocusTurn > 0) {
					damageMod *= focusDamageBuff * overloadDamageBuff;
					eleDamageMod *= focusDamageBuff * overloadDamageBuff;
				} else {
					if (focusBuffTurns > 0) {
						damageMod *= focusDamageBuff;
						eleDamageMod *= focusDamageBuff;
					}
					if (overloadedTurn > 0) {
						damageMod *= overloadDamageBuff;
						eleDamageMod *= overloadDamageBuff;
					}
				}
				eleDamageMod *= buffElementDamageMultiplier + 1;
				damageMod *= buffDamageMultiplier + 1;
				if (playerStats.mainHand != null) {
					if (playerStats.mainHand.damage > 0) {
						WeaponAttackCalculation (playerStats.mainHand, playerStats.offHand, damageMod, eleDamageMod);
						attacked = true;
					}
				}
				if (playerStats.offHand != null) {
					if (playerStats.offHand.damage > 0) {
						WeaponAttackCalculation (playerStats.offHand, playerStats.mainHand, damageMod, eleDamageMod);
					}
				}
				break;
				//Generate ability attack
			case AttackMode.Ability:
				if (overFocusTurn > 0) {
					damageMod *= focusDamageBuff * overloadDamageBuff;
					eleDamageMod *= focusDamageBuff * overloadDamageBuff;
				} else {
					if (focusBuffTurns > 0) {
						damageMod *= focusDamageBuff;
						eleDamageMod *= focusDamageBuff;
					}
					if (overloadedTurn > 0) {
						damageMod *= overloadDamageBuff;
						eleDamageMod *= overloadDamageBuff;
					}
				}
				eleDamageMod += buffElementDamageMultiplier;
				damageMod += buffDamageMultiplier;
				Attack attack = new Attack (playerStats.abilities[abilityID].damage * damageMod,
					playerStats.abilities[abilityID].elementDamage * eleDamageMod, playerStats.abilities[abilityID].element);
				attackList.Add (attack);
				break;
				//Generate item's attack
			case AttackMode.Item:

				break;
			default:
				break;
		}

	}
	public void Ability (int part) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		if (!playerStats.abilities[abilityID].offensive) {
			Debug.Log("Utility");
			StartCoroutine(UtilityAbilityRoutine());
		} else {
			StartCoroutine (OffensiveAbilityRoutine (playerStats.AbilityDamage (abilityID), playerStats.AbilityElementDamage (abilityID), playerStats.AbilityElement (abilityID), part, abilityID));
		}
	}
	IEnumerator UtilityAbilityRoutine(){
		playerStats.abilities[abilityID].UseAbility();
		yield return new WaitForSeconds(0.5f);
		EndPlayerTurn(false);
	}
	IEnumerator OffensiveAbilityRoutine (float abilityDamage, float abilityElementDamage, Element abilityElement, int part, int ID) {
		proceed = false;
		animator.SetTrigger ("Attack");
		yield return new WaitUntil (() => proceed);
		playerStats.abilities[ID].UseAbility ();
		proceed = false;
		Debug.Log ("Damage1: " + abilityDamage);
		Debug.Log ("DamageEle1: " + abilityElementDamage);
		float damageMod = 1;
		float eleDamageMod = 1;
		if (overFocusTurn > 0) {
			damageMod *= focusDamageBuff * overloadDamageBuff;
			eleDamageMod *= focusDamageBuff * overloadDamageBuff;
		} else {
			if (focusBuffTurns > 0) {
				damageMod *= focusDamageBuff;
				eleDamageMod *= focusDamageBuff;
			}
			if (overloadedTurn > 0) {
				damageMod *= overloadDamageBuff;
				eleDamageMod *= overloadDamageBuff;
			}
		}
		//Buff damage modifier apply
		eleDamageMod *= buffElementDamageMultiplier + 1;
		damageMod *= buffDamageMultiplier + 1;
		Debug.Log ("DamageMod: " + damageMod);
		Debug.Log ("DamageEleMod: " + eleDamageMod);
		//Buff flat damage apply
		abilityDamage += buffFlatDamage;
		abilityElementDamage += buffFlatElementDamage;

		abilityDamage *= damageMod;
		abilityElementDamage *= eleDamageMod;
		yield return new WaitUntil (() => proceed);
		combatController.HitEnemy (abilityDamage, abilityElementDamage, abilityElement, part, 0, 0);
		proceed = false;
		yield return new WaitUntil (() => proceed);
		startTime = Time.time;
		movingLength = Vector3.Distance (transform.position, startPos);
		InvokeRepeating ("MoveFromEnemy", Time.deltaTime, Time.deltaTime);
		proceed = false;
	}

	public void CombatItem (int slot) {
		StartCoroutine (CombatItemRoutine (slot));
	}
	IEnumerator CombatItemRoutine (int slot) {
		playerStats.combatItems[slot].ActivateCombatConsumable ();
		yield return new WaitForSeconds (0.5f);
		EndPlayerTurn (false);
	}

	
	
	//Player Focus
	public void PlayerFocus () {
		focusBuffTurns = 3;
		menuController.focusEnabled = false;
		_Buff buff = new Focus (2);
		buff.player = this;
		playerBuffs.Add (buff);
		EndPlayerTurn (true);
	}
	//Player Overload
	public void PlayerOverload () {
		combatController.enemyTurns = 2;
		menuController.focusEnabled = false;
		menuController.overloadEnabled = false;
		if (focusedTurn) {
			foreach (var item in playerBuffs) {
				if (item != null) {
					if (item.GetType ().Name == "Focus") {
						playerBuffs[playerBuffs.IndexOf (item)] = null;
					}
					if (item.GetType ().Name == "Overload") {
						playerBuffs[playerBuffs.IndexOf (item)] = null;
					}
				}
			}
			_Buff buff = new OverFocus (4);
			buff.player = this;
			playerBuffs.Add (buff);
			overFocusTurn = 5;
			focusBuffTurns = 0;
			overloadedTurn = 0;
			EndPlayerTurn (true);
		} else {
			overloadedTurn = 3;
			_Buff buff = new Overload (2);
			buff.player = this;
			playerBuffs.Add (buff);
			EndPlayerTurn (true);
		}
	}
	//Ending player's turn and checking various modifiers to that
	void EndPlayerTurn (bool setBool) {
		if (playerStats.health > 0) {
			if (focusBuffTurns > 0) {
				focusBuffTurns--;
			}
			if (overFocusTurn > 0) {
				overFocusTurn--;
				menuController.focusEnabled = false;
				menuController.overloadEnabled = false;
				if (overFocusTurn == 0) {
					combatController.EnemyAttacks ();
				} else {
					menuController.PlayersTurn ();
				}
			} else if (focusedTurn) {
				focusedTurn = false;
				menuController.overloadEnabled = false;
				menuController.PlayersTurn ();
			} else if (overloadedTurn > 0) {
				overloadedTurn--;
				if (overloadedTurn == 0) {
					menuController.focusEnabled = true;
					menuController.overloadEnabled = true;
					combatController.EnemyAttacks ();
				} else {
					menuController.PlayersTurn ();
				}
			} else {
				focusedTurn = setBool;
				menuController.focusEnabled = !setBool;
				menuController.overloadEnabled = true;
				combatController.EnemyAttacks ();
			}
		} else {
			combatController.LoseEncounter ();
		}
	}

	//Dodge and block modifier check when getting hit
	public string GetHit (float damage, float elementDamage, Element element, bool area, int damageType) {
		string returnedValue = ""; //Returning value to report in TextBox
		//Include modifiers to calculations: 
		//overloadDamageTakenBonus
		//focusDefensiveBonus
		CancelInvoke ("BlockCountDown");
		CancelInvoke ("DodgeCountDown");
		if (playerStats.speed != 0) {
			playerStats.dodgeModifier = playerStats.speed;
		}
		Debug.Log ("Dodge Timer: " + dodgeTimer * playerStats.dodgeModifier);
		Debug.Log ("Block Timer: " + playerStats.blockModifier * blockTimer);
		if (damage >= 0 || elementDamage >= 0) {
			if (playerStats.dodgeModifier * dodgeTimer > (dodgeDuration - perfectDodge)) {
				if (area) {
					returnedValue = "You dodged but took " + TakeDamage (damage, elementDamage, element, 1, damageType) + " area damage!";
				} else {
					//Dodged attack
					returnedValue = "You dodged the attack!";
					TakeDamage (0, 0, 0, 1, 0);
				}
			} else if (blockTimer > 0) {
				Debug.Log ("Block Timer: " + blockTimer * playerStats.blockModifier);
				if ((playerStats.blockModifier * blockTimer) > (blockDuration - perfectBlock)) {
					returnedValue = "You blocked the attack and took no damage!";
					TakeDamage (0, 0, 0, 1, 0);
				} else {
					bool blocked = false;
					foreach (var blockModifier in blockTiers) {
						if (blockTimer >= (blockModifier * blockDuration)) {
							returnedValue = "You blocked the attack but took " + TakeDamage (damage, elementDamage, element, 1 - blockModifier, damageType) + "!";
							blocked = true;
							break;
						}
					}
					if (!blocked) {
						returnedValue = "Your block failed and you took " + TakeDamage (damage, elementDamage, element, 1, damageType) + " damage!";
					}
				}
			} else {
				//Damage taken calculations
				returnedValue = "You took " + TakeDamage (damage, elementDamage, element, 1, damageType) + " damage!";
			}
		} else {
			returnedValue = "Enemy attack missed!";
		}
		blockTimer = 0;
		dodgeTimer = 0;
		if (playerStats.health <= 0) {
			combatController.LoseEncounter ();
			returnedValue += "\nYou died!";
		}
		return returnedValue;
	}
	//Damage taken calculations
	string TakeDamage (float damage, float elementDamage, Element element, float blockModifier, int damageType) {
		float damageTaken, damageModifier = 0, eleModifier = 1;
		float armorAlgMod = CombatController.armorAlgorithmModifier;
		if (damageType == 0) {
			damageModifier = armorAlgMod / (armorAlgMod + playerStats.physicalArmor);
		} else if (damageType == 1) {
			damageModifier = armorAlgMod / (armorAlgMod + playerStats.magicArmor);
		} else {
			damageModifier = 1;
		}
		if (element == Element.None) {
			damage += elementDamage;
			elementDamage = 0;
		}
		if (overloadedTurn > 0) {
			damageModifier *= overloadDebuff;
			eleModifier *= overloadDebuff;
		}
		damageModifier *= blockModifier;
		eleModifier *= blockModifier;
		

		if (buffDamageReduction > 0) {
			damageModifier *= buffDamageReduction;
		}
		Debug.Log("b4elementDamage : " +elementDamage);
		Debug.Log("b4elemod " + eleModifier);
		eleModifier *= 1-(((float) playerStats.elementWeakness[System.Convert.ToInt32 (element)]) / 100);
		Debug.Log("elemod " + eleModifier);
		damage *= damageModifier;
		elementDamage *= eleModifier;
		Debug.Log("elementDamage : " +elementDamage);
		damageTaken = damage + elementDamage;
		playerStats.health -= damageTaken;
		if (damageTaken < 0) {
			PopUpText(damageTaken.ToString("0.#"),PlayerPopUpColor.Healing);
		} else {
			PopUpText(damageTaken.ToString("0.#"),PlayerPopUpColor.Damage);
		}
		UpdateStats ();
		if (elementDamage != 0) {
			return damage.ToString ("0.#") + " + " + elementDamage.ToString ("0.#") + " " + element.ToString();
		} else {
			return damage.ToString ("0.#");
		}
	}

	public void UpdateStats () {
		menuController.UpdatePlayerHealth (playerStats.health, playerStats.maxHealth, playerStats.health / playerStats.maxHealth);
	}
	//InGame PopUp Method
	public void PopUpText (string popuptext, PlayerPopUpColor popUpColor) {
		GameObject popup;
		switch (popUpColor)
		{
			case PlayerPopUpColor.Damage:
				popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
				popup.GetComponent<TextMesh> ().text = popuptext;
				break;
			case PlayerPopUpColor.Healing:
				popup = Instantiate (Resources.Load ("CombatResources/HealPopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
				popup.GetComponent<TextMesh> ().text = popuptext;
				break;
			case PlayerPopUpColor.Stamina:
				popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
				popup.GetComponent<TextMesh> ().text = popuptext;
				popup.GetComponent<TextMesh>().color = new Color(255, 189, 102);
				break;

			case PlayerPopUpColor.Status:
				popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
				popup.GetComponent<TextMesh> ().text = popuptext;
				popup.GetComponent<TextMesh>().color = Color.yellow;
				break;
			default:
				break;
		}
	}

	void MoveToEnemy () {
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		if (Vector3.Distance (enemyPos, transform.position) > attackRange) {
			transform.position = Vector3.Lerp (transform.position, enemyPos, (distanceCovered / movingLength) * 4);
			//transform.Translate(((enemyPos-transform.position)+(enemyPos-transform.position).normalized)*Time.deltaTime*5);
		} else {
			proceed = true;
			CancelInvoke ("MoveToEnemy");
		}
	}
	void MoveFromEnemy () {
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		if (Vector3.Distance (startPos, transform.position) > 0.1) {
			transform.position = Vector3.Lerp (transform.position, startPos, distanceCovered / movingLength * 5);
			//transform.Translate((startPos-transform.position)*Time.deltaTime*5);
		} else {
			CancelInvoke ("MoveFromEnemy");
			EndPlayerTurn (false);
			combatController.cameraScript.ResetCameraAfterAttack ();
		}
	}
	void BlockCountDown () {
		blockTimer -= Time.deltaTime;
		if (blockTimer <= 0) {
			blockTimer = 0;
			CancelInvoke ("BlockCountDown");
		}
	}

	void DodgeCountDown () {
		dodgeTimer -= Time.deltaTime;
		if (dodgeTimer <= 0) {
			dodgeTimer = 0;
			CancelInvoke ("DodgeCountDown");
		}
	}

	public void Dodge (int direction) {
		if (dodgeTimer <= 0 && !defended && enemyTurn) {
			defended = true;
			//Dodge animation depending on direction | 1 = right and down | 0 = left and up
			dodgeTimer += dodgeDuration;
			animator.SetTrigger("Dodge");
			InvokeRepeating ("DodgeCountDown", 0, Time.deltaTime);
		}
	}

	public void Block () {
		if (blockTimer <= 0 && !defended && enemyTurn) {
			defended = true;
			//Block 
			blockTimer += blockDuration;
			animator.SetTrigger("Block");
			InvokeRepeating ("BlockCountDown", 0, Time.deltaTime);
		}
	}
}