using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResist : Buff {
	public List<int> buffElementalWeakness;

	public ElementResist(List<int> resists){
		buffElementalWeakness = resists;
		turnsRemaining = -1;
	}
	public ElementResist(List<int> resists, int turns){
		buffElementalWeakness = resists;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		for(int i = 0;i<player.buffElementalWeakness.Count;i++){
			if(player.buffElementalWeakness[i]<buffElementalWeakness[i]){
				player.buffElementalWeakness[i] = buffElementalWeakness[i];
			}
		}
		if(turnsRemaining>0){
			turnsRemaining--;
		}
		if(turnsRemaining == 0){
			player.playerBuffs.Remove(this);
		}
	}
}
