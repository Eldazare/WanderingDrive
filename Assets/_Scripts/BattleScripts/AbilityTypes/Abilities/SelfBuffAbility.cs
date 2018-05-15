using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfBuffAbility : Ability {

	int potency;
	int turns;
	_Buff appliedBuff;

	public SelfBuffAbility(){
	}


	override protected void InitializeInside(){
		this.potency = int.Parse (dataArray [2]);
		this.turns = int.Parse (dataArray [3]);
	}


	override public void UseAbility(){
		//player.GetComponent<AbilityController>().AbilityEffect("SelfBuffEffect");
		player.playerStats.stamina -= staminaCost;
		_Buff buff = new Accuracy((int)this.potency);
		buff.turnsRemaining = this.turns;
		player.playerBuffs.Add(buff);
		player.UpdateStats();
	}
}
