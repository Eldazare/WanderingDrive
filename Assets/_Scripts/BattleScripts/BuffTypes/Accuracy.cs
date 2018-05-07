using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy : _Buff {

	float returnDelay = 0;
	
	public Accuracy(int accuracy){
		potency = accuracy;
		turnsRemaining = -1;
		helpful = true;
	}
	public Accuracy(int accuracy, int turns){
		potency = accuracy;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			if(player.accuracyBuff<potency){
				player.accuracyBuff = (int)potency;
			}
		}else{
		}
		return returnDelay;
	}
}
