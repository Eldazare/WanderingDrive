using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPieceAbstraction  {

	public ComboPieceType type;
	public float aliveTime;

	public ComboPieceAbstraction(ComboPieceType type, float aliveTime){
		this.type = type;
		this.aliveTime = aliveTime;
	}
}
