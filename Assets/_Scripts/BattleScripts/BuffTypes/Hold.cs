using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : _Buff {


	public Hold (){
		turnsRemaining = -1;
	}
	public Hold (int turns){
		turnsRemaining = turns;
	}
	public override void DoYourThing(){
		if(player != null){
			player.hold = true;
		}else{
			enemy.hold = true;
		}
	}
}
