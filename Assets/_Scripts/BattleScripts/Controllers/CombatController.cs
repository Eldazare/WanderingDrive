﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CombatController : MonoBehaviour {

	public List<Enemy> enemyList;
	public PlayerCombatScript player; //Drag from Hierarchy
	public PlayerStats playerStats;
	public GameObject enemyHost; //Drag from Hierarchy
	public MenuController menuController; //Drag from Hierarchy
	public TouchControlScript touchController; //Drag from Hierarchy
	public CameraController cameraScript; //Drag from Hierarchy
	public bool enemyAttacked;
	List<List<RecipeMaterial>> encounterDrops = new List<List<RecipeMaterial>>();
	public int enemyTurns;
	public bool playerDead;
	public static float armorAlgorithmModifier = 50; // = N | [% = N / (N+Armor)]   
	public void enemyAttacks(){
		StartCoroutine(enemyAttacksRoutine());
	}
	public void ResetPlayerDefence(){
		touchController.enemyTurn = true;
	}
	
	public void StartCombat(Loadout loadout, List<NodeEnemy> nodeEnemyList){
		playerStats = player.playerStats;
		enemyList = new List<Enemy> ();
		//if(loadout.mainHand != null){
			playerStats.mainHand = WeaponCreator.CreateWeaponStatBlock ((WeaponType)System.Enum.Parse(typeof(WeaponType),loadout.mainHand.subType), loadout.mainHand.itemID);
			playerStats.dodgeModifier += playerStats.mainHand.dodgeModifier;
			playerStats.blockModifier += playerStats.mainHand.blockModifier;
			playerStats.magicArmor += playerStats.mainHand.magicArmorBonus;
		//}
		if(loadout.offHand != null){
			playerStats.offHand = WeaponCreator.CreateWeaponStatBlock ((WeaponType)System.Enum.Parse(typeof(WeaponType),loadout.offHand.subType), loadout.offHand.itemID);
			playerStats.dodgeModifier += playerStats.offHand.dodgeModifier;
			playerStats.blockModifier += playerStats.offHand.blockModifier;
			playerStats.magicArmor += playerStats.offHand.magicArmorBonus;
			playerStats.physicalArmor += playerStats.offHand.armorBonus;
		}

		GenerateArmors(loadout);
		player.weapon = Instantiate (Resources.Load ("CombatResources/WeaponDefault"), player.weaponSlot.transform) as GameObject;
		player.updateStats ();

		for(int i = 0;i<nodeEnemyList.Count;i++){
			EnemyCreation(i, nodeEnemyList[i].subtype, nodeEnemyList[i].id);
		}
		
		//Temporary ability generation
		Ability spell = new FireBall(player);
		spell.staminaCost = 10;
		spell.damage = 0;
		spell.elementDamage = 20;
		spell.element = Element.Fire;
		playerStats.abilities.Add(spell);

		/* _Buff buff = new ArmorBuff(50,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new Blind(50,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new Confusion(1);
		buff.player = player;
		player.playerBuffs.Add(buff); 
		buff = new Paralyze(1);
		buff.player = player;
		player.playerBuffs.Add(buff); 
		buff = new Freeze(1);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new DamageMultiplier(0.8f,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new DamageOverTime(10,10,0,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new ElementDamageMultiplier(0.8f,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new FlatDamage(10,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new FlatElementDamage(10,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new HealthRegen(10, 3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new StaminaRegen(10, 3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new ElementDamageMultiplier(0.8f,3);
		buff.player = player;
		player.playerBuffs.Add(buff);
		buff = new Stun(2);
		buff.player = player;
		player.playerBuffs.Add(buff); */

		/* buff = new Stun(2);
		buff.enemy = enemyList[1];
		enemyList[1].enemyBuffList.Add(buff);

		buff = new DamageOverTime(10, 10, 0, 3);
		buff.enemy = enemyList[2];
		enemyList[2].enemyBuffList.Add(buff); */

		menuController.targetedEnemy = enemyList[0];
		player.transform.LookAt(enemyList[0].transform.position);
		CreateHealthBars();
		menuController.PlayersTurn();
	}

	void GenerateArmors(Loadout loadout){
		float speed = 0;
		int j = 0;

		//Armor generation
		if(loadout.wornArmor[0] != null){
			foreach (var item in loadout.wornArmor)	
			{
				Armor armor = ArmorCreator.CreateArmor((ArmorType)System.Enum.Parse(typeof(ArmorType), item.subType), item.itemID);
				playerStats.physicalArmor += armor.defense;
				playerStats.magicArmor += armor.magicDefense;

				int i = 0;
				foreach (var item1 in armor.elementResists)
				{
					playerStats.elementWeakness[i] += item1;
					i++;
				}
				speed += armor.speed;
				j++;
			}
			playerStats.speed = speed/j;
		}

		//Accessory generation
		if(loadout.wornAccessories[0] != null){
			foreach (var item in loadout.wornAccessories){
				Accessory acc = ArmorCreator.CreateAccessory(item.itemID);
				int i = 0;
				playerStats.damage += acc.damage;
				playerStats.elementDamage += acc.elementDamage;
				playerStats.magicArmor += acc.magicDefense;
				foreach (var item1 in acc.elementResists)
				{
					playerStats.elementWeakness[i] += item1;
					i++;
				}
			}
		}
	}

	IEnumerator enemyAttacksRoutine(){
		touchController.enemyTurn = true;
		menuController.EnemyTurnTextFade();
		foreach (var item in enemyList){
			if(!playerDead){
				if(item != null){
						item.ApplyEnemyBuffs();
					if(!item.stunned){
						yield return new WaitForSeconds(3f);
						StartCoroutine(item.Attack());
						yield return new WaitUntil(()=>enemyAttacked);
					}else{
						yield return new WaitForSeconds(1f);
					}
				}
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
		GameObject enemyObject = Instantiate(Resources.Load("EnemyModels/BigGolem",typeof(GameObject)),new Vector3(enemyPos.x-(enemySpacing*2), enemyPos.y, enemyPos.z+(enemySpacing*4)),enemyHost.transform.rotation) as GameObject;
		Enemy enemy = enemyObject.GetComponent<Enemy>();
		enemyObject.transform.LookAt(player.transform.position);
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
			
		DropData drop = DropDataCreator.CreateDropData(DropDataCreator.parseDroppertype(enemy.enemyStats.subtype),enemy.enemyStats.ID);
		List<RecipeMaterial> list = DropDataCreator.CalculateDrops(drop ,2 ,enemy.enemyStats.partList);

		encounterDrops.Add(list);

		enemyList[enemyList.IndexOf(enemy)] = null;
		Destroy(enemy.gameObject, 1f);
		foreach (var item in enemyList)
		{
			if(item != null){
				item.updateStats();
			}
		}
		foreach (var item in enemyList)
		{
			if(item != null){
				menuController.targetedEnemy = item;
				break;
			}
		}
		foreach (var item in encounterDrops)
		{
			foreach (var item1 in item)
			{
				Debug.Log(item1.itemId);
			}
		}
		if(enemyList.Count == 0){
			WinEncounter();
		}
	}

	void WinEncounter(){
		//Generate loot, health, stamina and rest of the returned values to a object and give it back to map scene upon transitioning there
	}
	public void LoseEncounter(){
		playerDead = true;
	}
	public void HitPlayer(float damage, float elementDamage, Element element, bool area, int damageType){
		menuController.messageToScreen(player.GetHit(damage, elementDamage, element, area, damageType));
	}
	public void HitEnemy(float damage, float elementDamage, Element element, int part, float accuracy, WeaknessType weaknessType){
		menuController.messageToScreen(menuController.targetedEnemy.GetHit(damage, elementDamage, element, part, accuracy, weaknessType));
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
					menuController.enemyPartCanvasButtons[i].GetComponent<Image>().color = Color.white;
				}else{
					menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text>().text = enemy.enemyStats.partList[i].name +"\n"+enemy.enemyStats.partList[i].percentageHit+"%"+"\nBroken";
					menuController.enemyPartCanvasButtons[i].GetComponent<Image>().color = Color.red;
				}
				i++;
			}
			if(menuController.selectedPart < 0){
				menuController.SelectEnemyPart(0);
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
