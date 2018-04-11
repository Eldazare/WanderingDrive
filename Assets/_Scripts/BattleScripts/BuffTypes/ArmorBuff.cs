using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff {
	int armor;
	public ArmorBuff(int armorgain){
		armor = armorgain;
		turnsRemaining = -1;
	}
	public ArmorBuff(int armorgain, int turns){
		armor = armorgain;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			if(player.buffArmor<armor){
				player.buffArmor = armor;
			}
		}else{
			if(enemy.buffArmor<armor){
				enemy.buffArmor = armor;
			}
		}
	}
}
