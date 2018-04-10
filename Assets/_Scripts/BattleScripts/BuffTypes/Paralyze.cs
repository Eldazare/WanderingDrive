using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralyze : Buff {
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
