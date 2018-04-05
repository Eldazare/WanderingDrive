using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutsContainer {

	private int loadoutCount;
	private List<Loadout> loadoutList;

	public LoadoutsContainer(int loadoutCount){
		this.loadoutCount = loadoutCount;
		loadoutList = new List<Loadout> ();
		for (int i = 0; i < loadoutCount; i++) {
			loadoutList.Add (null);
		}
	}

	public void InsertLoadout (Loadout loadout, int index){
		if (index < loadoutCount && index >= 0) {
			loadoutList [index] = loadout;
		} else {
			Debug.LogError ("Invalid index given to LoadoutsContainer (" + index + ", loadoutCount was " + loadoutCount+").");
		}
	}

	public void RemoveLoadout (int index){
		if (index < loadoutCount && index >= 0) {
			loadoutList [index] = null;
		} else {
			Debug.LogError ("Invalid index given to LoadoutsContainer (" + index + ", loadoutCount was " + loadoutCount+").");
		}
	}

	public void Transfer(LoadoutsContainer lodCon){
		for (int i = 0; i<lodCon.loadoutCount;i++){
			this.loadoutList[i] = lodCon.loadoutList[i];
		}
	}

	public Loadout GetLoadout (int index){
		Loadout lod = loadoutList [index];
		if (lod == null){
			Debug.LogError ("LoadoutsContainer returned a null Loadout");
		}
		return lod;
	}

}
