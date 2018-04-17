using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : _Buff {
	float damageovertime; // = Potency
	Element element;
	public int damageType;
	float returnDelay = 1f;
	public DamageOverTime(float damage, Element ele){
		damageovertime = damage;
		element = ele;
		turnsRemaining = -1;
	}
	public DamageOverTime(float damage, Element ele, int turns){
		damageovertime = damage;
		element = ele;
		turnsRemaining = turns;
	}
	override public float DoYourThing(){
		if(player != null){
			player.combatController.HitPlayer(0,damageovertime, element, true, 1);
		}else{
			enemy.combatController.HitEnemy(0,damageovertime, element, 0, 0, 0);
		}
		return returnDelay;
	}
}
