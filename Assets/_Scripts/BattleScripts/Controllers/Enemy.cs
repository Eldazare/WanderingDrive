﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    public EnemyStats enemyStats;
    public Animator animator;
    Vector3 startPos;
    public CombatController combatController;

    public string enemyName;
    public int enemyID;
    Image healthBar;
    public bool proceed;
    public GameObject cameraTarget;
    public GameObject[] partCanvas; //Legacy on target visual buttons
    public GameObject[] partButtons;

    //Buffs have to know Enemy instead of EnemyStats so they can damage the right enemy with GetHit
    public List<_Buff> enemyBuffList = new List<_Buff> ();
    public float buffDamageMultiplier, buffElementDamageMultiplier, healthRegen, blind, buffDamageReduction;
    public int buffFlatDamage, buffFlatElementDamage, buffArmor;
    public bool stunned, confused, frozen, paralyzed, hold;
    public List<int> buffElementalWeakness = new List<int> { 0, 0, 0, 0, 0, 0 };

    float weaknessTypeAccuracyAmount = 20f;
    //Buffs end

    public IEnumerator Attack () {
        startPos = transform.position;
        yield return new WaitUntil (() => proceed);
        proceed = false;
        InvokeRepeating ("MoveToPlayer", 0, Time.deltaTime);
        yield return new WaitUntil (() => proceed);
        proceed = false;
        animator.SetTrigger ("Attack");
        yield return new WaitUntil (() => proceed);
        proceed = false;
        // TODO: Verify attack
        int randIndex = Random.Range (0, enemyStats.attackList.Count);
        EnemyAttack chosenAttack = enemyStats.attackList[randIndex];
        float attackDamage = chosenAttack.damage;
        float attackEleDamage = chosenAttack.elementDamage;
        float damageMod = 1;
        float eleDamageMod = 1;

        attackDamage += buffFlatDamage;
        attackEleDamage += buffFlatElementDamage;
        damageMod *= 1 + buffDamageMultiplier;
        eleDamageMod *= 1 + buffElementDamageMultiplier;
        Attack newAttack = new Attack(attackDamage, attackEleDamage,chosenAttack.element, damageMod, eleDamageMod, 0, chosenAttack.damageType);
        AttackResult result;
        result = combatController.UniversalDamageTaken(false, newAttack);
        if (Random.Range (0, 100) < blind) {
            //combatController.HitPlayer (-1, -1, 0, false, 0);
            result.dodged = true;
        } else {
            //combatController.HitPlayer (attackDamage * damageMod, attackEleDamage * eleDamageMod, chosenAttack.element, false, chosenAttack.damageType);
        }
        combatController.HitPlayer(result);
        yield return new WaitUntil (() => proceed);
        proceed = false;
        InvokeRepeating ("MoveFromPlayer", 0, Time.deltaTime);
    }
    public void ApplyEnemyBuffs () {
        StartCoroutine (ApplyBuffs ());
    }
    IEnumerator ApplyBuffs () {
        buffDamageMultiplier = 0;
        buffArmor = 0;
        buffElementDamageMultiplier = 0;
        buffFlatDamage = 0;
        buffFlatElementDamage = 0;
        healthRegen = 0;
        blind = 0;
        stunned = false;
        confused = false;
        frozen = false;
        paralyzed = false;
        hold = false;
        for (int i = 0; i < buffElementalWeakness.Count; i++) {
            buffElementalWeakness[i] = 0;
        }
        foreach (var item in enemyBuffList) {
            if (item != null) {
                if (item.turnsRemaining == 0) {
                    enemyBuffList[enemyBuffList.IndexOf (item)] = null;
                } else {
                    yield return new WaitForSeconds (item.DoYourThing ());
                }
                item.turnsRemaining--;
            }
        }
        if (healthRegen > 0) {
            enemyStats.health += healthRegen;
            if (enemyStats.health > enemyStats.maxHealth) {
                enemyStats.health = enemyStats.maxHealth;
            }
            GameObject popup = Instantiate (Resources.Load ("CombatResources/HealPopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
            popup.GetComponent<TextMesh> ().text = healthRegen.ToString ("0.#");
            updateStats ();
            yield return new WaitForSeconds (1f);
        }
        proceed = true;
    }
    public Defense EnemyDefense(){
        float damageMod =  enemyStats.partList[combatController.menuController.selectedPart].damageMod, chanceToHit = enemyStats.partList[combatController.menuController.selectedPart].percentageHit;
        chanceToHit -= blind;
        Defense defense = new Defense(enemyStats.armor, 0, enemyStats.elementWeakness, enemyStats.weaknessType, damageMod, chanceToHit);
        return defense;
    }

    public string GetHit(AttackResult attack){
    //public string GetHit (float damage, float elementDamage, Element element, int part, float accuracy, WeaknessType weaknessType) {
        EnemyPart part =  enemyStats.partList[combatController.menuController.selectedPart];
        float damageTaken = attack.damage+attack.elementDamage;
       
        if (!attack.dodged) {
            part.DamageThisPart (damageTaken);
            enemyStats.health -= damageTaken;
            combatController.menuController.targetHealthBar.GetComponent<TargetEnemyHealthBar> ().UpdateCurrentHP ();
            // animator.SetTrigger("TakeDamage"); 
            if (damageTaken < 0) {
                GameObject popup = Instantiate (Resources.Load ("CombatResources/HealPopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity) as GameObject;
                popup.GetComponent<TextMesh> ().text = damageTaken.ToString ("0.#");
            } else {
                GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z), Quaternion.identity) as GameObject;
                popup.GetComponent<TextMesh> ().text = damageTaken.ToString ("0.#");
            }
            string message;
            if (attack.elementDamage != 0) {
                message = attack.damage.ToString ("0.#") + " + " + attack.elementDamage.ToString ("0.#") + " " + attack.element.ToString ();
                message = MenuController.DamageTextColor(message);
            } else {
                message = attack.damage.ToString ("0.#");
            }
            if (enemyStats.health <= 0) {
                combatController.EnemyDies (this);
                //animator.SetTrigger("Death");
                return enemyName + " took " + message + " damage and died!";
            } else {
                return enemyName + " took " + message + " damage!";
            }
        } else {
            return "Your attack missed!";
        }
    }
    public void StatusTextPopUp (string text) {
        GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (transform.position.x, transform.position.y + 3, transform.position.z) - transform.right, Quaternion.identity) as GameObject;
        popup.GetComponent<TextMesh> ().text = text;
    }
    public void RemoveFromBuffList (_Buff buff) {
        enemyBuffList.Remove (buff);
    }
    void MoveToPlayer () {
        if (Vector3.Distance (combatController.player.transform.position, transform.position) > 4) {
            //transform.Translate((combatController.player.transform.position-transform.position)*Time.deltaTime*enemyStats.quickness);
            transform.position = Vector3.MoveTowards (transform.position, combatController.player.transform.position, Time.deltaTime * enemyStats.quickness + 0.5f); //Quickness as movement speed
        } else {
            proceed = true;
            CancelInvoke ("MoveToPlayer");
        }
    }
    void MoveFromPlayer () {
        if (Vector3.Distance (startPos, transform.position) > 0.1) {
            //transform.Translate((startPos-transform.position)*Time.deltaTime*enemyStats.quickness);
            transform.position = Vector3.MoveTowards (transform.position, startPos, Time.deltaTime + 3); //Quickness as movement speed
        } else {
            CancelInvoke ("MoveFromPlayer");
            combatController.enemyAttacked = true;
        }
    }
    public void updateStats () {
        combatController.updateEnemyStats (enemyStats.health, enemyStats.maxHealth, this);
    }
}