using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Consumable {

	// Values set by creatorScript
	public ConsumableType type;
	public int id;
	public float potency;

	// Values for individual consumable types
    protected PlayerCombatScript player;
	public void SetConsumablePlayer(PlayerCombatScript _player){
		player = _player;
	}

	virtual
	public void ActivateConsumable(){
		Debug.LogError ("Undefined action (activating item)");
	}

	protected bool ReduceInventoryAmount(){
		ItemSubType typeAsItemSubtype = (ItemSubType)System.Enum.Parse (typeof(ItemSubType), type.ToString ());
		return Inventory.RemoveItem (ItemType.Cons, typeAsItemSubtype, id, 1);
	}
}
