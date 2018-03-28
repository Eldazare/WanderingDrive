using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager  {
	static private bool readBool = false;
	// TODO: Add rest of weapon dictionaries

	static Dictionary<string,string> consumableData = new Dictionary<string, string>{ };
	static Dictionary<string,string> materialData = new Dictionary<string, string>{ };
	static Dictionary<string,string> armorData = new Dictionary<string, string>{ };
	static Dictionary<string,string> recipeData = new Dictionary<string, string>{ };
	static Dictionary<string,string> enemySmallData = new Dictionary<string, string>{ };
	static Dictionary<string,string> enemyLargeData = new Dictionary<string, string>{ };
	static Dictionary<string,string> dropData = new Dictionary<string, string>{ };
	static Dictionary<string,string> gatherData = new Dictionary<string, string>{ };

	public static List<string> nameListGeneric = new List<string>{};
	public static List<string> descriptionListGeneric= new List<string>{};

	static Dictionary<string,string> weaponSwordData = new Dictionary<string, string>{ };
	static Dictionary<string,string> recipeUpSwordData = new Dictionary<string, string>{ };

	public static List<string> recipeNameList= new List<string>{};



	static public string ReadDataString(string entryName){
		if (!readBool) {
			DownloadTextData ();
		}
		string[] splitted= entryName.Split ("_".ToCharArray());
		string identifier = splitted [0];
		switch (identifier) {
		case "consumable":
			return consumableData [entryName];
		case "material":
			return materialData [entryName];
		case "enemySmall":
			return enemySmallData [entryName];
		case "enemyLarge":
			return enemyLargeData [entryName];
        case "armor":
            return armorData[entryName];
		case "recipe":
			return recipeData[entryName];
		case "drop":
			return dropData[entryName];
		case "gather":
			return gatherData[entryName];
		case "weapon":
			switch (splitted [1]) {
			case "sword":
				return weaponSwordData [entryName];
			default:
				return null;
			}
		case "recipeUp":
			switch (splitted [1]) {
			case "sword":
				return recipeUpSwordData [entryName];
			default:
				return null;
			}
		default:
			Debug.LogError ("DataManager was given unsuitable identifier in entryName (" + entryName + ").");
			return null;
		}
	}

	static public float ReadDataFloat(string entryName){
		string value = ReadDataString (entryName);
		return float.Parse (value);
	}

	static public int ReadDataInt(string entryName){
		string value = ReadDataString (entryName);
		return int.Parse (value);
	}


	static private void DownloadTextData(){
		DownloadSingleFile ("ConsumableCombatConfig", consumableData, nameListGeneric);
		DownloadSingleFile ("ConsumableNonConConfig", consumableData, nameListGeneric);
		DownloadSingleFile ("MaterialConfig", materialData, nameListGeneric);
		DownloadSingleFile ("EnemySmallConfig", enemySmallData, nameListGeneric);
		DownloadSingleFile ("EnemyLargeConfig", enemyLargeData, nameListGeneric);
		DownloadSingleFile ("ArmorConfig", armorData, nameListGeneric);
		DownloadSingleFile ("DropConfig", dropData, nameListGeneric);
		DownloadSingleFile ("GatheringConfig", gatherData, nameListGeneric);
		DownloadSingleFile ("RecipeConfig", recipeData, recipeNameList);

		DownloadSingleFile ("RecipeSwordUpgradeConfig", recipeUpSwordData, recipeNameList);
		DownloadSingleFile ("WeaponSwordConfig", weaponSwordData, nameListGeneric);
		readBool = true;
		/* NameDescContainer.GenerateNames (nameListGeneric, descriptionListGeneric);
		RecipeContainer.GenerateRecipes (recipeNameList); */
	}

	static private void DownloadSingleFile(string filename, Dictionary<string,string> dic, List<string> namelist){
		string path = "DataText/" + filename;
		TextAsset fullData = Resources.Load (path) as TextAsset; 
		string[] data = fullData.text.Split("\r\n".ToCharArray());
		foreach (string line in data) {
			if (line.Length > 0){
				if (line [0] == "&" [0]) {
					descriptionListGeneric.Add (line);
				}
				else if (line [0] == "%"[0]) {
					namelist.Add (line);
				}
				else if (line[0] != "#"[0]) {
					string[] keyValue = line.Split ("="[0]);
					dic.Add (keyValue [0], keyValue [1]);
				}
			}
		}
	}
}
