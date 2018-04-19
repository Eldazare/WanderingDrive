using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropperType{EnemySmall, EnemyLarge, Gather};
// Copy to NameType in NameDescContainer

public static class DropDataCreator {

	// TODO: part maximum amount from config at some point

	// TODO: Better random than Random.Range?


	// Just put enemyStats.subtype here lol. More example than useful method tbh.
	public static DropperType parseDroppertype (string str){
		return (DropperType)System.Enum.Parse (typeof(DropperType), str);
	}

	public static int DefaultNormalDropAmount(){
		return DataManager.ReadDataInt ("Drop_DefaultDropAmount");
	}


	public static DropData CreateDropData(DropperType dropperType, int id){
		string begin = "Drop_" + dropperType.ToString() + "_"+id+"_";
		DropData createe = new DropData ();
		if (dropperType != DropperType.Gather) {
			EnemyDropData createeEnemy = new EnemyDropData ();
			for (int i = 0; i<9;i++){
				string dataStr = DataManager.ReadDataString (begin + "p" + i);
				if (dataStr == null) {
					createeEnemy.partDropDatas.Add (null);
				} else {
					string[] infoSplit = dataStr.Split ("/".ToCharArray ());
					DropData createeDrop = new DropData ();
					foreach (string str in infoSplit) {
						string[] datasplit = str.Split ("_".ToCharArray ());
						createeDrop.drops.Add (new RecipeMaterial ("Mat_Mat_" + datasplit [0]));
						createeDrop.percentageList.Add (int.Parse (datasplit [1]));
					}
					createeEnemy.partDropDatas.Add (createeDrop);
				}
			}
			createe = createeEnemy;
		}
		string partString = DataManager.ReadDataString (begin + "Normal");
		if (partString == null) {
			Debug.LogError ("Null Normal drop" + dropperType.ToString ());
		}
		string[] normalDrops = partString.Split ("/".ToCharArray ());
		foreach (string str in normalDrops) {
			string[] partSplit = str.Split ("_".ToCharArray ());
			createe.drops.Add (new RecipeMaterial ("Mat_Mat_" + partSplit [0]));
			createe.percentageList.Add (int.Parse (partSplit [1]));
		}
		return createe;
	}


	// Last parameter can be null
	public static List<RecipeMaterial> CalculateDrops (DropData dropData, int normalDropAmount, List<EnemyPart> partList){
		List<RecipeMaterial> createe = new List<RecipeMaterial> ();
		for (int i = 0; i < normalDropAmount; i++) {
			createe.Add(GetSingleDrop (dropData));
		}
		List<DropData> partDrops = dropData.GetPartDrops ();
		if (partDrops != null) {
			for (int i = 0; i < 9; i++) {
				if (partDrops [i] != null) {
					if (partList [i].broken) {
						createe.Add (GetSingleDrop (partDrops [i]));
					}
				}
			}
		}
		return createe;
	}


	private static RecipeMaterial GetSingleDrop (DropData dropData){
		List<RecipeMaterial> drops = dropData.drops;
		List<int> percentages = dropData.percentageList;
		int max = 0;
		foreach (int i in percentages) {
			max += i;
		}
		int choice = Random.Range (0, max) + 1;
		int total = 0;
		int resultIndex = -1;
		for (int j = 0; j < percentages.Count; j++) {
			total += percentages [j];
			if (total >= choice) {
				resultIndex = j;
				break;
			}
		}
		Debug.Log (resultIndex + " & count: " + drops.Count + " Percentages count: "+percentages.Count);
		Debug.Log (choice + "/" + max);
		return drops [resultIndex];
	}
}
