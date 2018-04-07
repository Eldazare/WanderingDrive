using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blind : Buff{
	float blinding;
	public Blind(){
		turnsRemaining = -1;
	}
	public Blind(int turns){
		turnsRemaining = turns;
	}
	
	override public void DoYourThing(){
		if(player.blind<blinding){
			player.blind = blinding;
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
