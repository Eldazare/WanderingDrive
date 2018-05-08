using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : CombatConsumable  {
	override public void ActivateCombatConsumable() {
		float healing = potency;
		if(player){
			player.playerStats.health += potency;
			if(player.playerStats.health > player.playerStats.maxHealth){
				healing = potency - (player.playerStats.health - player.playerStats.maxHealth);
				player.playerStats.health = player.playerStats.maxHealth;
			}
			player.PopUpText(healing.ToString("0.#"),PlayerPopUpColor.Healing);
			player.UpdateStats();
		}
	}
}
