using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController2 : MonoBehaviour {

	public string Name = "Unnamed";
	public int HP = 100;
	public int AP = 10;

	public Wat whoa;
	//public Enemy e;


	// Use this for initialization
	void Start () {
		whoa = new Wat(100, 10);
		//e = new Enemy (Name, HP, AP);

		whoa.AlterEx1 (-10);
		//e.GetHit (10);


	}

	// Update is called once per frame
	void Update () {
		
	}
}
