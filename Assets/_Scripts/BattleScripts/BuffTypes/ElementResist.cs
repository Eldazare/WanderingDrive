using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResist : _Buff {
	public List<int> buffElementalWeakness;

	float returnDelay = 0;
	public ElementResist(List<int> resists){
		buffElementalWeakness = resists;
		turnsRemaining = -1;
		helpful = true;
	}
	public ElementResist(List<int> resists, int turns){
		buffElementalWeakness = resists;
		turnsRemaining = turns;
		helpful = true;
	}
	override public float DoYourThing(){
		if(player != null){
			for(int i = 0;i<player.buffElementalWeakness.Count;i++){
				if(player.buffElementalWeakness[i]<buffElementalWeakness[i]){
					player.buffElementalWeakness[i] = buffElementalWeakness[i];
				}
			}
		}else{
			for(int i = 0;i<enemy.buffElementalWeakness.Count;i++){
				if(enemy.buffElementalWeakness[i]<buffElementalWeakness[i]){
					enemy.buffElementalWeakness[i] = buffElementalWeakness[i];
				}
			}
		}
		return returnDelay;
	}
}
