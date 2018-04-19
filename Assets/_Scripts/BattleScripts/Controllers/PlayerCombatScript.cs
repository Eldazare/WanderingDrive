﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	float blockTimer, dodgeTimer, blockDuration = 2f, dodgeDuration = 1f, perfectDodge = 0.35f, perfectBlock = 0.15f; //Defensive timers and the accuracy wanted
	float[] blockTiers = { 0.75f, 0.5f, 0.25f };
	bool focusedTurn, focusDefensiveBonus, skipTurn, overloadDamageTakenBonus, focusPlusOverloadTurn, focusPlustOverloadBonus; //Focus and overload logic booleans
	float focusDamageBuff = 1.5f, focusplustoverloadDamageBuff = 2f, overloadDamageBuff = 1.5f, overloadDebuff = 1f, attackedPenalty = 0.5f;
	int overloadedTurn, focusBuffTurns;
	int attackRange = 2; //How close the player moves to the enemy
	bool defended, attacked;
	public int abilityID;
	float distanceCovered, movingLength, startTime, lerpSpeed = 5f;

	//Player buffs that reset every turn and buffs apply them everyturn
	public List<_Buff> playerBuffs = new List<_Buff> ();
	public float buffDamageMultiplier, buffElementDamageMultiplier, healthRegen, staminaRegen, blind, buffDamageReduction;
	public int buffFlatDamage, buffFlatElementDamage, buffArmor;
	public bool stunned, confused, frozen, paralyzed, hold;
	public List<int> buffElementalWeakness = new List<int> { 0, 0, 0, 0, 0, 0 };

	//Buffs and debuffs end here!
	public Animator animator;
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
		stunned = false;
		confused = false;
		frozen = false;
		paralyzed = false;
		hold = false;
		yield return new WaitForSeconds (1f);
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
		playerStats.stamina += staminaRegen;
		if (playerStats.stamina < playerStats.maxStamina) {
			playerStats.stamina = playerStats.maxStamina;
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
			updateStats ();
			yield return new WaitForSeconds (1f);
		}
		menuController.proceed = true;
	}

	public void StatusTextPopUp (string text) {
		GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
		popup.GetComponent<TextMesh> ().text = text;
	}
	IEnumerator AttackRoutine (Enemy target, int part) {
		startTime = Time.time;
		movingLength = Vector3.Distance (transform.position, enemyPos);
		InvokeRepeating ("moveToEnemy", 0, Time.deltaTime);
		yield return new WaitUntil (() => proceed);
		animator.SetTrigger ("Attack");
		proceed = false;
		yield return new WaitUntil (() => proceed);
		proceed = false;
		float damageMod = 1;
		float eleDamageMod = 1;
		if (focusPlustOverloadBonus) {
			damageMod += focusplustoverloadDamageBuff;
			eleDamageMod += focusplustoverloadDamageBuff;
		} else {
			if (focusBuffTurns > 0) {
				damageMod += focusDamageBuff;
				eleDamageMod += focusDamageBuff;
			}
			if (overloadedTurn > 0) {
				damageMod += overloadDamageBuff;
				eleDamageMod += overloadDamageBuff;
			}
		}
		eleDamageMod += buffElementDamageMultiplier;
		damageMod += buffDamageMultiplier;

		if (playerStats.mainHand != null) {
			if (playerStats.mainHand.damage > 0) {
				WeaponAttack (playerStats.mainHand, playerStats.offHand, damageMod * attackedPenalty, eleDamageMod, part);
				attacked = true;
			}

		}
		if (playerStats.offHand != null) {
			if (playerStats.offHand.damage > 0) {
				WeaponAttack (playerStats.offHand, playerStats.mainHand, damageMod * attackedPenalty, eleDamageMod, part);
			}

		}
		yield return new WaitUntil (() => proceed);
		startTime = Time.time;
		movingLength = Vector3.Distance (transform.position, startPos);
		InvokeRepeating ("moveFromEnemy", 0, Time.deltaTime);
		proceed = false;
	}

	void WeaponAttack (WeaponStats weapon, WeaponStats otherWep, float damageMod, float eleDamageMod, int part) {
		if (weapon.damage > 0) {
			float playerDamage = weapon.damage;
			float playerElementDamage = weapon.damage;
			if (otherWep != null) {
				playerDamage += otherWep.damageBonus;
				playerElementDamage += otherWep.elementDamageBonus;
			}
			playerDamage += buffFlatDamage;
			playerElementDamage += buffFlatElementDamage;
			playerDamage *= damageMod;
			playerElementDamage *= eleDamageMod;
			if (blind > 0) {
				if (Random.Range (0, 100) + weapon.accuracyBonus > blind) {
					combatController.HitEnemy (-1, 0, 0, part, weapon.accuracyBonus, weapon.weaknessType);
				} else {
					combatController.HitEnemy (playerDamage, playerElementDamage, weapon.element, part, weapon.accuracyBonus, weapon.weaknessType);
				}
			} else {
				combatController.HitEnemy (playerDamage, playerElementDamage, weapon.element, part, weapon.accuracyBonus, weapon.weaknessType);
			}
		}
	}
	public void Ability (int part) {
		startPos = transform.position;
		enemyPos = menuController.targetedEnemy.transform.position;
		StartCoroutine (AbilityRoutine (playerStats.abilityDamage (abilityID), playerStats.abilityElementDamage (abilityID), playerStats.abilityElement (abilityID), part, abilityID));
	}

	IEnumerator AbilityRoutine (float abilityDamage, float abilityElementDamage, Element abilityElement, int part, int ID) {
		proceed = false;
		animator.SetTrigger ("Attack");
		yield return new WaitUntil (() => proceed);
		playerStats.abilities[ID].UseAbility ();
		proceed = false;
		float damageMod = 1;
		float eleDamageMod = 1;
		if (focusPlustOverloadBonus) {
			damageMod += focusplustoverloadDamageBuff;
			eleDamageMod += focusplustoverloadDamageBuff;
		} else {
			if (focusBuffTurns > 0) {
				damageMod += focusBuffTurns;
				eleDamageMod += focusBuffTurns;
			}
			if (overloadedTurn > 0) {
				damageMod += overloadDamageBuff;
				eleDamageMod += overloadDamageBuff;
			}
		}
		//Buff damage modifier apply
		eleDamageMod += buffElementDamageMultiplier;
		damageMod += buffDamageMultiplier;

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
		InvokeRepeating ("moveFromEnemy", Time.deltaTime, Time.deltaTime);
		proceed = false;
	}

	public void Consumable (int slot) {
		EndPlayerTurn (false);
		focusDefensiveBonus = false;
	}

	public void PlayerFocus () {
		focusDefensiveBonus = true;
		focusBuffTurns = 3;
		menuController.focusEnabled = false;
		_Buff buff = new Focus (2);
		buff.player = this;
		playerBuffs.Add (buff);
		EndPlayerTurn (true);
	}

	public void PlayerOverload () {
		overloadedTurn = 3;
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
			focusPlusOverloadTurn = true;
			focusPlustOverloadBonus = true;
			EndPlayerTurn (true);
		} else {
			_Buff buff = new Overload (2);
			buff.player = this;
			playerBuffs.Add (buff);
			EndPlayerTurn (true);
		}
	}

	void EndPlayerTurn (bool setBool) {
		if (focusBuffTurns > 0) {
			focusBuffTurns--;
		}
		if (focusPlusOverloadTurn) {
			menuController.focusEnabled = false;
			menuController.overloadEnabled = false;
			focusPlusOverloadTurn = setBool;
			menuController.PlayersTurn ();
		} else if (focusedTurn) {
			focusedTurn = false;
			menuController.overloadEnabled = false;
			menuController.PlayersTurn ();
		} else if (overloadedTurn > 0) {
			overloadedTurn--;
			if (overloadedTurn == 0) {
				menuController.focusEnabled = true;
				menuController.overloadEnabled = true;
				combatController.enemyAttacks ();
			} else {
				menuController.PlayersTurn ();
			}
		} else {
			focusedTurn = setBool;
			menuController.focusEnabled = !setBool;
			menuController.overloadEnabled = true;
			focusPlustOverloadBonus = false;
			combatController.enemyAttacks ();
		}
	}

	public string GetHit (float damage, float elementDamage, Element element, bool area, int damageType) {
		string returnedValue = ""; //Returning value to report in TextBox
		//Include modifiers to calculations: 
		//overloadDamageTakenBonus
		//focusDefensiveBonus
		CancelInvoke ("BlockCountDown");
		CancelInvoke ("DodgeCountDown");
		if (damage >= 0 || elementDamage >= 0) {
			if (playerStats.dodgeModifier * dodgeTimer > (dodgeDuration - perfectDodge)) {
				if (area) {
					returnedValue = "You dodged but took " + takeDamage (damage, elementDamage, element, damageType).ToString ("0.#") + " area damage!";
				} else {
					//Dodged attack
					returnedValue = "You dodged the attack!";
					takeDamage (0, 0, 0, 0);
				}
			} else if (blockTimer > 0) {
				if ((playerStats.blockModifier * blockTimer) > (blockDuration - perfectBlock)) {
					returnedValue = "You blocked the attack and took no damage!";
					takeDamage (0, 0, 0, 0);
				} else {
					bool blocked = false;
					foreach (var blockModifier in blockTiers) {
						Debug.Log ("Blockmod*blockdura " + (blockModifier * blockDuration));
						if (blockTimer >= (blockModifier * blockDuration)) {
							returnedValue = "You blocked the attack but took " + takeDamage (damage, elementDamage, element, blockModifier, damageType).ToString ("0.#") + "!";
							blocked = true;
							break;
						}
					}
					if (!blocked) {
						returnedValue = "Your block failed and you took " + takeDamage (damage, elementDamage, element, damageType).ToString ("0.#") + " damage!";
					}
				}
			} else {
				//Damage taken calculations
				returnedValue = "You took " + takeDamage (damage, elementDamage, element, damageType).ToString ("0.#") + " damage!";
			}
		} else {
			returnedValue = "Enemy attack missed!";
		}

		combatController.ResetPlayerDefence ();
		blockTimer = 0;
		dodgeTimer = 0;
		defended = false;
		if (playerStats.health <= 0) {
			combatController.LoseEncounter ();
			returnedValue += "\nYou died!";
		}
		return returnedValue;
	}

	float takeDamage (float damage, float elementDamage, Element element, int damageType) {
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
			damageModifier += overloadDebuff;
			eleModifier += overloadDebuff;
		}

		damageModifier -= playerStats.damageReduction;
		damageModifier -= buffDamageReduction;
		eleModifier -= ((float) playerStats.elementWeakness[System.Convert.ToInt32 (element)]) / 100;

		damage = damage * damageModifier;
		elementDamage = elementDamage * eleModifier;
		damageTaken = damage + elementDamage;
		playerStats.health -= damageTaken;
		if (damageTaken < 0) {
			GameObject popup = Instantiate (Resources.Load ("CombatResources/HealPopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh> ().text = damageTaken.ToString ("0.#");
		} else {
			GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh> ().text = damageTaken.ToString ("0.#");
		}
		updateStats ();
		return damageTaken;
	}

	float takeDamage (float damage, float elementDamage, Element element, float blockModifier, int damageType) {
		float damageTaken, damageModifier = 0, eleModifier = 1;
		if (damageType == 0) {
			damageModifier = CombatController.armorAlgorithmModifier / (CombatController.armorAlgorithmModifier + playerStats.physicalArmor);
		} else if (damageType == 1) {
			damageModifier = CombatController.armorAlgorithmModifier / (CombatController.armorAlgorithmModifier + playerStats.magicArmor);
		} else {
			damageModifier = 1;
		}
		if (overloadedTurn > 0) {
			damageModifier += overloadDebuff;
			eleModifier += overloadDebuff;
		}

		damageModifier -= blockModifier;
		eleModifier -= blockModifier;

		damageModifier -= playerStats.damageReduction;
		damageModifier -= buffDamageReduction;
		eleModifier -= ((float) playerStats.elementWeakness[System.Convert.ToInt32 (element)]) / 100;

		damage = damage * damageModifier;
		elementDamage = elementDamage * eleModifier;
		damageTaken = damage + elementDamage;
		playerStats.health -= damageTaken;
		GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
		popup.GetComponent<TextMesh> ().text = damageTaken.ToString ("0.#");
		updateStats ();
		return damageTaken;
	}

	public void updateStats () {
		menuController.updatePlayerHealth (playerStats.health, playerStats.maxHealth, playerStats.health / playerStats.maxHealth);
	}

	void moveToEnemy () {
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		if (Vector3.Distance (enemyPos, transform.position) > attackRange) {
			transform.position = Vector3.Lerp (transform.position, enemyPos, distanceCovered / movingLength);
			//transform.Translate(((enemyPos-transform.position)+(enemyPos-transform.position).normalized)*Time.deltaTime*5);
		} else {
			proceed = true;
			CancelInvoke ("moveToEnemy");
		}
	}
	void moveFromEnemy () {
		float distanceCovered = (Time.time - startTime) * lerpSpeed;
		if (Vector3.Distance (startPos, transform.position) > 0.1) {
			transform.position = Vector3.Lerp (transform.position, startPos, distanceCovered / movingLength);
			//transform.Translate((startPos-transform.position)*Time.deltaTime*5);
		} else {
			CancelInvoke ("moveFromEnemy");
			EndPlayerTurn (false);
			focusDefensiveBonus = false;
			combatController.cameraScript.ResetCamera ();
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
		if (dodgeTimer <= 0 && !defended) {
			defended = true;
			//Dodge animation depending on direction | 1 = right and down | 0 = left and up
			dodgeTimer += dodgeDuration;
			InvokeRepeating ("DodgeCountDown", 0, Time.deltaTime);
		}
	}

	public void Block () {
		if (blockTimer <= 0 && !defended) {
			defended = true;
			//Block 
			blockTimer += blockDuration;
			InvokeRepeating ("BlockCountDown", 0, Time.deltaTime);
		}
	}
}