using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CombatController : MonoBehaviour {

	public List<Enemy> enemyList;
	public PlayerCombatScript player;
	public PlayerStats playerStats;
	public GameObject enemyHost;
	public MenuController menuController;
	public TouchControlScript touchController;
	public CameraController cameraScript;
	public bool enemyAttacked;
	List<RecipeMaterial> encounterDrops;
	public int enemyTurns;
	bool playerDead;
	public void enemyAttacks(){
		StartCoroutine(enemyAttacksRoutine());
	}
	public void ResetPlayerDefence(){
		touchController.enemyTurn = true;
	}

	
	public void StartCombat(Loadout loadout, List<NodeEnemy> nodeEnemyList){
		enemyList = new List<Enemy> ();
		
		player.playerStats.mainHand = WeaponCreator.CreateWeaponStatBlock (loadout.mainHand.subType, loadout.mainHand.ItemID);
		if(loadout.offHand != null){
			player.playerStats.offHand = WeaponCreator.CreateWeaponStatBlock (loadout.offHand.subType, loadout.offHand.ItemID);
		}

		GenerateArmors(loadout);
		player.weapon = Instantiate (Resources.Load ("CombatResources/WeaponDefault"), player.weaponSlot.transform) as GameObject;
		player.updateStats ();

		for(int i = 0;i<nodeEnemyList.Count;i++){
			EnemyCreation(i, nodeEnemyList[i].subtype, nodeEnemyList[i].id);
		}
		menuController.targetedEnemy = enemyList[0];
		CreateHealthBars();
		menuController.PlayersTurn();
	}

	void GenerateArmors(Loadout loadout){
		float armorAmount = 0;
		float speed = 0;
		int j = 0;

		//Armor generation
		if(loadout.wornArmor[0] != null){
			foreach (var item in loadout.wornArmor)	
			{
				Armor armor = ArmorCreator.CreateArmor((ArmorTypes)System.Enum.Parse(typeof(ArmorTypes), item.subType), item.itemID);
				armorAmount += armor.defense;

				int i = 0;
				foreach (var item1 in armor.elementResists)
				{
					playerStats.elementalWeakness[i] += item1;// + (int)(armor.magicDefense*100);
					i++;
				}
				speed += armor.speed;
				j++;
			}
			playerStats.speed = speed/j;
		}
		

		

		//Accessory generation
		if(loadout.wornAccessories[0] != null){
			foreach (var item in loadout.wornAccessories)
			{
				Accessory acc = ArmorCreator.CreateAccessory(item.itemID);
				int i = 0;
				foreach (var item1 in acc.elementResists)
				{
					playerStats.elementalWeakness[i] += item1;// + (int)(acc.magicDefense*100);
					i++;
				}
			}
		}
	}

	IEnumerator enemyAttacksRoutine(){
		
			yield return new WaitUntil(()=>menuController.proceed);
			touchController.enemyTurn = true;
			menuController.EnemyTurnTextFade();
			foreach (var item in enemyList)
			{
				if(!playerDead){
					if(item != null){
						yield return new WaitForSeconds(3f);
						StartCoroutine(item.Attack());
						yield return new WaitUntil(()=>enemyAttacked);
					}
				}else{
					break;
				}
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

			
		DropData drop = DropDataCreator.CreateDropData(DropDataCreator.parseDroppertype(enemy.enemyStats.subtype),enemy.enemyID);
		List<RecipeMaterial> list = DropDataCreator.CalculateDrops(drop ,1 ,enemy.enemyStats.partList);

		foreach (var item in list)
		{
			//encounterDrops.Add(item);
		}
		//menuController.enemyHealthBars.Remove(menuController.enemyHealthBars[menuController.enemyHealthBars.Count]);
		Destroy(menuController.enemyHealthBars[menuController.enemyHealthBars.Count-1]);
		enemyList[enemyList.IndexOf(enemy)] = null;
		Destroy(enemy.gameObject,2f);
		foreach (var item in enemyList)
		{
			if(item != null){
				item.updateStats();
			}
			
		}
		if(enemyList.Count == 0){
			WinEncounter();
		}
	}

	void WinEncounter(){
		
	}
	public void LoseEncounter(){
		playerDead = true;
	}

	public void HitPlayer(float damage, float elementDamage, Element element, bool area){
		menuController.messageToScreen(player.GetHit(damage, elementDamage, element, area));
	}

	public void HitEnemy(float damage, float elementDamage, Element element, int part, float damageMod){
		menuController.messageToScreen(menuController.targetedEnemy.GetHit(damage, elementDamage, element, part, damageMod));
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
