using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold : Buff {

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
