using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegen : _Buff {
	float heal;
	float returnDelay = 0;
	public HealthRegen(float regen){
		heal = regen;
		turnsRemaining = -1;
	}
	public HealthRegen(float regen, int turns){
		heal = regen;
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.healthRegen<heal){
				player.healthRegen = heal;
			}
		}else{
			if(enemy.healthRegen<heal){
				enemy.healthRegen = heal;
			}
		}
		return returnDelay;
	}
}
