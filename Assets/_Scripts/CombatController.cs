using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

	// public enum Elements{
	// 	fire = 1, water = 2, grass = 3, lightning = 4
	// };
	public List<Enemy> enemyList;
	public PlayerCombatScript player;
	public PlayerStats playerStats;
	public string textBoxMessage;
	string getHitReturn;
	public GameObject enemyHost;
	GameObject mapToBattleContainer;
	public MenuController menuController;
	public bool enemyAttacked;

	//public Text TextBox;
	

	void Start () {
		//Add    .GetComponent<ContainerLuokka>();
		mapToBattleContainer = GameObject.Find("MapToBattleContainer");
		//Get the right player model
		playerStats.loadStats();
		//Find amount of Enemies and their ID from maptobattlecontainer
		//Instantiate the x amount of enemies and let them know their ID

		//Temp enemy instantiation
		GameObject enemyObject = Instantiate(Resources.Load("Enemy1_EyeRaptor", typeof(GameObject)),enemyHost.transform.position, Quaternion.identity, enemyHost.transform) as GameObject;
		Enemy enemy = enemyObject.GetComponent<Enemy>();
		enemyList.Add(enemy);
		menuController.targetedEnemy = enemy;
		enemy.player = player;
		enemy.combatController = this;
	}
	public void enemyAttacks(){
		StartCoroutine(enemyAttacksRoutine());
	}
	IEnumerator enemyAttacksRoutine(){
		for (int i = 0; i < enemyList.Count; i++) {
			StartCoroutine(enemyList[i].Attack());
			yield return new WaitUntil(()=>enemyAttacked);
		}
		menuController.DefaultButtons.SetActive(true);
	}
	void textBoxUpdate(){
		// Do the thing
	}


	// public section
	
	




}
