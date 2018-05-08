using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Freeze : _Buff {
	float returnDelay = 1f;
	public Freeze(){
		turnsRemaining = -1;
	}
	public Freeze(int turns){
		turnsRemaining = turns;
	}
	public override float DoYourThing(){
		if(player != null){
			player.frozen = true;
			player.PopUpText("Frozen", PlayerPopUpColor.Status);
		}else{
			enemy.frozen = true;
			enemy.StatusTextPopUp("Frozen");
		}
		return returnDelay;
	}
}
