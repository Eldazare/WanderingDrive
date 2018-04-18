using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Focus : _Buff {

	public Focus(int turns){
		turnsRemaining = turns;
	}

	override public float DoYourThing(){
		return 0;
	}
}
