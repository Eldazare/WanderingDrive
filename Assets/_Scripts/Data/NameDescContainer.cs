using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class NameDescContainer  {

	private static bool genBool = false;

	public enum NameType{material, nonconCon, conCon, sword, axe, spear, dagger, pistol, bow, greatbow, buckler, towershield, armor, enemySmall, enemyLarge, gather};
	// subtypes as per index: 
	//material, nonconCon, conCon (0-2)
	//sword, axe, spear, dagger, pistol, bow, greatbow, buckler, towershield (3-11)
	//armor (12)
	//enemySmall, enemyLarge (13-14)
	//gatherNode (15)

	static List<List<string>> names = new List<List<string>>{ };
	static List<List<string>> descriptions = new List<List<string>>{ };

	public static void GenerateNames(List<string> namelist, List<string> descList){
		if (!genBool) {
			genBool = true;
			foreach (string str in namelist) {
				string[] splitStr = str.Split ("_".ToCharArray ());
				NameType nametype = (NameType)System.Enum.Parse (typeof(NameType), splitStr [1]);
				names [Convert.ToInt32 (nametype)] [int.Parse (splitStr [2])] = splitStr [3];
			}

			foreach (string str in descList) {
				string[] splitStr = str.Split ("_".ToCharArray ());
				NameType nametype = (NameType)System.Enum.Parse (typeof(NameType), splitStr [1]);
				descriptions [Convert.ToInt32 (nametype)] [int.Parse (splitStr [2])] = splitStr [3];
			}
		} else {
			Debug.LogError ("GenerateNames called twice or more");
		}
	}

	public static string GetName(NameType subtype, int index){
		return names [Convert.ToInt32 (subtype)] [index];
	}

	public static string GetDescription(NameType subtype, int index){
		return descriptions [Convert.ToInt32 (subtype)] [index];
	}
}
