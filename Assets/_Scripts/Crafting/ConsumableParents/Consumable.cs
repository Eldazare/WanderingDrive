using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Consumable {

	public ConsumableType type;
	public float potency;
    protected PlayerCombatScript player;

	public void SetConsumablePlayer(PlayerCombatScript _player){
		player = _player;
	}
	virtual
	public void ActivateCombatConsumable(){
		Debug.LogError ("Undefined action (activating item)");
	}

	virtual
	public void ActivateWorldConsumable(){
		Debug.LogError ("Undefined action (activating item)");
	}

	virtual
	public void ActivateDungeonConsumable(){
		Debug.LogError ("Undefined action (activating item)");
	}
}
