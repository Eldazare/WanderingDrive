using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuffAbstraction {

	public BuffType buffType;
	public string subType; // "Subtype" of buff, ex. Element in dots and resists. Parse to enum when generating actual object of buff.
	public float potency; // Magnitude
	public int duration; // in encounter


	public WorldBuffAbstraction(BuffType type, string subtype, float magnitude, int duration){
		this.buffType = type;
		this.subType = subtype;
		this.potency = magnitude;
		this.duration = duration;
	}
}
