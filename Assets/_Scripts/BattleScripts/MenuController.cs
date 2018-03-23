using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {


	CombatController combatController;
	public Enemy targetedEnemy;
	PlayerCombatScript playerCombatScript;


	public GameObject DefaultButtons;
	public GameObject AbilityButtons;
	public GameObject ItemMenu;
	public PlayerCombatScript player;
	bool playersTurn;
	int enemyTargetNumber;

	//Doesn't actually do anything yet
	//Uses the linked list concept, to switch targets
	public void NextTarget () {
		enemyTargetNumber++;
		if(enemyTargetNumber > combatController.enemyList.Count){
			enemyTargetNumber = 0;
		}
		targetedEnemy = combatController.enemyList[enemyTargetNumber];
	}
	public void PreviousTarget() {
		enemyTargetNumber--;
		if(enemyTargetNumber < 0){
			enemyTargetNumber = combatController.enemyList.Count;
		}
		targetedEnemy = combatController.enemyList[enemyTargetNumber];
	}
 	//Buttons for button menus. Really basic at the moment.
	//The buttons are set as children of empty GameObjects and we're basically just tossing them from out of view back in view.
	public void AbilitiesMenu() {
		DefaultButtons.SetActive (false);
		AbilityButtons.SetActive (true);
	}
	public void ItemsMenu () {
		ItemMenu.SetActive (true);
		DefaultButtons.SetActive (false);
	}
	public void Back () {
		DefaultButtons.SetActive (true);
		ItemMenu.SetActive (false);
		AbilityButtons.SetActive (false);
	}
	public void PlayersTurn(){
		DefaultButtons.SetActive(true);
	}

	// Buttons.
	public void Attack () {
		DefaultButtons.SetActive(false);
		player.Attack ();
	}
	public void Ability(int slot) {
		player.Ability (slot);
	}

	public void Consumable(int slot){
		player.Consumable(slot);
	}
	public void Focus() {
		player.PlayerFocus ();
	}
	public void Overload () {
		player.PlayerOverload ();
	}


}
