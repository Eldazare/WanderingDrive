using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {


	CombatController combatController;
	Enemy target;
	PlayerCombatScript playerCombatScript;


	public GameObject DefaultButtons;
	public GameObject AbilityButtons;
	public GameObject ItemMenu;

	// Doesn't actually do anything yet
	// Uses the linked list concept, to switch targets
	public void NextTarget () {
		target = target.Next;
	}
	public void PreviousTarget() {
		target = target.Previous;
	}

	// Buttons for button menus. Really basic at the moment.
	// The buttons are set as children of empty GameObjects and we're basically just tossing them from out of view back in view.
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


	// Buttons.
	public void Attack () {
		combatController.Attack (target);
	}
	public void Ability1() {
		combatController.Ability (target, 1);
	}
	public void Ability2() {
		combatController.Ability (target, 2);
	}
	public void Ability3() {
		combatController.Ability (target, 3);
	}
	public void Ability4() {
		combatController.Ability (target, 4);
	}
	public void Focus() {
		combatController.PlayerFocus ();
	}
	public void Overload () {
		combatController.PlayerOverload ();
	}


}
