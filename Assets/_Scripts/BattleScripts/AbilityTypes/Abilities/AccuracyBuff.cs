using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccuracyBuff : Ability {

	float potency;

	public AccuracyBuff(){

	}


	override public void Initialize(PlayerCombatScript _player){
		this.player = _player;
		string[] datas = dataString.Split ("/".ToCharArray());
		// TODO: bler
	}


	override public void UseAbility(){
		//player.GetComponent<AbilityController>().AbilityEffect("AccuracyBuff");
		player.playerStats.stamina -= staminaCost;
		_Buff buff = new Accuracy((int)potency);
		buff.turnsRemaining = 5;
		player.playerBuffs.Add(buff);
		player.UpdateStats();
	}
}
