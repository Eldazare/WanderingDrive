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
    public int treasureID;
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
        if(y + 1 < 14) {
            mineGrid.soilGrid[y + 1][x].GetComponent<OreElement>().Dig();
        }
        if(y - 1 > -1) {
            mineGrid.soilGrid[y - 1][x].GetComponent<OreElement>().Dig();
        }
        if(x + 1 < 18) {
            mineGrid.soilGrid[y][x + 1].GetComponent<OreElement>().Dig();
        }
        if(x - 1 > -1) {
            mineGrid.soilGrid[y][x - 1].GetComponent<OreElement>().Dig();
        }
        if(y + 1 < 14 && x + 1 < 18) {
            mineGrid.soilGrid[y + 1][x + 1].GetComponent<OreElement>().Dig();
        }
        if (y - 1 > -1 && x - 1 > -1) {
            mineGrid.soilGrid[y - 1][x - 1].GetComponent<OreElement>().Dig();
        }
        if(y - 1 > -1 && x + 1 < 18) {
            mineGrid.soilGrid[y - 1][x + 1].GetComponent<OreElement>().Dig();
        }
        if(y + 1 < 14 && x - 1 > -1) {
            mineGrid.soilGrid[y + 1][x - 1].GetComponent<OreElement>().Dig();
        }
        mineGrid.soilGrid[y][x].GetComponent<OreElement>().Dig();
        mineGrid.usingHammer = true;
    }

    public void Crack() {
        if(mineGrid.usingHammer == true) {
            mineGrid.crack.fillAmount += 0.06f;
        }
        else {
            mineGrid.crack.fillAmount += 0.01f;
        }
        if(mineGrid.crack.fillAmount == 1) {
            mineGrid.collapsedPanel.SetActive(true);
            mineGrid.collapsedPanel.GetComponent<GetTreasures>().ShowTreasures();
        }
    }
}
