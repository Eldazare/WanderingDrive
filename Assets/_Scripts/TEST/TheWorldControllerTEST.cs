using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorldControllerTEST : MonoBehaviour {

	public void DoBattle(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<WorldNode> ().Interact ();
	}

	public void DoCraft(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().StartCrafting ();
	}

	public void DoUpgrade(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().StartUpgrading ();
	}
}
