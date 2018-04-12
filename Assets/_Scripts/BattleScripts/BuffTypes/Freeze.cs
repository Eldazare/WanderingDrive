using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Freeze : _Buff {

	public Freeze(){
		turnsRemaining = -1;
	}
	public Freeze(int turns){
		turnsRemaining = turns;
	}
	public override void DoYourThing(){
		if(player != null){
			player.frozen = true;
		}else{
			enemy.frozen = true;
		}
	}
}
