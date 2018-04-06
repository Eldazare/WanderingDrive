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
	public HealthRegen(float regen, int turns, int encounters){
		heal = regen;
		turnsRemaining = turns;
		encountersRemaining = encounters;
	}
	override public void DoYourThing(){
		player.playerStats.health+=heal;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerStats.playerBuffs.Remove(this);
		}
	}
	
}
