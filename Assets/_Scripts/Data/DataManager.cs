using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DataManager  {
	static private bool readBool = false;
	// TODO: Recipe/Weapon/Upgrade dictionary may BLOAT


	public enum DataManagerDictionaryTypes{
		consumable, material, armor, recipe, enemySmall, enemyLarge, drop, gather,
		weapon, recipeUp
	}

	static List<Dictionary<string,string>> configDatas = new List<Dictionary<string,string>>{ };

	public static List<string> nameListGeneric = new List<string>{};
	public static List<string> descriptionListGeneric= new List<string>{};
	public static List<string> recipeNameList= new List<string>{};

	static public string ReadDataString(string entryName){
		if (!readBool) {
			DownloadTextData ();
		}
		string[] splitted= entryName.Split ("_".ToCharArray());
		string identifier = splitted [0];

		try{
			DataManagerDictionaryTypes dicType = (DataManagerDictionaryTypes)System.Enum.Parse (typeof(DataManagerDictionaryTypes), identifier);
			return configDatas[System.Convert.ToInt32(dicType)][entryName];
		} catch{
			Debug.LogError ("Identifier part is not defined in DataManager: " + identifier);
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
		for (int i = 0; i < System.Enum.GetNames (typeof(DataManagerDictionaryTypes)).Length; i++) {
			configDatas.Add(new Dictionary<string,string>());
		}
		DownloadSingleFile ("ConsumableCombatConfig", configDatas[(int)DataManagerDictionaryTypes.consumable], nameListGeneric);
		DownloadSingleFile ("ConsumableNonConConfig", configDatas[(int)DataManagerDictionaryTypes.consumable], nameListGeneric);
		DownloadSingleFile ("MaterialConfig", configDatas[(int)DataManagerDictionaryTypes.material], nameListGeneric);
		DownloadSingleFile ("EnemySmallConfig", configDatas[(int)DataManagerDictionaryTypes.enemySmall], nameListGeneric);
		DownloadSingleFile ("EnemyLargeConfig", configDatas[(int)DataManagerDictionaryTypes.enemyLarge], nameListGeneric);
		DownloadSingleFile ("ArmorConfig", configDatas[(int)DataManagerDictionaryTypes.armor], nameListGeneric);
		DownloadSingleFile ("DropConfig", configDatas[(int)DataManagerDictionaryTypes.drop], nameListGeneric);
		DownloadSingleFile ("GatheringConfig", configDatas[(int)DataManagerDictionaryTypes.gather], nameListGeneric);
		DownloadSingleFile ("RecipeConfig", configDatas[(int)DataManagerDictionaryTypes.recipe], recipeNameList);

		DownloadSingleFile ("RecipeSwordUpgradeConfig", configDatas[(int)DataManagerDictionaryTypes.recipeUp], recipeNameList);
		DownloadSingleFile ("WeaponSwordConfig", configDatas[(int)DataManagerDictionaryTypes.weapon], nameListGeneric);
		readBool = true;
		NameDescContainer.GenerateNames (nameListGeneric, descriptionListGeneric);
		RecipeContainer.GenerateRecipes (recipeNameList);
	}

	static private void DownloadSingleFile(string filename, Dictionary<string,string> dic, List<string> namelist){
		string path = "DataText/" + filename;
		TextAsset fullData = Resources.Load (path) as TextAsset; 
		string[] data = fullData.text.Split("\r\n".ToCharArray());
		foreach (string line in data) {
			if (line.Length > 0){
				if (line [0] == "&" [0]) {
					descriptionListGeneric.Add (line.Substring (1));
				}
				else if (line [0] == "%"[0]) {
					namelist.Add (line.Substring(1));
				}
				else if (line[0] != "#"[0]) {
					string[] keyValue = line.Split ("="[0]);
					dic.Add (keyValue [0], keyValue [1]);
				}
			}
		}
	}
}
