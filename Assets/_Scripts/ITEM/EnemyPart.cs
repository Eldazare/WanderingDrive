using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart {

	public string name;
	public int percentageHit;
	public float damageMod;
	public float hp;
	public float maxHP;
	public bool broken = false;

	public bool DamageThisPart(float damage){
		hp -= damage;
		if (hp <= 0) {
			hp = 0;
			broken = true;
		}
		return broken;
	}
}
