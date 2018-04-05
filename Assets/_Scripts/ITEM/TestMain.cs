using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMain : MonoBehaviour {

	// FOR TESTING PURPOSES. No final code here.
	UndyingObject und;

	void Start () {
		DontDestroyOnLoad (this);
		und = this.GetComponent<UndyingObject> ();
		/*
		Inventory inv = new Inventory ();
		EnemyStats stats = EnemyStatCreator.LoadStatBlockData (0, "enemySmall");
		stats.partList [1].broken = true;
		DropData dropDat = DropDataCreator.CreateDropData (DropDataCreator.parseDroppertype ("enemySmall"), 0);

		List<RecipeMaterial> lastList = DropDataCreator.CalculateDrops (dropDat, 4, stats.partList);

		foreach (RecipeMaterial mat in lastList) {
			Debug.Log ("matname: " + NameDescContainer.GetName((NameType)System.Enum.Parse(typeof(NameType), mat.subtype), mat.itemId));
		} */
		StartCoroutine (WaitLoadScene ());
	}

	private IEnumerator WaitLoadScene(){
		yield return new WaitForSeconds(5);
		Loadout loadout = new Loadout (1);
		loadout.AddMainHand (new InventoryWeapon (0, "sword"));
		und.loadoutList.InsertLoadout (loadout, 0);
		WorldNode testNode = this.GetComponent<WorldNode> ();
		testNode.Interact ();
	}
}
