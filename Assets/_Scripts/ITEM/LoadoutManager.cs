using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadoutManager : MonoBehaviour {

	// TODO: AddToLoadoutContainer? (Created loadout has to be stored somewhere)
	// TODO: Exit + ComCon buttons don't have onClick()


    public Image itemImage;
    public Text itemInfo;
    public GameObject loadoutSlotPanel;
    public RecipeMaterial currentMaterial;
    Loadout myLoadout = new Loadout(2);

    public List<Sprite> spriteList = new List<Sprite>();
	List<List<InventoryArmor>> itemList;
	/*
    List<InventoryArmor> armorList = Inventory.inventoryArmor;
    List<InventoryArmor> accessoryList = new List<InventoryArmor>();
    List<InventoryArmor> helmList = new List<InventoryArmor>();
    List<InventoryArmor> chestList = new List<InventoryArmor>();
    List<InventoryArmor> armsList = new List<InventoryArmor>();
    List<InventoryArmor> legsList = new List<InventoryArmor>();
    List<InventoryArmor> bootsList = new List<InventoryArmor>();
    */
    public List<InventoryWeapon> weaponList = Inventory.inventoryWeapons;
    public List<int> combatConsumables = Inventory.combatConsumables;

    public List<InventoryArmor> currentList;
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
		itemList = new List<List<InventoryArmor>>();
		for (int i = 0; i < System.Enum.GetNames (typeof(ArmorType)).Length; i++) {
			itemList.Add (new List<InventoryArmor> ());
		}
		foreach (InventoryArmor invArmor in Inventory.inventoryArmor) {
			ArmorType parsedEnum = (ArmorType)System.Enum.Parse(typeof(ArmorType), invArmor.subType);
			itemList [(int)parsedEnum].Add (invArmor);
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
        chosenHand = slot;
        counter = 0;
        if (weaponList.Count > 0) {
            ItemSubType parsed_enum = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), weaponList[0].subType);
            currentItemSubType = parsed_enum;
            currentItem = weaponList[0].itemID;
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
        chosenAccessorySlot = slot;
        counter = 0;
        if (currentList.Count > 0) {
            currentItem = currentList[0].itemID;
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }
    }

	// Possibly use ArmorType id (int) as parameter? (Instead of itemSubtype)
    public void ChangeArmorSlot(int _itemSubtype) {
        currentItemType = ItemType.Arm;
        currentItemSubType = (ItemSubType)_itemSubtype;
        counter = 0;
        Debug.Log(currentList);
		currentList = itemList[(int)System.Enum.Parse(typeof(ArmorType),currentItemSubType.ToString())];
        Debug.Log(currentList);
        if (currentList.Count > 0) {
            currentItem = currentList[0].itemID;
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }

    }


    private void UpdateInfoTexts() {
        currentMaterial = new RecipeMaterial(currentItemType, currentItemSubType, currentItem);
		Debug.Log (currentMaterial.GetName ());
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
    }

    public void Confirm() {
        loadoutSlotPanel.SetActive(true);
    }

    public void SaveLoadout(int slot) {
        LoadoutsContainer myContainer = GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().loadoutList;
        myContainer.InsertLoadout(myLoadout, slot);
        Cancel();
    }

    public void Cancel() {
        loadoutSlotPanel.SetActive(false);
    }

    public void Exit() {
        GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().EndCrafting();
    }

}
