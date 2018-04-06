using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff{
	public PlayerCombatScript player;
	public int turnsRemaining;

	public int encountersRemaining;
	public abstract void DoYourThing();
	
}
