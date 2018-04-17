using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Buff {
	public PlayerCombatScript player;
	public Enemy enemy;
	public int turnsRemaining;
	public float potency;
	public bool helpful;
	public virtual float DoYourThing(){return 0;}
}