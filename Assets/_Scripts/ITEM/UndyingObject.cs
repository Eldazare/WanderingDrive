using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Mapbox.Utils;

public class UndyingObject : MonoBehaviour {

	// container of all player data
	// Travels through scenes
	// add more stuff as it comes up


	private bool doTutorialBox = true;

	// PUBLIC only for testing purposes
	public LoadoutsContainer loadoutList;
	public WorldStatsContainer playerWorldStats;

	double locLatitude;
	double locLongitude;
	int chosenLoadout;
	List<NodeEnemy> storedNode;

	public GameObject interactedNode;
	//public List<Vector2d> storedMarkers;
	public Vector2d storedPlayerPosition;
	public List<MarkerDataContainer> storedMarkers;


	void Start () {
		DontDestroyOnLoad (this);
		DataManager.ReadDataString ("nonexistent"); // TODO: Loading screen -data (show loading bar or something)
		NodeInteraction.InitializeUndyingObject(this);
		Inventory.Initialize ();
		NodeSpawner.InitializeWeights ();
		// TODO: Potentially load player data
		//ELSE:
		playerWorldStats = new WorldStatsContainer();
		int loadoutCount = 10;

		loadoutList = new LoadoutsContainer(loadoutCount);

		storedMarkers = new List<MarkerDataContainer> ();
		//storedPlayerPosition = new Vector2d (0, 0);

        // Get location data here
        //StartCoroutine (UpdateLocationData(10)); // Enable this when testing location
		FillInventoryForTest (); // TESTING ONLY
        StartCoroutine (ToTheWorld ());
	}

	private void FillInventoryForTest(){
		Inventory.PutItem (ItemType.Wep, ItemSubType.Sword, 0, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Accessory, 0, 1);
		InsertBasicArmorToInventory ();
		Loadout loadout = new Loadout (1);
		loadout.AddMainHand (new InventoryWeapon (0, "Sword"));
		for (int i = 0; i < 50; i++) {
			Inventory.PutItem (ItemType.Mat, ItemSubType.Mat, i, 50);
		}
		//loadoutList.InsertLoadout (loadout, 0);
	}

	private void InsertBasicArmorToInventory(){
		Inventory.PutItem (ItemType.Arm, ItemSubType.Helm, 0, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Chest, 0, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Arms, 0, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Legs, 0, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Boots, 0, 1);
		Loadout loadout = new Loadout (1);
		loadout.AddArmor (new InventoryArmor (0, "Helm"));
		loadout.AddArmor (new InventoryArmor (0, "Chest"));
		loadout.AddArmor (new InventoryArmor (0, "Arms"));
		loadout.AddArmor (new InventoryArmor (0, "Legs"));
		loadout.AddArmor (new InventoryArmor (0, "Boots"));
		loadout.AddMainHand (new InventoryWeapon (0, "Sword"));
		loadout.AddCombatConsumable (0, (int)ConsumableType.ConsumableUniversal, 0);
		loadout.AddCombatConsumable (1, (int)ConsumableType.ConsumableCombat, 0);
		loadoutList.InsertLoadout (loadout, 9);
	}

	private IEnumerator ToTheWorld(){
		//yield return new WaitForSeconds (3.0f); // DEBUG & VIDEO
        yield return SceneManager.LoadSceneAsync ("TheWorld");
		if (doTutorialBox) {
			GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<TheWorldControllerTEST> ().DoTutorial ();
		}
		doTutorialBox = false;
    }

	private IEnumerator ToTheWorldWithMaterials(List<RecipeMaterial> recipMats){
		yield return SceneManager.LoadSceneAsync ("TheWorld");
		GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<TheWorldControllerTEST> ().SetResources (recipMats);
	}

	public void GetLastMarkers(List<MarkerDataContainer> vl){
		//vl = storedMarkers;
		for (int i = 0; i < storedMarkers.Count; i++) {
			vl.Add (storedMarkers [i]);
		}
		Debug.Log ("Last markers get, entries: vl: " + vl.Count + ", sM: " + storedMarkers.Count);

		if (storedMarkers.Count > 0) {
			Debug.Log ("latlong of entry 0: " + storedMarkers [0]._latlong.x + ", " + storedMarkers[0]._latlong.y);
		}
	}
	public void SetLastMarkers(List<MarkerDataContainer> vl){
		//storedMarkers = vl;
		storedMarkers.Clear();
		for (int i = 0; i < vl.Count; i++) {
			storedMarkers.Add (vl[i]);
		}
		Debug.Log ("Last markers set, entries: " + storedMarkers.Count);
	}

	public void ReceiveChosenLoadout(int loadoutIndex){
		if (storedNode != null) {
			StartCombat (loadoutIndex, storedNode);
		}
	}

	public void CombatPrompt (List<NodeEnemy> enemyList){
		TheWorldControllerTEST testCon = GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<TheWorldControllerTEST> ();
		testCon.ReturnFromLoadoutMenu ();
		storedNode = enemyList;
		testCon.GenerateLoadoutButtons ();
	}

	private void StartCombat(int loadoutIndex, List<NodeEnemy> enemyList){
		if (interactedNode != null) {
			DisableInteractedNode ();
			Debug.Log (loadoutIndex + " is loadoutIndex");
			StartCoroutine (StartCombatIenum (loadoutList.GetLoadout (loadoutIndex), enemyList));
		}
	}

	private IEnumerator StartCombatIenum(Loadout loadout, List<NodeEnemy> enemyList){
		yield return SceneManager.LoadSceneAsync ("BattleScene");
		CombatController comCon = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		//playerWorldStats;
		// TODO: Add buffs to StartCombat
		comCon.StartCombat (loadout, enemyList);
	}

	public void UpdateWorldPosition(double latitude, double longitude){
		this.locLatitude = latitude;
		this.locLongitude = longitude;
	}

	public void EndCombat(List<List<RecipeMaterial>> dropListList){
		List<RecipeMaterial> dropListFinal = new List<RecipeMaterial> ();
		foreach (List<RecipeMaterial> dropList in dropListList) {
			foreach (RecipeMaterial recMat in dropList) {
				Inventory.InsertRecipeMaterial (recMat);
				dropListFinal.Add(recMat);
			}
		}
		StartCoroutine (ToTheWorldWithMaterials(dropListFinal));
	}

	public void GetGatherinNode(int nodeIndex, int dropAmount){
		if (interactedNode != null) {
			DisableInteractedNode ();
			DropData nodeDropData = DropDataCreator.CreateDropData (DropperType.Gather, nodeIndex);
			List<RecipeMaterial> dropList = DropDataCreator.CalculateDrops (nodeDropData, dropAmount, null);
            StartGatheringGame(dropList);
			/*
			foreach (RecipeMaterial recMat in dropList) {
				Inventory.InsertRecipeMaterial (recMat);
			}
			GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<TheWorldControllerTEST> ().SetResources (dropList);
			*/
		}
	}

    private void StartGatheringGame(List<RecipeMaterial> droplist) {
        StartCoroutine(StartGatheringEnum(droplist));
    }

    private IEnumerator StartGatheringEnum(List<RecipeMaterial> droplist) {
        yield return SceneManager.LoadSceneAsync("GatherMinigame");
		GameObject.FindGameObjectWithTag("MineGrid").GetComponent<PopulateGrid>().StartTheMinigame(droplist);
    }

	public void EndGatheringGame(List<RecipeMaterial> droplist){
		foreach (RecipeMaterial recMat in droplist) {
			Inventory.InsertRecipeMaterial (recMat);
		}
		StartCoroutine (ToTheWorldWithMaterials(droplist));
	}

    public void StartCrafting(){
		StartCoroutine (StartCraftingIenum ());
	}

	private IEnumerator StartCraftingIenum(){
		yield return SceneManager.LoadSceneAsync ("Crafting");
		//Crafting craft = GameObject.FindGameObjectWithTag ("CraftingController").GetComponent<Crafting> ();
	}

    public void StartLoadoutManagement() {
        StartCoroutine(StartLoadoutManagementEnum());
    }

    private IEnumerator StartLoadoutManagementEnum() {
        yield return SceneManager.LoadSceneAsync ("Loadout");
    }

	public void StartUpgrading(){
		StartCoroutine(StartUpgradingIenum());
	}

	private IEnumerator StartUpgradingIenum(){
		yield return SceneManager.LoadSceneAsync ("CraftingUpgrade");
		//CraftingUpgrade craft = GameObject.FindGameObjectWithTag ("CraftingUpgradeController").GetComponent<CraftingUpgrade> ();
	}

	public void EndCrafting(){
		StartCoroutine (ToTheWorld());
	}


	public void StartTutorial(){
		StartCoroutine (StartTutorialIenum ());
	}

	private IEnumerator StartTutorialIenum(){
		yield return SceneManager.LoadSceneAsync ("TutorialBattle");
		CombatController comCon = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		Loadout loadout = new Loadout (1);
		loadout.AddArmor (new InventoryArmor (0, "Helm"));
		loadout.AddArmor (new InventoryArmor (0, "Chest"));
		loadout.AddArmor (new InventoryArmor (0, "Arms"));
		loadout.AddArmor (new InventoryArmor (0, "Legs"));
		loadout.AddArmor (new InventoryArmor (0, "Boots"));
		loadout.AddMainHand (new InventoryWeapon (0, "Sword"));
		List<NodeEnemy> enemyList = new List<NodeEnemy> ();
		enemyList.Add (new NodeEnemy (3, "EnemySmall")); // Mushroom
		comCon.StartCombat (loadout, enemyList);
	}




	private IEnumerator UpdateLocationData(float seconds){
		while (true){
			yield return new WaitForSeconds (seconds);
			// TODO: Get latitude and longitude from phone
			locLatitude = 0;
			locLongitude = 0;
			//LoadNodes (locLatitude, locLongitude);
		}
	}

	public void NullNodeData(){
		storedNode = null;
	}

	private void DisableInteractedNode(){
		if (interactedNode != null) {
			interactedNode.SetActive (false);
		}
	}
}
