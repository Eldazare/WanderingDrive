using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPieceAbstraction  {

	public string type;
	public float aliveTime;

	public ComboPieceAbstraction(string type, float aliveTime){
		this.type = type;
		this.aliveTime = aliveTime;
	}
}
