using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyBuff : Ability {

	public AccuracyBuff(PlayerCombatScript _player){
		player = _player;
	}
	override public void UseAbility(){
		player.GetComponent<AbilityController>().AbilityEffect("AccuracyBuff");
		player.playerStats.stamina -= staminaCost;
		_Buff buff = new Accuracy((int)potency);
		buff.turnsRemaining = 5;
		player.playerBuffs.Add(buff);
		player.UpdateStats();
	}
}
