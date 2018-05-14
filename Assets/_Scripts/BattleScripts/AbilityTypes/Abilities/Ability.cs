using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability{

	public int staminaCost;
	public string abilityName;
	public bool offensive;


	protected PlayerCombatScript player;
	public string dataString;

	public virtual void UseAbility(){}
	public virtual void Initialize(PlayerCombatScript _player){}

	protected List<ComboPieceAbstraction> ConvertToComboPieces(List<string> strParts){
		List<ComboPieceAbstraction> returnee = new List<ComboPieceAbstraction> ();
		foreach (string str in strParts) {
			string[] data = str.Split ("_".ToCharArray ());
			returnee.Add(new ComboPieceAbstraction((ComboPieceType)System.Enum.Parse(typeof(ComboPieceType), data[0]), float.Parse(data[1])));
		}
		return returnee;
	}
}
