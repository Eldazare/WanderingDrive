using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : _Buff {
	public Confusion(){
		turnsRemaining = -1;
	}
	public Confusion(int turns){
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		player.confused = true;
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
