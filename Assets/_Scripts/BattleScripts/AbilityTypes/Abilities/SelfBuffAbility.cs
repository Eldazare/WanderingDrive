using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBuffAbility : Ability {

	int potency;
	int turns;
	string appliedBuffType;

	public SelfBuffAbility(){
	}


	override protected void InitializeInside(){
		appliedBuffType = dataArray[2];
		this.potency = int.Parse (dataArray [3]);
		this.turns = int.Parse (dataArray [4]);
	}


	override public void UseAbility(){
		//player.GetComponent<AbilityController>().AbilityEffect("SelfBuffEffect");
		player.playerStats.stamina -= staminaCost;
		_Buff buff = (_Buff)System.Activator.CreateInstance (System.Type.GetType (this.appliedBuffType), this.potency);
		buff.turnsRemaining = this.turns;
		player.playerBuffs.Add(buff);
		player.UpdateStats();
	}
}
