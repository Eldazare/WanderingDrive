using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Blind : _Buff{
	float blinding;  //0-100
	public Blind(float blind){
		blinding = blind;
		turnsRemaining = -1;
	}
	public Blind(float blind, int turns){
		blinding = blind;
		turnsRemaining = turns;
	}
	
	override public void DoYourThing(){
		if(player != null){
			if(player.blind<blinding){
				player.blind = blinding;
			}
		}else{
			if(enemy.blind<blinding){
				enemy.blind = blinding;
			}
		}
	}
}
