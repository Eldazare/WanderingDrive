using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{

    public Image itemImage;
    public Text itemInfo;
    public RecipeMaterial currentMaterial;
    public Loadout myLoadout = new Loadout(2);

    public List<Sprite> spriteList = new List<Sprite>();
    List<List<InventoryArmor>> itemList = new List<List<InventoryArmor>>();
    List<InventoryArmor> armorList = Inventory.inventoryArmor;
    List<InventoryArmor> accessoryList = new List<InventoryArmor>();
    List<InventoryArmor> helmList = new List<InventoryArmor>();
    List<InventoryArmor> chestList = new List<InventoryArmor>();
    List<InventoryArmor> armsList = new List<InventoryArmor>();
    List<InventoryArmor> legsList = new List<InventoryArmor>();
    List<InventoryArmor> bootsList = new List<InventoryArmor>();
    public List<InventoryWeapon> weaponList = Inventory.inventoryWeapons;
    public List<int> combatConsumables = Inventory.combatConsumables;

    public List<InventoryArmor> currentList = new List<InventoryArmor>();
    public int currentItem;
    public int counter;
    public int chosenHand;
    public int chosenConsSlot;
    public int chosenAccessorySlot;
    ItemSubType currentItemSubType;
    ItemType currentItemType;

    void Start(){
        currentItem = -1;
        chosenHand = -1;
        counter = -1;
        //Sprites to recources and load
        for (int i = 0; i <= armorList.Count; i++) {
            ItemSubType parsed_enum = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), armorList[i].subType);
            switch (parsed_enum) {
                case ItemSubType.Accessory:
                    accessoryList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
                case ItemSubType.Arms:
                    armsList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
                case ItemSubType.Boots:
                    bootsList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
                case ItemSubType.Chest:
                    chestList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
                case ItemSubType.Helm:
                    helmList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
                case ItemSubType.Legs:
                    legsList.Add(armorList[i]);
                    //armorList.RemoveAt(i);
                    break;
            }
        }
        itemList.Add(accessoryList);
        itemList.Add(helmList);
        itemList.Add(chestList);
        itemList.Add(armsList);
        itemList.Add(legsList);
        itemList.Add(bootsList);

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
        else if (currentItemType == ItemType.Cons) {
            if (counter >= currentList.Count) {
                counter = 0;
                currentItem = combatConsumables[0];
            }
            else {
                currentItem = combatConsumables[counter];
            }
        }
        else {
            if (counter >= currentList.Count) {
                counter = 0;
                currentItem = currentList[0].itemID;
            }
            else {
                currentItem = currentList[counter].itemID;
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
        else if (currentItemType == ItemType.Cons) {
            if (counter < 0) {
                currentItem = combatConsumables[combatConsumables.Count - 1];
                counter = combatConsumables.Count - 1;
            }
            else {
                currentItem = combatConsumables[counter];
            }
        }
        else {
            if (counter < 0) {
                currentItem = currentList[currentList.Count - 1].itemID;
                counter = currentList.Count - 1;
            }
            else {
                currentItem = currentList[counter].itemID;
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
        if (weaponList.Count > 0) {
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }

    }

    public void ChangeConsSlot(int slot) {
        currentItemType = ItemType.Cons;
        currentItemSubType = ItemSubType.ComCon;
        currentItem = combatConsumables[0];
        chosenConsSlot = slot;
        counter = 0;
        UpdateInfoTexts();
    }

    public void ChangeAccessorySlot(int slot) {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Accessory;
        currentList = itemList[0];
        currentItem = currentList[0].itemID;
        chosenAccessorySlot = slot;
        counter = 0;
        if (currentList.Count > 0) {
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }
    }

    public void ChangeArmorSlot(int _itemSubtype) {
        currentItemType = ItemType.Arm;
        currentItemSubType = (ItemSubType)_itemSubtype;
        counter = 0;
        switch (currentItemSubType) {
            case ItemSubType.Arms:
                currentList = itemList[3];
                break;
            case ItemSubType.Boots:
                currentList = itemList[5];
                break;
            case ItemSubType.Chest:
                currentList = itemList[2];
                break;
            case ItemSubType.Helm:
                currentList = itemList[1];
                break;
            case ItemSubType.Legs:
                currentList = itemList[4];
                break;
            default:
                Debug.Log("Error, unsuitable parameters");
                break;
        }
        currentItem = currentList[0].itemID;
        if (currentList.Count > 0) {
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }

    }


    private void UpdateInfoTexts() {
        currentMaterial = new RecipeMaterial(currentItemType, currentItemSubType, currentItem);
        itemInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);
    }

    private void ClearInfoTexts() {
        itemInfo.text = "~Empty~";
        //Tai jotain muuta järkevämpää?
    }

    public void AddToLoadout() {
        if (currentItemType == ItemType.Wep) {
            if (chosenHand == 1) {
                myLoadout.AddMainHand(weaponList[counter]);
            }
            else if (chosenHand == 0) {
                myLoadout.AddOffHand(weaponList[counter]);
            }
        }
        else if (currentItemType == ItemType.Cons) {
            myLoadout.AddCombatConsumable(chosenConsSlot, currentItem);
        }
        else if (currentItemType == ItemType.Arm) {
            if (currentItemSubType != ItemSubType.Accessory) {
                myLoadout.AddArmor(currentList[counter]);
            }
            else if (currentItemSubType == ItemSubType.Accessory) {
                myLoadout.AddAccessory(currentList[counter], chosenAccessorySlot);
            }
        }
        Debug.Log(myLoadout.mainHand.subType);
    }
}
