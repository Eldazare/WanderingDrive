using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMain : MonoBehaviour {


	void Start () {
		DontDestroyOnLoad (this);
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
		List<NodeEnemy> nodeInfo = new List<NodeEnemy> ();
		for (int i = 0; i < 3; i++) {
			nodeInfo.Add (new NodeEnemy (0, "enemySmall"));
		}
		Loadout loadout = new Loadout (1);
		loadout.AddMainHand (new Inventory_Weapon (0, "sword"));
		yield return SceneManager.LoadSceneAsync ("BattleScene");
		CombatController comCon = GameObject.FindGameObjectWithTag ("CombatController").GetComponent<CombatController> ();
		comCon.StartCombat (loadout, nodeInfo);
	}
}
