using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CombatController : MonoBehaviour {

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
	public CameraController cameraScript;
	public bool enemyAttacked;
	List<EnemyStats> deadEnemies;
	public int enemyTurns;
		
	void Start () {

		//Temp enemy instantiation
		/* GameObject enemyObject = Instantiate(Resources.Load("EnemyModels/BigGolem", typeof(GameObject)),enemyHost.transform.position, enemyHost.transform.rotation, enemyHost.transform) as GameObject;
		Enemy enemy = enemyObject.GetComponent<Enemy>();
		enemyList.Add(enemy);
		menuController.targetedEnemy = enemy;
		enemy.combatController = this; 
		int number = 0;
		foreach (var item in enemyList)
		{
			menuController.enemyHealthBars[number].SetActive(true);
			number++;
		}*/

		/*
		player.playerStats.weapon = WeaponCreator.CreateWeaponStatBlock("sword", 0);
		player.weapon = Instantiate(Resources.Load("CombatResources/WeaponDefault"), player.weaponSlot.transform) as GameObject;
		player.updateStats();
		StartCombat();
		*/
	}
	public void enemyAttacks(){
		StartCoroutine(enemyAttacksRoutine());
	}
	public void ResetPlayerDefence(){
		touchController.enemyTurn = true;
	}

	
	public void StartCombat(Loadout loadout, List<NodeEnemy> nodeEnemyList){
		enemyList = new List<Enemy> ();

		player.playerStats.weapon = WeaponCreator.CreateWeaponStatBlock (loadout.mainHand.subType, loadout.mainHand.ItemID);
		player.weapon = Instantiate (Resources.Load ("CombatResources/WeaponDefault"), player.weaponSlot.transform) as GameObject;
		player.updateStats ();

		for(int i = 0;i<nodeEnemyList.Count;i++){
			EnemyCreation(i, nodeEnemyList[i].subtype, nodeEnemyList[i].id);
		}
		menuController.targetedEnemy = enemyList[0];
		CreateHealthBars();
		menuController.PlayersTurn();
	}

	IEnumerator enemyAttacksRoutine(){
		yield return new WaitUntil(()=>menuController.proceed);
		touchController.enemyTurn = true;
		menuController.EnemyTurnTextFade();
		for (int i = 0; i < enemyList.Count; i++) {
			yield return new WaitForSeconds(3f);
			StartCoroutine(enemyList[i].Attack());
			yield return new WaitUntil(()=>enemyAttacked);
		}
		touchController.enemyTurn = false;
		enemyTurns--;
		if(enemyTurns<=0){
			enemyTurns = 0;
			menuController.PlayersTurn();
		}else{
			enemyAttacks();
		}
		
	}
	void EnemyCreation(int enemySpacing, string enemyType, int id){
		Vector3 enemyPos = enemyHost.transform.position;
		//Adds spacing
		GameObject enemyObject = Instantiate(Resources.Load("EnemyModels/BigGolem",typeof(GameObject)),new Vector3(enemyPos.x-enemySpacing, enemyPos.y, enemyPos.z+(enemySpacing*4)),enemyHost.transform.rotation) as GameObject;
		Enemy enemy = enemyObject.GetComponent<Enemy>();
		enemy.enemyName = NameDescContainer.GetName((NameType)System.Enum.Parse(typeof(NameType), enemyType), id);
		enemy.enemyStats = EnemyStatCreator.LoadStatBlockData(id, enemyType);
		
		enemyList.Add(enemy);
		enemy.combatController = this;
	}

	void CreateHealthBars(){
		int i = enemyList.Count-1;
		foreach (var item in enemyList)
		{
			menuController.GenerateHealthBars(i, item);
			updateEnemyStats(item.enemyStats.health, item.enemyStats.maxHealth, item);
			i--;
		}
	}

	public void EnemyDies(Enemy enemy){
		enemyList.Remove(enemy);
		deadEnemies.Add(enemy.ReturnDeadStats());
		Destroy(enemy,2f);
		if(enemyList.Count == 0){
			WinEncounter();
		}
	}

	void WinEncounter(){
		//Generate loot
	}

	public void HitPlayer(int damage,int elementDamage,Element element, bool area){
		menuController.messageToScreen(player.GetHit(damage, elementDamage, element, area));
	}

	public void HitEnemy(int damage, int elementDamage, Element element, int part){
		menuController.messageToScreen(menuController.targetedEnemy.GetHit(damage, elementDamage, element, part));
	}

	public void updateEnemyStats(float health, float maxhealth, Enemy enemy){
		menuController.updateEnemyHealth(health, maxhealth, health/maxhealth, enemy);
	}
	public void ActivatePartCanvas(Enemy enemy){
		int i = 0;
		if(!menuController.enemyPartCanvas.activeInHierarchy){
			menuController.enemyPartCanvas.SetActive(true);
			i = 0;
			foreach (var item in enemy.enemyStats.partList){
				menuController.enemyPartCanvasButtons[i].SetActive(true);
				if(!enemy.enemyStats.partList[i].broken){
					menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text>().text = enemy.enemyStats.partList[i].name +"\n"+enemy.enemyStats.partList[i].percentageHit+"%";
				}else{
					menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text>().text = enemy.enemyStats.partList[i].name +"\n"+enemy.enemyStats.partList[i].percentageHit+"%"+"\nBroken";
					menuController.enemyPartCanvasButtons[i].GetComponent<Image>().color = Color.red;
				}
				i++;
			}
		}else{
			foreach (var item in menuController.enemyPartCanvasButtons)
			{
				item.SetActive(false);
			}
			menuController.enemyPartCanvas.SetActive(false);
		}
	}
}
