using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotDestroy : MonoBehaviour {


	public static DoNotDestroy self;


	// Use this for initialization
	void Awake () {
		if (self == null) {
			DontDestroyOnLoad (gameObject);
			self = this;
		} else if (self != this) {
			Destroy (gameObject);
		}
	}
	


}
