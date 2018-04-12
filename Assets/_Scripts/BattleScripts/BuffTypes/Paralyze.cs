using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralyze : _Buff {
	public Paralyze(){
		turnsRemaining = -1;
	}
	public Paralyze(int turns){
		turnsRemaining = turns;
	}
	public override void DoYourThing(){
		if(player != null){
			player.paralyzed = true;
		}else{
			enemy.paralyzed = true;
		}
	}
}
