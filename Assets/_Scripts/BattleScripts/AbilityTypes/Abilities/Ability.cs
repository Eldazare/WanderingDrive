using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability{
	public int staminaCost;
	public Enemy enemy;
	public PlayerCombatScript player;
	public virtual void UseAbility(){}
}
