using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confusion : Buff {
	public Confusion(){
		turnsRemaining = -1;
	}
	public Confusion(int turns){
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			player.confused = true;
		}else{
			enemy.confused = true;
		}
	}
}
