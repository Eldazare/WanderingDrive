using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableAbstraction {

	public ConsumableType type;
	public int index;

	public ConsumableAbstraction(ConsumableType conType, int index){
		this.type = conType;
		this.index = index;
	}
}
