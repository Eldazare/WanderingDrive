using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndyingObject : MonoBehaviour {

	// container of all player data
	// Travels through scenes
	// add more stuff as it comes up


	// PUBLIC only for testing purposes
	public LoadoutsContainer loadoutList;
	public WorldStatsContainer playerWorldStats;
	private NodeSpawner nodeSpawner;

	double locLatitude;
	double locLongitude;
	int chosenLoadout;
	List<NodeEnemy> storedNode;

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

        // Get location data here
        //StartCoroutine (UpdateLocationData(10)); // Enable this when testing location
		FillInventoryForTest (); // TESTING ONLY
        StartCoroutine (ToTheWorld ());
	}

	private void FillInventoryForTest(){
		Inventory.PutItem (ItemType.Wep, ItemSubType.Sword, 0, 1);
		Inventory.PutItem (ItemType.Mat, ItemSubType.Mat, 1, 4);
		Inventory.PutItem (ItemType.Mat, ItemSubType.Mat, 2, 1);
		Inventory.PutItem (ItemType.Arm, ItemSubType.Accessory, 0, 1);
		InsertBasicArmorToInventory ();
		Loadout loadout = new Loadout (1);
		loadout.AddMainHand (new InventoryWeapon (0, "Sword"));
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
		loadoutList.InsertLoadout (loadout, 9);
	}

	private IEnumerator ToTheWorld(){
        yield return SceneManager.LoadSceneAsync ("TheWorld");
		nodeSpawner = GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<NodeSpawner> ();
    }

	public void ReceiveChosenLoadout(int loadoutIndex){
		if (storedNode != null) {
			StartCombat (loadoutIndex, storedNode);
		}
	}

	public void CombatPrompt (List<NodeEnemy> enemyList){
		storedNode = enemyList;
		GameObject.FindGameObjectWithTag ("NodeSpawner").GetComponent<TheWorldControllerTEST> ().GenerateLoadoutButtons ();
	}

	private void StartCombat(int loadoutIndex, List<NodeEnemy> enemyList){
		Debug.Log (loadoutIndex + " is loadoutIndex");
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

	public void EndCombat(List<List<RecipeMaterial>> dropListList){
		foreach (List<RecipeMaterial> dropList in dropListList) {
			foreach (RecipeMaterial recMat in dropList) {
				Inventory.InsertRecipeMaterial (recMat);
			}
		}
		StartCoroutine (ToTheWorld());
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



	private IEnumerator UpdateLocationData(float seconds){
		while (true){
			yield return new WaitForSeconds (seconds);
			// TODO: Get latitude and longitude from phone
			locLatitude = 0;
			locLongitude = 0;
			nodeSpawner.LoadNodes (locLatitude, locLongitude);
		}
	}

	public void NullNodeData(){
		storedNode = null;
	}
}
