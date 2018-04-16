using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : _Buff {
	int armor;
	float returnDelay = 0;
	public ArmorBuff(int armorgain){
		armor = armorgain;
		turnsRemaining = -1;
	}
	public ArmorBuff(int armorgain, int turns){
		armor = armorgain;
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.buffArmor<armor){
				player.buffArmor = armor;
			}
		}else{
			if(enemy.buffArmor<armor){
				enemy.buffArmor = armor;
			}
		}
		return returnDelay;
	}
}
