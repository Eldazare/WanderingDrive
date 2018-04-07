using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuffAbstraction {

	public BuffType buffType;
	public int duration; // in encounter


	public WorldBuffAbstraction(BuffType type, int duration){
		this.buffType = type;
		this.duration = duration;
	}
}
