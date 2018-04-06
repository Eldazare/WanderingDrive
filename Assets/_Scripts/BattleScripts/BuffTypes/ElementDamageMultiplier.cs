using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementDamageMultiplier : Buff {
	float multi;
	override public void DoYourThing(){
		player.playerStats.buffElementDamageMultiplier +=multi;
	}
}
