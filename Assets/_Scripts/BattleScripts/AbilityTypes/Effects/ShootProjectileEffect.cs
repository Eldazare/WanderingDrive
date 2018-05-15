using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : EffectType {
	public float projSpeed = 5;
	override public void StartEffect(){
		InvokeRepeating("MoveToEnemy", 0, Time.deltaTime);
	}
	public void MoveToEnemy(){
		if(Vector3.Distance(player.menuController.targetedEnemy.transform.position, transform.position)>0.1){
			transform.Translate(((player.menuController.targetedEnemy.transform.position-transform.position)+(player.menuController.targetedEnemy.transform.position-transform.position).normalized)*Time.deltaTime*projSpeed);
		}else{
			player.proceed = true;
			//Instantiate(Resources.Load("CombatResources/SpellEffects/FireExplosion"));
			Destroy(gameObject);
		}
	}
}
