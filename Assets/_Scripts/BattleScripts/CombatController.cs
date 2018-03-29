using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

	// public enum Elements{
	// 	fire = 1, water = 2, grass = 3, lightning = 4
	// };

	public GameObject loadOut;
	public List<Enemy> enemyList;
	public PlayerCombatScript player;
	public PlayerStats playerStats;
	public string textBoxMessage;
	string getHitReturn;
	public GameObject enemyHost;
	GameObject mapToBattleContainer;
	public MenuController menuController;
	public TouchControlScript touchController;
	public GameObject enemyCreator;
	public bool enemyAttacked;
	public int enemyTurns;

	//public Text TextBox;
	

	void Start () {

		//Temp enemy instantiation
		GameObject enemyObject = Instantiate(Resources.Load("EnemyModels/BigGolem", typeof(GameObject)),enemyHost.transform.position, enemyHost.transform.rotation, enemyHost.transform) as GameObject;
		Enemy enemy = enemyObject.GetComponent<Enemy>();
		enemyList.Add(enemy);
		menuController.targetedEnemy = enemy;
		enemy.combatController = this;

		/*foreach (var item in loadOut.enemyList){
			float enemySpacing = 0;
			Vector3 enemyPos = enemyHost.transform.position;
			//Adds spacing
			GameObject enemyObject = Instantiate(Resources.Load("CombatResources"+item.ID, typeof(GameObject)),new Vector3(enemyPos.x-enemySpacing, enemyPos.y, enemyPos.z+(enemySpacing*4), Quaternion.identity, enemyHost.transform) as GameObject;
			Enemy enemy = enemyObject.GetComponent<Enemy>();
			enemyList.Add(enemy);
			enemy.combatcontroller = this;
			enemySpacing++;
		}
		menuController.targetedEnemy = enemyList[0];
		*/
		player.playerStats.weapon = WeaponCreator.CreateWeaponStatBlock("sword", 0);
		player.updateStats();

		//Initialize enemy healthbars
		foreach (var item in enemyList)
		{
			int number = 0;
			menuController.enemyHealthBars[number].SetActive(true);
			number++;
		}
	}
	public void enemyAttacks(){
		StartCoroutine(enemyAttacksRoutine());
	}
	public void ResetPlayerDefence(){
		touchController.enemyTurn = true;
	}

	
	public void StartCombat(){

		menuController.PlayersTurn();
	}
	IEnumerator enemyAttacksRoutine(){
		yield return new WaitForSeconds(1.5f);
		touchController.enemyTurn = true;
		for (int i = 0; i < enemyList.Count; i++) {
			StartCoroutine(enemyList[i].Attack());
			yield return new WaitUntil(()=>enemyAttacked);
		}
		touchController.enemyTurn = false;
		yield return new WaitForSeconds(1.5f);
		menuController.PlayersTurn();
	}
	
	public void enemycreation(){
		//Call enemyCreator and give it its things
	}

	public void playerStatCreation(){
		player.playerStats = new PlayerStats(); //Give stats to PlayerStats
	}

	public void HitPlayer(int damage,int elementDamage,Element element, bool area){
		menuController.messageToScreen(player.GetHit(damage, elementDamage, element, area));
		//player.GetHit (damage, elementDamage, element, area);
	}

	public void HitEnemy(int damage, int elementDamage, Element element){
		menuController.messageToScreen(menuController.targetedEnemy.GetHit(damage, elementDamage, element));
	}

	public void updateEnemyStats(float health, float maxhealth, float percentage, Enemy enemy){
		menuController.updateEnemyHealth(health, maxhealth, percentage, enemy);
	}
}
