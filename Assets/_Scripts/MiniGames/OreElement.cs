using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreElement : MonoBehaviour
{
    public Image myImage;
    public Sprite secondSoil;
    public Sprite bottomSoil;
    public Sprite treasure;
    public int depth;
    public bool treasureExists = false;
    public bool treasureUp = false;

    private void Start() {
        depth = 0;
        myImage = gameObject.GetComponent<Image>();
    }

    public void dig() {
        if (depth == 0) {
            myImage.sprite = secondSoil;
            depth = 1;
        }
        else if (depth == 1) {
            if(treasureExists == true) {
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
