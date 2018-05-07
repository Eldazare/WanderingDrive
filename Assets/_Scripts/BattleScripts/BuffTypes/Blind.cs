using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Blind : _Buff{
	float returnDelay = 1f;
	public Blind(float blind){
		helpful = false;
		potency = blind;
		turnsRemaining = -1;
	}
	public Blind(float blind, int turns){
		helpful = false;
		potency = blind;
		turnsRemaining = turns;
	}
	
	override public float DoYourThing(){
		if(player != null){
			if(player.blind<potency){
				player.blind = potency;
				player.StatusTextPopUp("Blinded");
			}
		}else{
			if(enemy.blind<potency){
				enemy.blind = potency;
				enemy.StatusTextPopUp("Blinded");
			}
		}
		return returnDelay;
	}
}
