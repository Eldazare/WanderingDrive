using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : _Buff {
	float returnDelay = 1f;
	public Confusion(){
		turnsRemaining = -1;
	}
	public Confusion(int turns){
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			player.confused = true;
			player.StatusTextPopUp("Confused");
		}else{
			enemy.confused = true;
			enemy.StatusTextPopUp("Confused");
		}
		return returnDelay;
	}
}
