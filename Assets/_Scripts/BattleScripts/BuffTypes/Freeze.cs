using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : Buff {

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
		
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			if(player != null){
				player.playerBuffs.Remove(this);
			}else{
				enemy.enemyBuffs.Remove(this);
			}
		}
	}
}
