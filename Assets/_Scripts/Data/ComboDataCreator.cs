using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ComboPieceType{Tap, Swipe, Spin}

public static class ComboDataCreator  {

	public static List<ComboPieceAbstraction> GetWeaponCombo(WeaponType wepType, int level){
		// TODO: Datamanager content
		string begin = "Combo_"+wepType.ToString()+"_"+level;
		string[] data = DataManager.ReadDataString (begin).Split ("/".ToCharArray ());
		List<ComboPieceAbstraction> returnee = new List<ComboPieceAbstraction> ();
		foreach (string str in data) {
			string[] content = str.Split ("_".ToCharArray ());
			ComboPieceAbstraction cp = new ComboPieceAbstraction ((ComboPieceType)System.Enum.Parse (typeof(ComboPieceType), content [0]), float.Parse (content [1]));
			returnee.Add (cp);
		}
		return returnee;
	}
}
