using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStatsContainer {

	public float health;
	public float stamina;

	private List<WorldBuffAbstraction> buffs;



	public WorldStatsContainer(){
		buffs = new List<WorldBuffAbstraction> ();
		health = 100;
		stamina = 100;
	}

	public void AddWorldBuff(WorldBuffAbstraction buff){
		buffs.Add (buff);
	}

	public List<WorldBuffAbstraction> GetWorldBuffs(){
		return buffs;
	}


	public void AfterEncounter(){
		// decrease buff duration
		foreach (WorldBuffAbstraction wba in buffs) {
			wba.duration -= 1;
			if (wba.duration <= 0) {
				buffs.Remove (wba);
			}
		}
	}
}
