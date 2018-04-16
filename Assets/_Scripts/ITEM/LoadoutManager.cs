using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadoutManager : MonoBehaviour {

    public Image recipeImage;
    public Text recipeInfo;
    public Text resultInfo;
    public RecipeMaterial currentMaterial;

    public List<Sprite> spriteList = new List<Sprite>();

    public int currentItem;
    ItemSubType currentItemSubType;
    ItemType currentItemType;

    void Start () {
		
	}

    private void ChooseHead() {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Helm;
    }

    private void ChooseChest() {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Chest;
    }

    private void ChooseLegs() {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Legs;
    }

    private void ChooseArms() {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Arms;
    }

    private void ChooseAccessory() {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Accessory;
    }

    private void ChooseMainHand() {
        currentItemType = ItemType.Wep;
    }

    private void ChooseOffHand() {
        currentItemType = ItemType.Wep;
    }

    private void UpdateInfoTexts() {
        currentMaterial = new RecipeMaterial(currentItemType, currentItemSubType, currentItem);
        recipeInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);
        //resultInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);
    }
}
