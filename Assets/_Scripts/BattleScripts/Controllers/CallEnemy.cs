using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEnemy : MonoBehaviour {

	public Enemy enemy;

	public void Proceed(){
		enemy.proceed = true;
	}
}
