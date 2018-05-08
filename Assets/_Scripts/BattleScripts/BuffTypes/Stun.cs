using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : _Buff {

	float returnDelay = 1f;
	public Stun(){
		turnsRemaining = -1;
	}
	public Stun(int turns){
		turnsRemaining = turns;
	}
	public override float DoYourThing(){
		if(player != null){
			player.stunned = true;
			player.PopUpText("Stunned", PlayerPopUpColor.Status);
		}else{
			enemy.stunned = true;
			enemy.StatusTextPopUp("Stunned");
		}
		return returnDelay;
	}
}
