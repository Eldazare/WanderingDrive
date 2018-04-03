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

	

	void Start() {/* 
		string enemyType = "enemySmall";
		int id = 0;
		enemyStats = EnemyStatCreator.LoadStatBlockData(id, enemyType);
		//enemyName = "Enemy";
		enemyName = NameDescContainer.GetName((NameType)System.Enum.Parse(typeof(NameType), enemyType), id);
		updateStats(); */
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
	public string GetHit (int damage, int elementDamage, Element element, int part){
		float damageTaken, damageTakenModifier=1;
		if(element == 0){
			elementDamage = 0;
		}else{
			damageTakenModifier = enemyStats.elementWeakness[System.Convert.ToInt32(element)];
		}

		//0-100 vs 0-1.0
		if(Random.Range(0, 100)<enemyStats.partList[part].percentageHit){
			//Damage reduction calculations
			damageTaken = (damage+elementDamage)*damageTakenModifier;
			enemyStats.health -= damageTaken;
			enemyStats.partList[part].DamageThisPart(damageTaken); // Part takes damage
			// animator.SetTrigger("Ouch");
			updateStats();
			if(enemyStats.health <= 0){
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

	//Legacy
	/* public void playerAttackPart(int part){
		combatController.menuController.ChoosePartToAttack();
	} */

	public void ActivatePartCanvas(){

		//Legacy
		/* foreach (var item in partCanvas)
		{
			if(item.activeInHierarchy){
				item.SetActive(false);
			}else{
				item.SetActive(true);
			}
		} */
		int i = 0;
		/* foreach (var item in enemyStats.partList)
		{
			if(!item.broken){
				combatController.menuController.selectedPart = i;
				continue;
			}
			i++;
		} */


		if(!combatController.menuController.enemyPartCanvas.activeInHierarchy){
			combatController.menuController.enemyPartCanvas.SetActive(true);
			i = 0;
			foreach (var item in enemyStats.partList){
				combatController.menuController.enemyPartCanvasButtons[i].SetActive(true);
				if(!enemyStats.partList[i].broken){
					combatController.menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text>().text = enemyStats.partList[i].name +"\n"+enemyStats.partList[i].percentageHit+"%";
				}else{
					combatController.menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text>().text = enemyStats.partList[i].name +"\nBroken";
					combatController.menuController.enemyPartCanvasButtons[i].GetComponent<Image>().color = Color.red;
					combatController.menuController.enemyPartCanvasButtons[i].GetComponent<Button>().enabled = false;
				}
				i++;
			}
		}else{
			foreach (var item in combatController.menuController.enemyPartCanvasButtons)
			{
				item.SetActive(false);
			}
			combatController.menuController.enemyPartCanvas.SetActive(false);
		}
	}


	public EnemyStats ReturnDeadStats(){
		return enemyStats;
	}
}
