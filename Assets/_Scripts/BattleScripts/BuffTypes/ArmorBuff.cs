using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : _Buff {
	float returnDelay = 0;
	
	public ArmorBuff(int armorgain){
		potency = armorgain;
		turnsRemaining = -1;
		helpful = true;
	}
	public ArmorBuff(int armorgain, int turns){
		potency = armorgain;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffArmor<potency){
				player.buffArmor = (int)potency;
			}
		}else{
			if(enemy.buffArmor<potency){
				enemy.buffArmor = (int)potency;
			}
		}
		return returnDelay;
	}
}
