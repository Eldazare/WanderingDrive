using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OreElement : MonoBehaviour
{
    public Image myImage;
    public Sprite secondSoil;
    public Sprite bottomSoil;
    public int depth;

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
            myImage.sprite = bottomSoil;
            depth = 2;
        }
        
    }
}
