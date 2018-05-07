using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {

    public GameObject groundButton;
    public List<Sprite> treasures;
    public List<List<GameObject>> soilGrid = new List<List<GameObject>>();
    public List<List<GameObject>> partials = new List<List<GameObject>>();
    public int horizontal = 18;
    public int vertical = 14;
    public int howMany;
    public int depth;
	void Start () {
        howMany = 252;
        for (int i = 0; i < vertical; i++) {
            List<GameObject> newList = new List<GameObject>();
            List<GameObject> newList2 = new List<GameObject>();
            soilGrid.Add(newList);
            partials.Add(newList2);
            for (int j = 0; j < horizontal; j++) {
                soilGrid[i].Add(null);
                partials[i].Add(null);
            }
        }
        Populate();
        AddTreasures();
	}

    public void Populate() {
        for (int i = 0; i < vertical; i++) {
            for(int j = 0; j < horizontal; j++) {
                GameObject newObject = Instantiate(groundButton, transform);
                newObject.name = i + "" + j;
                soilGrid[i][j] = newObject;
            }
            
        }

    }

    public void AddTreasures() {
        soilGrid[0][5].GetComponent<OreElement>().treasure = treasures[0];
        soilGrid[0][6].GetComponent<OreElement>().treasure = treasures[1];
        soilGrid[0][5].GetComponent<OreElement>().treasureExists = true;
        soilGrid[0][6].GetComponent<OreElement>().treasureExists = true;
        partials[0][0] = soilGrid[0][5];
        partials[0][1] = soilGrid[0][6];
        soilGrid[1][5].GetComponent<OreElement>().treasure = treasures[0];
        soilGrid[1][6].GetComponent<OreElement>().treasure = treasures[1];
        soilGrid[1][5].GetComponent<OreElement>().treasureExists = true;
        soilGrid[1][6].GetComponent<OreElement>().treasureExists = true;
        partials[1][0] = soilGrid[1][5];
        partials[1][1] = soilGrid[1][6];
    }
}
