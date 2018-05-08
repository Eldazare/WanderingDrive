using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreElement : MonoBehaviour
{
    public PopulateGrid mineGrid;
    public Image myImage;
    public Sprite secondSoil;
    public Sprite bottomSoil;
    public Sprite treasure;
    public int depth;
    public int row, column;
    public bool treasureExists = false;
    public bool treasureUp = false;

    private void Start() {
        depth = 0;
        myImage = gameObject.GetComponent<Image>();
        mineGrid = GetComponentInParent<PopulateGrid>();
    }

    public void Dig() {
        if (mineGrid.usingHammer == true) {
            HitSoil();
        }
        else {
            if (depth == 0) {
                myImage.sprite = secondSoil;
                depth = 1;
            }
            else if (depth == 1) {
                if (treasureExists == true) {
                    myImage.sprite = treasure;
                    treasureUp = true;
                }
                else {
                    myImage.sprite = bottomSoil;
                }
                depth = 2;
            }
        }       
    }

    public void HitSoil() {
        mineGrid.usingHammer = false;
        mineGrid.chosenSpot = gameObject;
        int x = mineGrid.chosenSpot.GetComponent<OreElement>().column;
        int y = mineGrid.chosenSpot.GetComponent<OreElement>().row;
        mineGrid.soilGrid[y + 1][x].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y - 1][x].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y][x + 1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y][x -1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y + 1][x + 1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y - 1][x -1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y - 1][x + 1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y + 1][x - 1].GetComponent<OreElement>().Dig();
        mineGrid.soilGrid[y][x].GetComponent<OreElement>().Dig();
        mineGrid.usingHammer = true;
    }
}
