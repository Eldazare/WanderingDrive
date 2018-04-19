using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload : _Buff {

	public Overload(int turns){
		turnsRemaining = turns;
	}

	override public float DoYourThing(){
		return 0;
	}
}
