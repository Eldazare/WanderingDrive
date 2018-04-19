using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverFocus : _Buff {

	public OverFocus(int turns){
		turnsRemaining = turns;
	}

	override public float DoYourThing(){
		return 0;
	}
}
