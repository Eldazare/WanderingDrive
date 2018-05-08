using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : _Buff {

	float returnDelay = 1f;
	public Hold (){
		turnsRemaining = -1;
	}
	public Hold (int turns){
		turnsRemaining = turns;
	}
	public override float DoYourThing(){
		if(player != null){
			player.hold = true;
			player.PopUpText("Hold", PlayerPopUpColor.Status);
		}else{
			enemy.hold = true;
			enemy.StatusTextPopUp("Hold");
		}
		return returnDelay;
	}
}
