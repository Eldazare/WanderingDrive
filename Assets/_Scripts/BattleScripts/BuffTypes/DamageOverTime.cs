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
		AttackResult attack = new AttackResult(0, damageovertime, element, 100, 0);
		if(player != null){
			player.combatController.HitPlayer(attack);
		}else{
			enemy.combatController.HitEnemy(attack);
		}
		return returnDelay;
	}
}
