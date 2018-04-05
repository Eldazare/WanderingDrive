using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{
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

	public IEnumerator Attack() {
		startPos = transform.position;
		InvokeRepeating("moveToPlayer", 0, Time.deltaTime);
		yield return new WaitUntil(() =>proceed);
		animator.SetTrigger("Attack");
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		// TODO: Verify attack
		int randIndex = Random.Range(0,enemyStats.attackList.Count);
		EnemyAttack chosenAttack = enemyStats.attackList [randIndex];
		combatController.HitPlayer(chosenAttack.damage, chosenAttack.elementDamage, chosenAttack.element, false);
		proceed = false;
		yield return new WaitUntil(() =>proceed);
		InvokeRepeating("moveFromPlayer",0,Time.deltaTime);
		proceed = false;
	}

	void moveToPlayer(){
		if(Vector3.Distance(combatController.player.transform.position, transform.position)>4){
			//transform.Translate((combatController.player.transform.position-transform.position)*Time.deltaTime*enemyStats.quickness);
			transform.position = Vector3.MoveTowards(transform.position, combatController.player.transform.position, Time.deltaTime*enemyStats.quickness+0.5f); //Quickness as movement speed
		}else{
			proceed = true;
			CancelInvoke("moveToPlayer");
		}
	}
	void moveFromPlayer(){
		if(Vector3.Distance(startPos, transform.position)>0.1){
			//transform.Translate((startPos-transform.position)*Time.deltaTime*enemyStats.quickness);
			transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime*enemyStats.quickness+1); //Quickness as movement speed
		}else{
			CancelInvoke("moveFromPlayer");
			combatController.enemyAttacked = true;
		}
	}
	public string GetHit (float damage, float elementDamage, Element element, int part, float damageMod){
		float damageTaken, damageModifier=1+damageMod, eleModifier = 1;
		//0-100
		if(Random.Range(0, 100)<enemyStats.partList[part].percentageHit){
			//Damage reduction calculations

			eleModifier += enemyStats.elementWeakness[System.Convert.ToInt32(element)]/100;
			damageModifier += enemyStats.armor;

			eleModifier += enemyStats.partList[part].damageMod;
			damageModifier += enemyStats.partList[part].damageMod;

			damage *= damageModifier;
			elementDamage *= eleModifier;

			damageTaken = damage+elementDamage;

			enemyStats.health -= damageTaken;
			enemyStats.partList[part].DamageThisPart(damageTaken); // Part takes damage
			// animator.SetTrigger("Ouch");
			updateStats();
			GameObject popup = Instantiate(Resources.Load("CombatResources/DamagePopUp"),new Vector3(transform.position.x, transform.position.y+3, transform.position.z), Quaternion.identity) as GameObject;
			popup.GetComponent<TextMesh>().text = damageTaken.ToString("00");
			if(enemyStats.health <= 0){
				combatController.EnemyDies(this);
				//animator.SetTrigger("Death");
				return enemyName+" took "+damageTaken+" damage and died!";
			}else{
				return enemyName+" took "+damageTaken+" damage!";
			}
		}else{
			return "Attack missed!";
		}
	}

	public void updateStats(){
		combatController.updateEnemyStats(enemyStats.health, enemyStats.maxHealth, this);
	}
}
