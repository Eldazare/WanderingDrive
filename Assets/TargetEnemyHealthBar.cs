using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemyHealthBar : MonoBehaviour {

	public EnemyHealthBarScript hpbar;
	public MenuController menu;
	float damageTaken;
	void OnEnable(){
		UpdateBar(0);
	}
	public void UpdateBar(int part){

	}
}
