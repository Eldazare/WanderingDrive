using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatItem{
	public float returnDelay;
	public PlayerCombatScript player;
	public Enemy enemy;
	public float potency;
	public bool helpful;
	public virtual float DoYourItemThing(){return 0;}
}
