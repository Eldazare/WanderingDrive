using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaPotion : CombatConsumable  {
	override public void ActivateCombatConsumable() {
		float healing = potency;
		if(player){
			player.playerStats.stamina += potency;
			if(player.playerStats.stamina > player.playerStats.maxStamina){
				healing = potency - (player.playerStats.stamina - player.playerStats.maxStamina);
				player.playerStats.stamina = player.playerStats.maxStamina;
			}
			player.PopUpText(healing.ToString("0.#"),PlayerPopUpColor.Stamina);
			player.UpdateStats();
		}
	}
}
