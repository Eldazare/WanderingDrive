using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : Buff {
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
			player.combatController.HitEnemy(damageovertime, damageovertimeElement, element, 0, 0, 0);
		}else{
			enemy.combatController.HitEnemy(damageovertime, damageovertimeElement, element, 0, 0, 0);
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
