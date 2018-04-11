using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Buff {

	public Stun(){
		turnsRemaining = -1;
	}
	public Stun(int turns){
		turnsRemaining = turns;
	}
	public override void DoYourThing(){
		if(player != null){
			player.stunned = true;
		}else{
			enemy.stunned = true;
		}
	}
}
