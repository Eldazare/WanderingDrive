using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralyze : _Buff {

	float returnDelay = 1f;
	public Paralyze(){
		turnsRemaining = -1;
	}
	public Paralyze(int turns){
		turnsRemaining = turns;
	}
	public override float DoYourThing(){
		if(player != null){
			player.paralyzed = true;
			player.StatusTextPopUp("Paralyzed");
		}else{
			enemy.paralyzed = true;
			enemy.StatusTextPopUp("Paralyzed");
		}
		return returnDelay;
	}
}
