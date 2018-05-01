using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

	public List<Enemy> enemyList;
	public PlayerCombatScript player; //Drag from Hierarchy
	public PlayerStats playerStats;
	public GameObject enemyHost; //Drag from Hierarchy
	public MenuController menuController; //Drag from Hierarchy
	public TouchControlScript touchController; //Drag from Hierarchy
	public CameraController cameraScript; //Drag from Hierarchy
	public bool enemyAttacked;

	List<DropData> dropdata = new List<DropData> ();
	List<List<EnemyPart>> enemypartListList = new List<List<EnemyPart>> ();
	List<List<RecipeMaterial>> encounterDrops = new List<List<RecipeMaterial>> ();
	public int enemyTurns;
	public bool playerDead;
	bool proceed;
	public bool proceedToEnemyAttacks;
	public static float armorAlgorithmModifier = 50; // = N | [% = N / (N+Armor)]   

	//For Debugging purposes
	void Update() {
		if(Input.GetKeyDown(KeyCode.D)){
			player.Dodge(0);
		}
		if(Input.GetKeyDown(KeyCode.B)){
			player.Block();
		}
	}
	public void EnemyAttacks () {
		StartCoroutine (EnemyAttacksRoutine ());
	}
	public void ResetPlayerDefence () {
		player.defended = false;
	}

	public void StartCombat (Loadout loadout, List<NodeEnemy> nodeEnemyList) {
		playerStats = player.playerStats;
		armorAlgorithmModifier = DataManager.ReadDataFloat ("Armor_Correspondance");
		enemyList = new List<Enemy> ();
		if (loadout.mainHand != null) {
			playerStats.mainHand = WeaponCreator.CreateWeaponStatBlock ((WeaponType) System.Enum.Parse (typeof (WeaponType), loadout.mainHand.subType), loadout.mainHand.itemID);
			playerStats.dodgeModifier += playerStats.mainHand.dodgeModifier;
			playerStats.blockModifier += playerStats.mainHand.blockModifier;
			playerStats.magicArmor += playerStats.mainHand.magicArmorBonus;
			playerStats.physicalArmor += playerStats.mainHand.armorBonus;
		}
		if (loadout.offHand != null) {
			playerStats.offHand = WeaponCreator.CreateWeaponStatBlock ((WeaponType) System.Enum.Parse (typeof (WeaponType), loadout.offHand.subType), loadout.offHand.itemID);
			playerStats.dodgeModifier += playerStats.offHand.dodgeModifier;
			playerStats.blockModifier += playerStats.offHand.blockModifier;
			playerStats.magicArmor += playerStats.offHand.magicArmorBonus;
			playerStats.physicalArmor += playerStats.offHand.armorBonus;
		}

		GenerateArmors (loadout);
		player.weapon = Instantiate (Resources.Load ("CombatResources/WeaponDefault"), player.weaponSlot.transform) as GameObject;
		player.UpdateStats ();

		for (int i = 0; i < nodeEnemyList.Count; i++) {
			EnemyCreation (i, nodeEnemyList[i].subtype, nodeEnemyList[i].id);
		} 

		//EnemyCreation (0, nodeEnemyList[0].subtype, nodeEnemyList[0].id);
		//Temporary ability generation

		Ability spell = new FireBall (player);
		spell.staminaCost = 10;
		spell.damage = 0;
		spell.elementDamage = 20;
		spell.element = Element.Fire;
		playerStats.abilities.Add (spell);

		//Temporary buff generation
		/* _Buff buff = new DamageOverTime(10, Element.Fire,1);
		player.playerBuffs.Add(buff);
		buff.player = player;
		buff = new ArmorBuff(10);
		player.playerBuffs.Add(buff);
		buff.player = player;
		buff = new ArmorBuff(10,2);
		player.playerBuffs.Add(buff);
		buff.player = player; */

		menuController.targetedEnemy = enemyList[0];
		player.transform.LookAt (enemyList[0].transform.position);
		CreateHealthBars ();
		menuController.PlayersTurn ();
		foreach (var item in player.playerBuffs) {
			if (item != null) {
				Debug.Log (item.GetType ().Name);
			}
		}
	}

	void GenerateArmors (Loadout loadout) {
		float speed = 0;
		float j = 0;

		//Armor generation
		foreach (var item in loadout.wornArmor) {
			if (item != null) {
				Armor armor = ArmorCreator.CreateArmor ((ArmorType) System.Enum.Parse (typeof (ArmorType), item.subType), item.itemID);
				playerStats.physicalArmor += armor.defense;
				playerStats.magicArmor += armor.magicDefense;

				int i = 0;
				foreach (var item1 in armor.elementResists) {
					playerStats.elementWeakness[i] += item1;
					i++;
				}
				speed += armor.speed;
				j++;
			} else {
				speed++;
				j++;
			}
		}
		playerStats.speed = speed / j;

		//Accessory generation
		if (loadout.wornAccessories != null) {
			foreach (var item in loadout.wornAccessories) {
				if (item != null) {
					Accessory acc = ArmorCreator.CreateAccessory (item.itemID);
					int i = 0;
					playerStats.damage += acc.damage;
					playerStats.elementDamage += acc.elementDamage;
					playerStats.magicArmor += acc.magicDefense;
					foreach (var item1 in acc.elementResists) {
						playerStats.elementWeakness [i] += item1;
						i++;
					}
				}
			}
		}
	}

	IEnumerator EnemyAttacksRoutine () {
		if (!playerDead) {
			menuController.targetHealthBar.SetActive (false);
			touchController.enemyTurn = true;
			menuController.EnemyTurnTextFade ();
			foreach(var item in enemyList){
				if(item != null){
					item.proceed = false;
					item.ApplyEnemyBuffs ();
					if(!item.stunned){
						yield return item.Attack ();
						yield return new WaitUntil (() => enemyAttacked);
						enemyAttacked = false;
					}else{
						GameObject popup = Instantiate (Resources.Load ("CombatResources/DamagePopUp"), new Vector3 (item.transform.position.x, item.transform.position.y + 3, item.transform.position.z) - item.transform.right, Quaternion.identity) as GameObject;
						popup.GetComponent<TextMesh> ().text = "Stunned";
						yield return new WaitForSeconds (1f);
					}
				}
				ResetPlayerDefence();
			}
			enemyTurns--;
			if (enemyTurns <= 0) {
				enemyTurns = 0;
				touchController.enemyTurn = false;
				menuController.PlayersTurn ();
			} else {
				EnemyAttacks ();
			}
		}
	}
	void EnemyCreation (int enemySpacing, string enemyType, int id) {
		string enemyName =  NameDescContainer.GetName ((NameType) System.Enum.Parse (typeof (NameType), enemyType), id);
		Debug.Log(enemyName);
		Vector3 enemyPos = enemyHost.transform.position;
		//Adds spacing
		var i = ((enemySpacing * 1.0 + 6.7) / 14);
             
        //angle in radians, full circle has a radian angle of 2PI, so we find the radian angle between 
        //2 of our points by dividing 2PI by the number of points
        var angle = (float)(i * Mathf.PI * 2);
             
        var radiusX = 15;
        var radiusZ = 15;
             
        var x = (float)( Mathf.Sin (angle) * radiusX);
        var z = (float)( Mathf.Cos (angle) * radiusZ);

		GameObject enemyObject = Instantiate (Resources.Load ("EnemyModels/"+enemyName, typeof (GameObject)), new Vector3 (enemyPos.x - z, enemyPos.y, enemyPos.z - x), enemyHost.transform.rotation) as GameObject;
		
		Enemy enemy = enemyObject.GetComponent<Enemy> ();
		enemyObject.transform.LookAt (player.transform.position);
		enemy.enemyName = enemyName;
		enemy.enemyStats = EnemyStatCreator.LoadStatBlockData (id, enemyType);

		enemyList.Add (enemy);
		enemy.combatController = this;
	}

	void CreateHealthBars () {
		int i = enemyList.Count - 1;
		foreach (var item in enemyList) {
			menuController.GenerateHealthBars (i, item);
			updateEnemyStats (item.enemyStats.health, item.enemyStats.maxHealth, item);
			i--;
		}
	}
		
	public void EnemyDies (Enemy enemy) {
		DropData drop = DropDataCreator.CreateDropData (DropDataCreator.parseDroppertype (enemy.enemyStats.subtype), enemy.enemyStats.ID);
		dropdata.Add (drop);
		//enemypartListList.Add (enemy.enemyStats.partList);
		encounterDrops.Add(DropDataCreator.CalculateDrops(drop, DropDataCreator.DefaultNormalDropAmount(), enemy.enemyStats.partList));

		enemyList[enemyList.IndexOf (enemy)] = null;
		Destroy (enemy.gameObject, 1f);
		foreach (var item in enemyList) {
			if (item != null) {
				item.updateStats ();
			}
		}
		foreach (var item in enemyList) {
			if (item != null) {
				menuController.targetedEnemy = item;
				break;
			}
		}
		bool allDead = false;
		foreach (var item in enemyList) {
			if (item != null) {
				allDead = false;
				break;
			} else {
				allDead = true;
			}
		}
		if (allDead) {
			StartCoroutine (WinCombatRoutine ());
		}
	}
	public void LoseEncounter () {
		StartCoroutine (LoseEncounterRoutine ());
	}
	IEnumerator LoseEncounterRoutine () {
		proceed = false;
		playerDead = true;
		menuController.loseScreen.SetActive (true);
		GenerateAliveParts ();
		yield return new WaitForSeconds (2f);
		yield return new WaitUntil (() => proceed);
		BattleEndCombat (player.playerStats.maxHealth * 0.5f, 0, encounterDrops);
	}
	public void RunAway () {
		StartCoroutine (RunAwayRoutine ());
	}
	IEnumerator RunAwayRoutine () {
		proceed = false;
		GenerateAliveParts ();
		menuController.runAwayScreen.SetActive (true);
		yield return new WaitForSeconds (2f);
		yield return new WaitUntil (() => proceed);
		BattleEndCombat (player.playerStats.health, playerStats.stamina, encounterDrops);
	}

	void GenerateAliveParts () {
		foreach (var enemy in enemyList) {
			if (enemy != null) {
				DropData drop = DropDataCreator.CreateDropData (DropDataCreator.parseDroppertype (enemy.enemyStats.subtype), enemy.enemyStats.ID);
				List<RecipeMaterial> newList = DropDataCreator.CalculateDrops (drop, 0, enemy.enemyStats.partList);
				if (newList.Count > 0) {
					encounterDrops.Add (newList);
				}
			}
		}
		proceed = true;
	}
	IEnumerator WinCombatRoutine () {
		menuController.victoryScreen.SetActive (true);
		/*
		foreach (var item in dropdata) {
			List<RecipeMaterial> list = DropDataCreator.CalculateDrops (item, 2, enemypartListList[dropdata.IndexOf (item)]);
			encounterDrops.Add (list);
		}
		*/
		yield return new WaitForSeconds (2f);
		BattleEndCombat (player.playerStats.health, playerStats.stamina, encounterDrops);
		yield return new WaitForSeconds (1f); // <- no purpose
	}
	public void BattleEndCombat (float health, float stamina, List<List<RecipeMaterial>> loots) {
		UndyingObject worldObj = GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ();
		worldObj.playerWorldStats.health = health;
		worldObj.playerWorldStats.stamina = stamina;
		worldObj.EndCombat (loots);
	}
	public void HitPlayer (float damage, float elementDamage, Element element, bool area, int damageType) {
		menuController.MessageToScreen (player.GetHit (damage, elementDamage, element, area, damageType));
	}
	public void HitEnemy (float damage, float elementDamage, Element element, int part, float accuracy, WeaknessType weaknessType) {
		menuController.MessageToScreen (menuController.targetedEnemy.GetHit (damage, elementDamage, element, part, accuracy, weaknessType));
	}
	public void updateEnemyStats (float health, float maxhealth, Enemy enemy) {
		menuController.UpdateEnemyHealth (health, maxhealth, health / maxhealth, enemy);
	}
	public void ActivatePartCanvas (Enemy enemy) {
		int i = 0;
		if (!menuController.enemyPartCanvas.activeInHierarchy) {
			menuController.enemyPartCanvas.SetActive (true);
			i = 0;
			foreach (var item in enemy.enemyStats.partList) {
				menuController.enemyPartCanvasButtons[i].SetActive (true);
				if (!enemy.enemyStats.partList[i].broken) {
					menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text> ().text = enemy.enemyStats.partList[i].name + "\n" + enemy.enemyStats.partList[i].percentageHit + "%";
					menuController.enemyPartCanvasButtons[i].GetComponent<Image> ().color = Color.white;
				} else {
					menuController.enemyPartCanvasButtons[i].GetComponentInChildren<Text> ().text = enemy.enemyStats.partList[i].name + "\n" + enemy.enemyStats.partList[i].percentageHit + "%" + "\nBroken";
					menuController.enemyPartCanvasButtons[i].GetComponent<Image> ().color = Color.red;
				}
				i++;
			}
			if (menuController.selectedPart < 0) {
				menuController.SelectEnemyPart (0);
				menuController.targetHealthBar.SetActive (true);
				menuController.targetHealthBar.GetComponent<TargetEnemyHealthBar>().UpdateBar(0);
			}else{
				menuController.targetHealthBar.SetActive (true);
				menuController.targetHealthBar.GetComponent<TargetEnemyHealthBar>().UpdateBar(menuController.selectedPart);
			}
			
			
		} else {
			foreach (var item in menuController.enemyPartCanvasButtons) {
				item.SetActive (false);
			}
			menuController.enemyPartCanvas.SetActive (false);
		}
	}
}