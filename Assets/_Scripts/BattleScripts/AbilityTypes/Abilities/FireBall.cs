using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Ability{
	public FireBall(PlayerCombatScript _player){
		player = _player;
	}
	override public void UseAbility(){
		player.GetComponent<AbilityController>().AbilityEffect("FireBall");
	}
}
