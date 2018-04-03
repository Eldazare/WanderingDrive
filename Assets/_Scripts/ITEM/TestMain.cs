using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Inventory inv = new Inventory ();
		EnemyStats stats = EnemyStatCreator.LoadStatBlockData (0, "enemySmall");
		stats.partList [1].broken = true;
		DropData dropDat = DropDataCreator.CreateDropData (DropDataCreator.parseDroppertype ("enemySmall"), 0);

		List<RecipeMaterial> lastList = DropDataCreator.CalculateDrops (dropDat, 4, stats.partList);

		foreach (RecipeMaterial mat in lastList) {
			Debug.Log ("matId: " + mat.itemId);
		} 
	}

}
