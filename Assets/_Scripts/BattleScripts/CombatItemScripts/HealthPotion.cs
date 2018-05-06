using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : CombatItem  {

	public HealthPotion(PlayerCombatScript _player , float healing){
		player = _player;
		potency = healing;
		returnDelay = 1f;
	}
	override public float DoYourItemThing(){
		if(player){
			player.playerStats.health += potency;
			if(player.playerStats.health > player.playerStats.maxHealth){
				potency = potency - (player.playerStats.health - player.playerStats.maxHealth);
				player.playerStats.health = player.playerStats.maxHealth;
			}
			player.PopUpText(potency.ToString("0.#"),false);
			player.UpdateStats();
		}
		return returnDelay;
	}
}
