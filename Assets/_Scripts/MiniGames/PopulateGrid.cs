using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {

    public GameObject groundSprite;
    public int howMany;
    public int depth;
	void Start () {
        howMany = 252;
        Populate();
	}

    public void Populate() {
        GameObject newObject;
        for (int i = 0; i < howMany; i++) {
            newObject = (GameObject)Instantiate(groundSprite, transform);            
        }

    }
}
