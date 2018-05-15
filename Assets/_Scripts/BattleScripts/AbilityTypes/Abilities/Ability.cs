using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability{

	public int staminaCost;
	public string abilityName;
	//public string abilityBaseClass;
	public bool offensive;


	protected PlayerCombatScript player;
	public string[] dataArray;

	public void Initialize(PlayerCombatScript _player){
		this.player = _player;
		//this.abilityBaseClass = dataArray [0];
		this.staminaCost = int.Parse(dataArray [1]);
		InitializeInside ();
	}

	// Must be called with creator
	public void SetStringArr(string[] dataString, string theName){
		this.dataArray = dataString;
		this.abilityName = theName;
	}


	// Override in childs
	public virtual void UseAbility(){}

	// Override in childs
	protected virtual void InitializeInside(){}

	protected List<ComboPieceAbstraction> ConvertToComboPieces(List<string> strParts){
		List<ComboPieceAbstraction> returnee = new List<ComboPieceAbstraction> ();
		foreach (string str in strParts) {
			string[] data = str.Split ("_".ToCharArray ());
			returnee.Add(new ComboPieceAbstraction((ComboPieceType)System.Enum.Parse(typeof(ComboPieceType), data[0]), float.Parse(data[1])));
		}
		return returnee;
	}
}
