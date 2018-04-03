using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DropDataCreator {

	// TODO: part maximum amount from config at some point

	// TODO: Better random than Random.Range?


	// Just put enemyStats.subtype here lol. More example than useful method tbh.
	public static DropperType parseDroppertype (string str){
		return (DropperType)System.Enum.Parse (typeof(DropperType), str);
	}


	public static DropData CreateDropData(DropperType dropperType, int id){
		string begin = "drop_" + System.Enum.GetName (typeof(DropperType), dropperType) + "_"+id+"_";
		DropData createe = new DropData ();
		if (dropperType != DropperType.gather) {
			EnemyDropData createeEnemy = new EnemyDropData ();
			createeEnemy.partDropDatas = new List<DropData> ();
			for (int i = 1; i<10;i++){
				DropData createeDrop = new DropData ();
				int j = 0;
				while (true) {
					string dataStr = DataManager.ReadDataString (begin + "p" + i + "_" + j);
					if (dataStr == null) {
						if (createeDrop.drops.Count == 0) {
							createeEnemy.partDropDatas.Add (null);
						} else {
							createeEnemy.partDropDatas.Add (createeDrop);
						}
						break;
					} else {
						string[] datasplit = dataStr.Split ("_".ToCharArray ());
						createeDrop.drops.Add (new RecipeMaterial ("mat_mat_" + datasplit[0]));
						createeDrop.percentageList.Add (int.Parse (datasplit [1]));
						j++;
					}
				}
			}
			createe = createeEnemy;
		}
		int k = 0;
		while (true){
			string partString = DataManager.ReadDataString (begin + k);
			if (partString == null) {
				break;
			}
			string[] partSplit = partString.Split ("_".ToCharArray ());
			createe.drops.Add(new RecipeMaterial("mat_mat_"+partSplit[0]));
			createe.percentageList.Add(int.Parse(partSplit[1]));
			k++;
		}
		return createe;
	}


	// Last parameter can be null
	public static List<RecipeMaterial> CalculateDrops (DropData dropData, int normalDropAmount, List<EnemyPart> partList){
		List<RecipeMaterial> createe = new List<RecipeMaterial> ();
		for (int i = 0; i < normalDropAmount; i++) {
			createe.Add(GetSingleDrop (dropData.drops, dropData.percentageList));
		}
		List<DropData> partDrops = dropData.GetPartDrops ();
		if (partDrops != null) {
			for (int i = 0; i < 9; i++) {
				Debug.Log ("i = " + i);
				if (partDrops [i] != null) {
					if (partList [i].broken) {
						createe.Add (GetSingleDrop (partDrops [i].drops, partDrops [i].percentageList));
					}
				}
			}
		}
		return createe;
	}


	private static RecipeMaterial GetSingleDrop (List<RecipeMaterial> drops, List<int> percentages){
		int choice = Random.Range (0, 100) + 1;
		int total = 0;
		int resultIndex = -1;
		for (int j = 0; j < percentages.Count; j++) {
			total += percentages [j];
			if (total >= choice) {
				resultIndex = j;
				break;
			}
		}
		return drops [resultIndex];
	}
}
