using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadoutManager : MonoBehaviour {

    public Image itemImage;
    public Text itemInfo;
    public RecipeMaterial currentMaterial;
    public Loadout myLoadout = new Loadout(2);

    public List<Sprite> spriteList = new List<Sprite>();
    List<InventoryArmor> armorList = Inventory.inventoryArmor;
    List<InventoryArmor> accessoryList = new List<InventoryArmor>();
    List<InventoryArmor> helmList = new List<InventoryArmor>();
    List<InventoryArmor> chestList = new List<InventoryArmor>();
    List<InventoryArmor> armsList = new List<InventoryArmor>();
    List<InventoryArmor> legsList = new List<InventoryArmor>();
    List<InventoryArmor> bootsList = new List<InventoryArmor>();
    public List<InventoryWeapon> weaponList = Inventory.inventoryWeapons;
    public List<int> combatConsumables = Inventory.combatConsumables;


    public int currentItem;
    public int counter;
    public int chosenHand;
    ItemSubType currentItemSubType;
    ItemType currentItemType;

    void Start () {
        currentItem = -1;
        chosenHand = -1;
        counter = -1;
        //Sprites to recources and load
        for (int i = 0; i <= armorList.Count; i++) {
            ItemSubType parsed_enum = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), armorList[i].subType);
            switch (parsed_enum) {
                case ItemSubType.Accessory:
                    accessoryList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
                case ItemSubType.Arms:
                    armsList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
                case ItemSubType.Boots:
                    bootsList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
                case ItemSubType.Chest:
                    chestList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
                case ItemSubType.Helm:
                    helmList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
                case ItemSubType.Legs:
                    legsList.Add(armorList[i]);
                    armorList.RemoveAt(i);
                    break;
            }       
        }		
	}

    public void ChangeItemOnwards() {
        counter++;
        if (currentItemType == ItemType.Wep) {
            if (counter >= weaponList.Count) {
                counter = 0;
                currentItem = weaponList[0].itemID;
            }
            else {
                currentItem = weaponList[counter].itemID;
            }
        }
        else {
            switch (currentItemSubType) {
                case ItemSubType.Accessory:
                    if (counter >= accessoryList.Count) {
                        counter = 0;
                        currentItem = accessoryList[0].itemID;
                    }
                    else {
                        currentItem = accessoryList[counter].itemID;
                    }
                    break;
                case ItemSubType.Arms:
                    if (counter >= armsList.Count) {
                        counter = 0;
                        currentItem = armsList[0].itemID;
                    }
                    else {
                        currentItem = armsList[counter].itemID;
                    }
                    break;
                case ItemSubType.Boots:
                    if (counter >= bootsList.Count) {
                        counter = 0;
                        currentItem = bootsList[0].itemID;
                    }
                    else {
                        currentItem = bootsList[counter].itemID;
                    }
                    break;
                case ItemSubType.Chest:
                    if (counter >= chestList.Count) {
                        counter = 0;
                        currentItem = chestList[0].itemID; }
                    else {
                        currentItem = chestList[counter].itemID;
                    }
                    break;
                case ItemSubType.ComCon:
                    if (counter >= combatConsumables.Count) {
                        counter = 0;
                        currentItem = combatConsumables[0];
                    }
                    else {
                        currentItem = combatConsumables[counter];
                    }
                    break;
                case ItemSubType.Helm:
                    if (counter >= helmList.Count) {
                        counter = 0;
                        currentItem = helmList[0].itemID;
                    }
                    else {
                        currentItem = helmList[counter].itemID;
                    }
                    break;
                case ItemSubType.Legs:
                    if (counter >= legsList.Count) {
                        counter = 0;
                        currentItem = legsList[0].itemID;
                    }
                    else {
                        currentItem = legsList[counter].itemID;
                    }
                    break;
                default:
                    Debug.Log("Error, unsuitable parameters");
                    break;
            }
        }
        UpdateInfoTexts();
    }

    public void ChangeItemBackwards() {
        counter--;
        if (currentItemType == ItemType.Wep) {
            if (counter < 0) {
                currentItem = weaponList[0].itemID;
            }
            else {
                currentItem = weaponList[counter].itemID;
            }
        }
        else {
            switch (currentItemSubType) {
                case ItemSubType.Accessory:
                    if (counter < 0) {
                        currentItem = accessoryList[accessoryList.Count -1].itemID;
                        counter = accessoryList.Count - 1;
                    }
                    else {
                        currentItem = accessoryList[counter].itemID;
                    }
                    break;
                case ItemSubType.Arms:
                    if (counter < 0) {
                        currentItem = armsList[armsList.Count - 1].itemID;
                        counter = armsList.Count - 1;
                    }
                    else {
                        currentItem = armsList[counter].itemID;
                    }
                    break;
                case ItemSubType.Boots:
                    if (counter < 0) {
                        currentItem = bootsList[bootsList.Count - 1].itemID;
                        counter = bootsList.Count - 1;
                    }
                    else {
                        currentItem = bootsList[counter].itemID;
                    }
                    break;
                case ItemSubType.Chest:
                    if (counter < 0) {
                        currentItem = chestList[chestList.Count - 1].itemID;
                        counter = chestList.Count - 1;
                    }
                    else {
                        currentItem = chestList[counter].itemID;
                    }
                    break;
                case ItemSubType.ComCon:
                    if (counter < 0) {
                        currentItem = combatConsumables[combatConsumables.Count -1];
                        counter = combatConsumables.Count - 1;
                    }
                    else {
                        currentItem = combatConsumables[counter];
                    }
                    break;
                case ItemSubType.Helm:
                    if (counter < 0) {
                        currentItem = helmList[helmList.Count - 1].itemID;
                        counter = helmList.Count - 1;
                    }
                    else {
                        currentItem = helmList[counter].itemID;
                    }
                    break;
                case ItemSubType.Legs:
                    if (counter < 0) {
                        currentItem = legsList[legsList.Count - 1].itemID;
                        counter = legsList.Count - 1;
                    }
                    else {
                        currentItem = legsList[counter].itemID;
                    }
                    break;
                default:
                    Debug.Log("Error, unsuitable parameters");
                    break;
            }
        }
        UpdateInfoTexts();
    }

    public void ChangeWeaponSlot(int slot) {
        currentItemType = ItemType.Wep;
        ItemSubType parsed_enum = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), weaponList[0].subType);
        currentItemSubType = parsed_enum;
        currentItem = weaponList[0].itemID;
        chosenHand = slot;
        counter = 0;
        UpdateInfoTexts();
    }

    public void ChangeArmorSlot(int _itemSubtype) {
        currentItemType = ItemType.Arm;
        currentItemSubType = (ItemSubType)_itemSubtype;
        counter = 0;
            switch (currentItemSubType) {
                case ItemSubType.Accessory:
                    currentItemSubType = ItemSubType.Accessory;
                    currentItem = accessoryList[0].itemID;
                    break;
                case ItemSubType.Arms:
                    currentItemSubType = ItemSubType.Arms;
                    currentItem = armorList[0].itemID;
                    break;
                case ItemSubType.Boots:
                    currentItemSubType = ItemSubType.Boots;
                    currentItem = bootsList[0].itemID;
                    break;
                case ItemSubType.Chest:
                    currentItemSubType = ItemSubType.Chest;
                    currentItem = chestList[0].itemID;
                    break;
                case ItemSubType.ComCon:
                    currentItemSubType = ItemSubType.ComCon;
                    currentItem = combatConsumables[0];
                    break;
                case ItemSubType.Helm:
                    currentItemSubType = ItemSubType.Helm;
                    currentItem = helmList[0].itemID;
                    break;
                case ItemSubType.Legs:
                    currentItemSubType = ItemSubType.Legs;
                    currentItem = legsList[0].itemID;
                    break;
                default:
                    Debug.Log("Error, unsuitable parameters");
                    break;
            }
        UpdateInfoTexts();
    }


    private void UpdateInfoTexts() {
        currentMaterial = new RecipeMaterial(currentItemType, currentItemSubType, currentItem);
        itemInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);     
    }

    public void AddToLoadout() {
        if(currentItemType == ItemType.Wep) {
            if(chosenHand == 1) {
                myLoadout.AddMainHand(weaponList[counter]);
            }
            
        }

        Debug.Log(myLoadout.mainHand.subType);
    }
}
