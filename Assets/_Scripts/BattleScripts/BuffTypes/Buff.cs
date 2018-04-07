using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff {
	public PlayerStats player;
	public int turnsRemaining;
	public abstract void DoYourThing();
}