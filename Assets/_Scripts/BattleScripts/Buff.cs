using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff  {
	public PlayerCombatScript player;
	public Enemy enemy;
	public int turnsRemaining;
	public abstract void DoYourThing();
}