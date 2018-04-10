using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _Buff {
	public PlayerStats player;
	public int turnsRemaining;
	public abstract void DoYourThing();
}