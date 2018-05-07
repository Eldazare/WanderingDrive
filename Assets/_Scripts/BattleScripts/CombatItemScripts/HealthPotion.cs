using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : CombatConsumable  {
	override public void ActivateCombatConsumable() {
		if(player){
			player.playerStats.health += potency;
			if(player.playerStats.health > player.playerStats.maxHealth){
				potency = potency - (player.playerStats.health - player.playerStats.maxHealth);
				player.playerStats.health = player.playerStats.maxHealth;
			}
			player.PopUpText(potency.ToString("0.#"),false);
			player.UpdateStats();
		}
	}
}
