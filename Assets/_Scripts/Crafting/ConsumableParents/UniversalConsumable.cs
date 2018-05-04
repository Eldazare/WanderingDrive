using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalConsumable : Consumable {

	public override void ActivateCombatConsumable (){
		UniversalUse();
	}

	public override void ActivateDungeonConsumable (){
		UniversalUse ();
	}

	public override void ActivateWorldConsumable (){
		UniversalUse ();
	}

	virtual
	protected void UniversalUse(){
		Debug.LogError ("Fug");
	}
}
