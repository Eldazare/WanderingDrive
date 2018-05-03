using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {

    public GameObject groundSprite;
    public int howMany;
    public int depth;
	void Start () {
        howMany = 63;
        Populate();
	}

    public void Populate() {
        GameObject newObject;
        for (int i = 0; i < howMany; i++) {
            switch (depth) {
                case 4:
                    newObject = (GameObject)Instantiate(groundSprite, transform);
                    break;
            }
            
        }

    }
}
