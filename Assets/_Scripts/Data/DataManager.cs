using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum DataManagerDictionaryType{
	ComCon, NonCom, Material, Armor, Accessory, EnemySmall, EnemyLarge, Drop, Gather,
	RecipeWeapon, RecipeArmor, RecipeAccessory, RecipeComCon, RecipeNonCom, 	// copy of RecipeType
	Sword, Mace, Spear, Dagger, Pistol, Bow, GBow, ShieldS, ShieldL, Talisman, 	// copy of WeaponType
	RecipeUpSword, RecipeUpMace, RecipeUpSpear, RecipeUpDagger, RecipeUpPistol, RecipeUpBow, RecipeUpGBow,
	RecipeUpShieldS, RecipeUpShieldL, RecipeUpTalisman, 						// "copy" of WeaponType
	RecipeUpHelm, RecipeUpChest, RecipeUpArms, RecipeUpLegs, RecipeUpBoots, RecipeUpAccessory,
	Node,
	Ability
}

public static class DataManager  {
	static private bool readBool = false;
	// TODO: Recipe/RecipeUp dictionary may BLOAT

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
			DataManagerDictionaryType dicType = (DataManagerDictionaryType)System.Enum.Parse (typeof(DataManagerDictionaryType), identifier);
			try{
				return configDatas[System.Convert.ToInt32(dicType)][entryName];
			} catch{
				return null;
			}
		} catch{
			if (identifier != "nonexistent") {
				Debug.LogError ("Identifier part is not defined in DataManager: " + identifier);
			}
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
		for (int i = 0; i < System.Enum.GetNames (typeof(DataManagerDictionaryType)).Length; i++) {
			configDatas.Add(new Dictionary<string,string>());
		}
		DownloadSingleFile ("ConsumableCombatConfig", configDatas[(int)DataManagerDictionaryType.ComCon], nameListGeneric);
		DownloadSingleFile ("ConsumableNonConConfig", configDatas[(int)DataManagerDictionaryType.NonCom], nameListGeneric);
		DownloadSingleFile ("MaterialConfig", configDatas[(int)DataManagerDictionaryType.Material], nameListGeneric);
		DownloadSingleFile ("EnemySmallConfig", configDatas[(int)DataManagerDictionaryType.EnemySmall], nameListGeneric);
		DownloadSingleFile ("EnemyLargeConfig", configDatas[(int)DataManagerDictionaryType.EnemyLarge], nameListGeneric);
		DownloadSingleFile ("DropConfig", configDatas[(int)DataManagerDictionaryType.Drop], nameListGeneric);
		DownloadSingleFile ("GatheringConfig", configDatas[(int)DataManagerDictionaryType.Gather], nameListGeneric);
		DownloadSingleFile ("MultiBattleNodes", configDatas [(int)DataManagerDictionaryType.Node], nameListGeneric);

		DownloadSingleFile ("ArmorConfig", configDatas[(int)DataManagerDictionaryType.Armor], nameListGeneric);
		DownloadSingleFile ("AccessoryConfig", configDatas [(int)DataManagerDictionaryType.Accessory], nameListGeneric);
		DownloadSingleFile ("AbilityConfig", configDatas [(int)DataManagerDictionaryType.Ability], nameListGeneric);

		string[] armorEnumNames = System.Enum.GetNames (typeof(ArmorType));
		foreach (string armorString in armorEnumNames) {
			string armorRUPFile = "RecipeUpgrade/RecipeUp" + armorString + "Config";
			int index = (int)System.Enum.Parse (typeof(DataManagerDictionaryType), "RecipeUp" + armorString);
			DownloadSingleFile (armorRUPFile, configDatas [index], recipeNameList);
		}

		string[] recipeEnumNames = System.Enum.GetNames (typeof(CraftingRecipeType));
		foreach (string recipString in recipeEnumNames) {
			string recipeFile = "Recipes/Recipe"+recipString+"Config";
			string recipeDictionaryType = "Recipe" + recipString;
			DownloadSingleFile (recipeFile, configDatas [(int)System.Enum.Parse (typeof(DataManagerDictionaryType), recipeDictionaryType)], recipeNameList);
		}

		string[] wepEnumNames = System.Enum.GetNames (typeof(WeaponType));
		foreach (string wepString in wepEnumNames) {
			DownloadWeaponData (wepString);
		}
		readBool = true;

		NameDescContainer.GenerateNames (nameListGeneric, descriptionListGeneric); // Must be before any other stuffgeneration
		RecipeContainer.GenerateRecipes (recipeNameList);
		// NodeSpriteContainer.LoadSpriteData ();
	}

	static private void DownloadSingleFile(string filename, Dictionary<string,string> dic, List<string> namelist){
		string path = "DataText/" + filename;
		TextAsset fullData = Resources.Load (path) as TextAsset;
		if (fullData == null) {
			Debug.LogError ("FILE: " + path + " NOT FOUND!");
		}
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

	static private void DownloadWeaponData(string wepStr){
		string configPath = "Weapons/"+wepStr + "/Weapon" + wepStr + "Config";
		string recipePath = "Weapons/"+wepStr + "/Recipe" + wepStr + "UpgradeConfig";
		int dictionaryIndex = (int)System.Enum.Parse (typeof(DataManagerDictionaryType), wepStr);
		int dictionaryRecipeUpIndex = (int)System.Enum.Parse (typeof(DataManagerDictionaryType), "RecipeUp" + wepStr);
		DownloadSingleFile (configPath, configDatas [dictionaryIndex], nameListGeneric);
		DownloadSingleFile (recipePath, configDatas [dictionaryRecipeUpIndex], recipeNameList);
	}
}
