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
    private List<RecipeMaterial> items;
	private List<RecipeMaterial> result;
    public List<List<GameObject>> soilGrid = new List<List<GameObject>>();
    public List<List<GameObject>> treasures = new List<List<GameObject>>();
    public int horizontal = 18;
    public int vertical = 14;
    public int howMany;
    public int depth;
    public bool usingHammer = false;

	public void StartTheMinigame(List<RecipeMaterial> maxMats){
		chosenSpot = new GameObject();
		howMany = 252;
		for (int i = 0; i < vertical; i++) {
			List<GameObject> newList = new List<GameObject>();
			List<GameObject> newList2 = new List<GameObject>();
			for (int j = 0; j < horizontal; j++) {
				newList.Add(null);
				newList2.Add(null);
			}
			soilGrid.Add(newList);
			treasures.Add(newList2);
		}
		Populate();
		AddTreasures(items);
		items = maxMats;
	}

    private void Populate() {
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

    private void AddTreasures(List<RecipeMaterial> tavarat) {
        //tämä randomisoidaan myöhemmin
        soilGrid[0][5].GetComponent<OreElement>().treasure = treasureSprites[0];
        soilGrid[0][6].GetComponent<OreElement>().treasure = treasureSprites[1];
        soilGrid[0][5].GetComponent<OreElement>().treasureExists = true;
        soilGrid[0][6].GetComponent<OreElement>().treasureExists = true;
        treasures[0][0] = soilGrid[0][5];
        treasures[0][1] = soilGrid[0][6];
        soilGrid[11][8].GetComponent<OreElement>().treasure = treasureSprites[0];
        soilGrid[11][9].GetComponent<OreElement>().treasure = treasureSprites[1];
        soilGrid[11][8].GetComponent<OreElement>().treasureExists = true;
        soilGrid[11][9].GetComponent<OreElement>().treasureExists = true;
        treasures[1][0] = soilGrid[11][8];
        treasures[1][1] = soilGrid[11][9];
        soilGrid[13][2].GetComponent<OreElement>().treasure = treasureSprites[0];
        soilGrid[13][3].GetComponent<OreElement>().treasure = treasureSprites[1];
        soilGrid[13][2].GetComponent<OreElement>().treasureExists = true;
        soilGrid[13][3].GetComponent<OreElement>().treasureExists = true;
        treasures[2][0] = soilGrid[13][2];
        treasures[2][1] = soilGrid[13][3];
    }

    public void ChangeToHammer() {
        usingHammer = true;
    }

    public void ChangeToPickAxe() {
        usingHammer = false;
    }

    public void CountTreasures() {
		result = new List<RecipeMaterial> ();
        for(int i = 0; i < 3; i++) {
			Debug.Log ("I: " + i);
            if(treasures[i][0].GetComponent<OreElement>().treasureUp == true || treasures[i][1].GetComponent<OreElement>().treasureUp == true) {
				result.Add(items[i]);
            }
        }
		StartCoroutine (WaitBeforeEnd ());
    }

	private IEnumerator WaitBeforeEnd(){
		yield return new WaitForSeconds(0.8f);
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().EndGatheringGame (result);
	}

	public void ExitFromGathering(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().EndGatheringGame (new List<RecipeMaterial>());
	}

	public void ReturnFromGathering() {
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().EndGatheringGame (result);
	}
}
