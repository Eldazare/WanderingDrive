using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : Ability{

	private int damage, elementDamage;
	private Element element;

	private List<ComboPieceAbstraction> comboList; 

	public AttackAbility(){
	}

	override protected void InitializeInside(){
		this.damage = int.Parse(dataArray [2]);
		this.elementDamage = int.Parse(dataArray [3]);
		this.element = (Element)System.Enum.Parse (typeof(Element), dataArray [4]);
		List<string> comboAsString = new List<string>();
		for (int i = 5; i < dataArray.Length; i++) {
			comboAsString.Add (dataArray [i]);
		}
		comboList = ConvertToComboPieces (comboAsString);
	}

	override public void UseAbility(){
		player.GetComponent<AbilityController>().AbilityEffect("FireBall");
		player.playerStats.stamina -= staminaCost;
		player.UpdateStats();
	}
}
