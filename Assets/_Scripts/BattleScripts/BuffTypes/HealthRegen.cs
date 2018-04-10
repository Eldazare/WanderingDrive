using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : Buff {
	float heal;
	public HealthRegen(float regen){
		heal = regen;
		turnsRemaining = -1;
	}
	public HealthRegen(float regen, int turns){
		heal = regen;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			if(player.healthRegen<heal){
				player.healthRegen = heal;
			}
		}else{
			if(enemy.healthRegen<heal){
				enemy.healthRegen = heal;
			}
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			if(player != null){
				player.playerBuffs.Remove(this);
			}else{
				enemy.enemyBuffs.Remove(this);
			}
		}
	}
}
