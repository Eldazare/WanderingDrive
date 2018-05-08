using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour {

    public GameObject groundButton;
    public GameObject chosenSpot;
    public GameObject collapsedPanel;
    public Image crack;
    public List<Sprite> treasureSprites;
    public List<int> itemIndexes;
    public List<List<GameObject>> soilGrid = new List<List<GameObject>>();
    public List<List<GameObject>> treasures = new List<List<GameObject>>();
    public int horizontal = 18;
    public int vertical = 14;
    public int howMany;
    public int depth;
    public bool usingHammer = false;
    void Start () {
        chosenSpot = new GameObject();
        howMany = 252;
        for (int i = 0; i < vertical; i++) {
            List<GameObject> newList = new List<GameObject>();
            List<GameObject> newList2 = new List<GameObject>();
            soilGrid.Add(newList);
            treasures.Add(newList2);
            for (int j = 0; j < horizontal; j++) {
                soilGrid[i].Add(null);
                treasures[i].Add(null);
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
                newObject.GetComponent<OreElement>().row = i;
                newObject.GetComponent<OreElement>().column = j;
                soilGrid[i][j] = newObject;
            }
            
        }

    }

    public void AddTreasures() {
        //tämä randomisoidaan myöhemmin
        soilGrid[0][5].GetComponent<OreElement>().treasure = treasureSprites[0];
        soilGrid[0][6].GetComponent<OreElement>().treasure = treasureSprites[1];
        soilGrid[0][5].GetComponent<OreElement>().treasureExists = true;
        soilGrid[0][6].GetComponent<OreElement>().treasureExists = true;
        soilGrid[0][5].GetComponent<OreElement>().treasureID = 0;
        soilGrid[0][6].GetComponent<OreElement>().treasureID = 0;
        treasures[0][0] = soilGrid[0][5];
        treasures[0][1] = soilGrid[0][6];
        soilGrid[11][8].GetComponent<OreElement>().treasure = treasureSprites[0];
        soilGrid[11][9].GetComponent<OreElement>().treasure = treasureSprites[1];
        soilGrid[11][8].GetComponent<OreElement>().treasureExists = true;
        soilGrid[11][9].GetComponent<OreElement>().treasureExists = true;
        soilGrid[11][8].GetComponent<OreElement>().treasureID = 0;
        soilGrid[11][9].GetComponent<OreElement>().treasureID = 0;
        treasures[1][0] = soilGrid[11][8];
        treasures[1][1] = soilGrid[11][9];
    }

    public void ChangeToHammer() {
        usingHammer = true;
    }

    public void ChangeToPickAxe() {
        usingHammer = false;
    }

    public void CountTreasures() {
        for(int i = 0; i < treasures.Count; i++) {
            switch(treasures[i][0].GetComponent<OreElement>().treasureID) {
                case 0:
                if(treasures[i].Count == 2) {
                    itemIndexes[0] += 1;
                }
                    break;
            }
        }
    }
}
