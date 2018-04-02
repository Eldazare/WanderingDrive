using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropData : DropData {


	public List<DropData> partDropDatas;


	public EnemyDropData() : base(){
		partDropDatas = new List<DropData> ();
	}

	override
	public List<DropData> GetPartDrops() {
		try {
			return partDropDatas;
		} catch {
			return null;
		}
	
	}
}
