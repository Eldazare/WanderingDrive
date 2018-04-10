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
