using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : _Buff {
	float damageovertime;
	float damageovertimeElement;
	Element element;
	public int damageType;
	public DamageOverTime(float damage, float damageElement, Element ele){
		damageovertime = damage;
		damageovertimeElement = damageElement;
		element = ele;
		turnsRemaining = -1;
	}
	public DamageOverTime(float damage,float damageElement, Element ele, int turns){
		damageovertime = damage;
		damageovertimeElement = damageElement;
		element = ele;
		turnsRemaining = turns;
	}
	override public void DoYourThing(){
		if(player != null){
			player.combatController.HitPlayer(damageovertime, damageovertimeElement, element, true, 0);
		}else{
			enemy.combatController.HitEnemy(damageovertime, damageovertimeElement, element, 0, 0, 0);
		}
		
	}
}
