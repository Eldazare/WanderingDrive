using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuffAbstraction {

	public BuffType buffType;
	public float potency; // Magnitude
	public int duration; // in encounter


	public WorldBuffAbstraction(BuffType type, float magnitude, int duration){
		this.buffType = type;
		this.potency = magnitude;
		this.duration = duration;
	}
}
