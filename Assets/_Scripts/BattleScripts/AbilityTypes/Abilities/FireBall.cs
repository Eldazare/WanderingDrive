using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Ability{

	public int damage, elementDamage;
	public float potency;
	public Element element;
	public Enemy enemy;

	public FireBall(){
	}

	override public void Initialize(PlayerCombatScript _player){
		this.player = _player;
		string[] datas = dataString.Split ("/".ToCharArray());
		// TODO: Set
	}

	override public void UseAbility(){
		player.GetComponent<AbilityController>().AbilityEffect("FireBall");
		player.playerStats.stamina -= staminaCost;
		player.UpdateStats();
	}
}
