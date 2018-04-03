using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndyingObject : MonoBehaviour {

	// container of all player data
	// add more stuff as it comes up
	float health;
	float stamina;
	Inventory inventory;
	List<Loadout> loadoutList;

	double locLatitude;
	double locLongitude;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}

	public void StartCombat(int loadoutIndex, List<NodeEnemy> enemyList){
		StartCoroutine (StartCombatIenum (loadoutList [loadoutIndex], enemyList));
	}

	private IEnumerator StartCombatIenum(Loadout loadout, List<NodeEnemy> enemyList){
		yield return SceneManager.LoadSceneAsync ("BattleScene");
		CombatController comCon = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		// TODO: Add giving health and stamina to combat controller
		comCon.StartCombat (loadout, enemyList);
	}

	public void UpdateWorldPosition(double latitude, double longitude){
		this.locLatitude = latitude;
		this.locLongitude = longitude;
	}

	public void EndCombat(List<List<RecipeMaterial>> dropListList){
		foreach (List<RecipeMaterial> dropList in dropListList) {
			foreach (RecipeMaterial recMat in dropList) {
				// TODO: Inventory: put item (RecipeMaterial)
			}
		}
		StartCoroutine (EndCombatIenum ());
	}

	private IEnumerator EndCombatIenum(){
		yield return SceneManager.LoadSceneAsync ("TheWorld");
	}

	public void GetGatherinNode(int nodeIndex, int dropAmount){
		DropData nodeDropData = DropDataCreator.CreateDropData (DropperType.gather, nodeIndex);
		List<RecipeMaterial> dropList = DropDataCreator.CalculateDrops (nodeDropData, dropAmount, null);
		foreach (RecipeMaterial recMat in dropList) {
			// TODO: Inventory: put item
		}
	}

	public void StartCrafting(){
		StartCoroutine (StartCraftingIenum ());
	}

	private IEnumerator StartCraftingIenum(){
		yield return SceneManager.LoadSceneAsync ("Crafting");
		// TODO: Find crafting class and reference the inventory for it.
	}

	public void EndCrafting(){
		StartCoroutine (EndCraftingIenum ());
	}

	private IEnumerable EndCraftingIenum(){
		yield return SceneManager.LoadSceneAsync ("TheWorld");
	}
}
