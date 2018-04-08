using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndyingObject : MonoBehaviour {

	// container of all player data
	// Travels through scenes
	// add more stuff as it comes up


	// TODO: Add visual map generation method?

	// PUBLIC only for testing purposes
	public LoadoutsContainer loadoutList;
	public WorldStatsContainer playerWorldStats;

	double locLatitude;
	double locLongitude;

	void Start () {
		DontDestroyOnLoad (this);
		DataManager.ReadDataString ("nonexistent"); // TODO: Loading screen data
		NodeInteraction.InitializeUndyingObject(this);
		Inventory.Initialize ();
		// TODO: Potentially load player data
		//ELSE:
		playerWorldStats = new WorldStatsContainer();
		int loadoutCount = 10;

		loadoutList = new LoadoutsContainer(loadoutCount);

        // Get location data here
        //StartCoroutine (UpdateLocationData(10)); // Enable this when testing
        //StartCoroutine (ToTheWorld ());
        StartCrafting();
	}

	private IEnumerator ToTheWorld(){
        yield return SceneManager.LoadSceneAsync ("TheWorld");
    }

	//TODO: CHOOSE LOADOUT
	public void CombatPrompt (List<NodeEnemy> enemyList){
		StartCombat (0, enemyList);
	}

	private void StartCombat(int loadoutIndex, List<NodeEnemy> enemyList){
		StartCoroutine (StartCombatIenum (loadoutList.GetLoadout(loadoutIndex), enemyList));
	}

	private IEnumerator StartCombatIenum(Loadout loadout, List<NodeEnemy> enemyList){
		yield return SceneManager.LoadSceneAsync ("BattleScene");
		CombatController comCon = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		// TODO: Add health and stamina to startCombat??
		List<WorldBuffAbstraction> buffList = playerWorldStats.GetWorldBuffs();
		// TODO: Add buffs to StartCombat
		comCon.StartCombat (loadout, enemyList);
	}

	public void UpdateWorldPosition(double latitude, double longitude){
		this.locLatitude = latitude;
		this.locLongitude = longitude;
	}

	public void EndCombat(List<List<RecipeMaterial>> dropListList, float health, float stamina){
		foreach (List<RecipeMaterial> dropList in dropListList) {
			foreach (RecipeMaterial recMat in dropList) {
				Inventory.InsertRecipeMaterial (recMat);
			}
		}
		StartCoroutine (EndCombatIenum ());
	}

	private IEnumerator EndCombatIenum(){
		yield return SceneManager.LoadSceneAsync ("TheWorld");
	}

	public void GetGatherinNode(int nodeIndex, int dropAmount){
		DropData nodeDropData = DropDataCreator.CreateDropData (DropperType.Gather, nodeIndex);
		List<RecipeMaterial> dropList = DropDataCreator.CalculateDrops (nodeDropData, dropAmount, null);
		foreach (RecipeMaterial recMat in dropList) {
			Inventory.InsertRecipeMaterial (recMat);
		}
	}

	public void StartCrafting(){
		StartCoroutine (StartCraftingIenum ());
	}

	private IEnumerator StartCraftingIenum(){
		yield return SceneManager.LoadSceneAsync ("Crafting");
		Crafting craft = GameObject.FindGameObjectWithTag ("CraftingController").GetComponent<Crafting> ();

	}

	public void EndCrafting(){
		StartCoroutine (EndCraftingIenum ());
	}

	private IEnumerator EndCraftingIenum(){
		yield return SceneManager.LoadSceneAsync ("TheWorld");
	}



	private IEnumerator UpdateLocationData(float seconds){
		while (true){
			yield return new WaitForSeconds (seconds);
			// TODO: Get latitude and longitude from phone
			locLatitude = 0;
			locLongitude = 0;
		}
	}
}
