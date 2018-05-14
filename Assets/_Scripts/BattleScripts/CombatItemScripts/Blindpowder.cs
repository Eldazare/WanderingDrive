using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blindpowder : Consumable {

	override public void ActivateConsumable() {
		if(player){
			_Buff buff = new Blind(potency);
			buff.turnsRemaining = 5;
			buff.enemy = player.menuController.targetedEnemy;
			Debug.Log(buff.enemy);
			buff.enemy.enemyBuffList.Add(buff);
		}
	}
}
