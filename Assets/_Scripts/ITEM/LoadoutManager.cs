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
    public GameObject lSlot1, lSlot2, lSlot3, lSlot4, lSlot5, lSlot6, lSlot7, lSlot8, lSlot9, lSlot10;
    public GameObject storeButton;
    public GameObject acces1, acces2, head, chest, arms, legs, boots, cons1, cons2, cons3, cons4, mainH, offH;
    public RecipeMaterial currentMaterial;
    Loadout myLoadout = new Loadout(2);
    public bool emptyLoadout;
    public List<Sprite> spriteList = new List<Sprite>();
	List<List<InventoryArmor>> armorLists;
	/*
    List<InventoryArmor> armorList = Inventory.inventoryArmor;
    List<InventoryArmor> accessoryList = new List<InventoryArmor>();
    List<InventoryArmor> helmList = new List<InventoryArmor>();
    List<InventoryArmor> chestList = new List<InventoryArmor>();
    List<InventoryArmor> armsList = new List<InventoryArmor>();
    List<InventoryArmor> legsList = new List<InventoryArmor>();
    List<InventoryArmor> bootsList = new List<InventoryArmor>();
    */
	public List<InventoryWeapon> weaponList;
	List<List<int>> consumables;

	public List<int> combatConsumables;
	public List<int> universalConsumables;

    //public List<InventoryArmor> currentList;
	private int currentArmorIndex = -1;
    public int currentItem;
    public int counter;
    public int chosenHand;
    public int chosenConsSlot;
    public int chosenAccessorySlot;
    ItemSubType currentItemSubType;
    ItemType currentItemType;

    void Start(){
        emptyLoadout = true;
		weaponList = Inventory.inventoryWeapons;
		consumables = Inventory.inventoryConsumables;
		universalConsumables = Inventory.inventoryConsumables [(int)ConsumableType.ConsumableUniversal];
		combatConsumables = Inventory.inventoryConsumables [(int)ConsumableType.ConsumableCombat];
        currentItem = -1;
        chosenHand = -1;
        counter = -1;
        //Sprites to recources and load
		armorLists = new List<List<InventoryArmor>>();
		for (int i = 0; i < System.Enum.GetNames (typeof(ArmorType)).Length; i++) {
			armorLists.Add (new List<InventoryArmor> ());
		}
		foreach (InventoryArmor invArmor in Inventory.inventoryArmor) {
			ArmorType parsedEnum = (ArmorType)System.Enum.Parse(typeof(ArmorType), invArmor.subType);
			armorLists [(int)parsedEnum].Add (invArmor);
        }
    }

    public void ChangeItemOnwards() {
        counter++;
		switch (currentItemType) {
		case ItemType.Wep:
			if (counter >= weaponList.Count) {
				counter = 0;
			}
			currentItem = weaponList[counter].itemID;
			currentItemSubType = (ItemSubType)System.Enum.Parse (typeof(ItemSubType), weaponList [counter].subType);
			break;
		case ItemType.Cons:
			List<int> consList = consumables [(int)System.Enum.Parse (typeof(ConsumableType), currentItemSubType.ToString ())];
			if (counter >= consList.Count) {
				counter = 0;
			}
			currentItem = consList[counter];
			break;
		case ItemType.Arm:
			if (counter >= armorLists [currentArmorIndex].Count) {
				counter = 0;
			}
			currentItem = armorLists [currentArmorIndex] [counter].itemID;
			break;
		default:
			Debug.LogError ("FUCK");
			break;
		}
        UpdateInfoTexts();
    }

    public void ChangeItemBackwards() {
		counter--;
		switch (currentItemType) {
		case ItemType.Wep:
			if (counter < 0) {
				counter = weaponList.Count - 1;
			}
			currentItem = weaponList[counter].itemID;
			currentItemSubType = (ItemSubType)System.Enum.Parse (typeof(ItemSubType), weaponList [counter].subType);
			break;
		case ItemType.Cons:
			if (counter < 0) {
				counter = combatConsumables.Count - 1;
			}
			currentItem = combatConsumables [counter];
			break;
		case ItemType.Arm:
			if (counter < 0) {
				counter = armorLists [currentArmorIndex].Count - 1;
			}
			currentItem = armorLists[currentArmorIndex][counter].itemID;
			break;
		default:
			Debug.LogError ("FUCK");
			break;
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
		currentItemSubType = ItemSubType.ConsumableCombat;
        currentItem = combatConsumables[0];
        chosenConsSlot = slot;
        counter = 0;
        UpdateInfoTexts();
    }

	public void ChangeConsType(){
		currentItemType = ItemType.Cons;
		if (currentItemSubType != ItemSubType.ConsumableCombat) {
			currentItemSubType = ItemSubType.ConsumableCombat;
		} else {
			currentItemSubType = ItemSubType.ConsumableUniversal;
		}
	}

    public void ChangeAccessorySlot(int slot) {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Accessory;
		currentArmorIndex = (int)ArmorType.Accessory;
        chosenAccessorySlot = slot;
        counter = 0;
		if (armorLists[currentArmorIndex].Count > 0) {
			currentItem = armorLists[currentArmorIndex][0].itemID;
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }
    }

	// Possibly use ArmorType id (int) as parameter? (Instead of itemSubtype)
    public void ChangeArmorSlot(int armorTypeGiven) {
        currentItemType = ItemType.Arm;
		currentItemSubType = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), ((ArmorType)armorTypeGiven).ToString());
        counter = 0;
		currentArmorIndex = armorTypeGiven;
		if (armorLists[currentArmorIndex].Count > 0) {
			currentItem = armorLists[currentArmorIndex][0].itemID;
            UpdateInfoTexts();
        }
        else {
            ClearInfoTexts();
        }

    }


    private void UpdateInfoTexts() {
		Debug.Log ("currentItem: "+currentItem);
        currentMaterial = new RecipeMaterial(currentItemType, currentItemSubType, currentItem);
		Debug.Log (currentMaterial.GetName ());
        itemInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);
    }

    private void ClearInfoTexts() {
        itemInfo.text = "~Empty~";
        //Tai jotain muuta järkevämpää?
    }

    public void AddToLoadout() {
        if (emptyLoadout == true) {
            emptyLoadout = false;
            storeButton.SetActive(true);
        }
        if (currentItemType == ItemType.Wep) {
            if (chosenHand == 1) {
                myLoadout.AddMainHand(weaponList[counter]);
            }
            else if (chosenHand == 0) {
                myLoadout.AddOffHand(weaponList[counter]);
            }
        }
        else if (currentItemType == ItemType.Cons) {
			myLoadout.AddCombatConsumable(chosenConsSlot, (int)System.Enum.Parse(typeof(ConsumableType), currentItemSubType.ToString()) , combatConsumables.IndexOf(currentItem));
        }
        else if (currentItemType == ItemType.Arm) {
            if (currentItemSubType != ItemSubType.Accessory) {
				myLoadout.AddArmor(armorLists[currentArmorIndex][counter]);
            }
            else if (currentItemSubType == ItemSubType.Accessory) {
				myLoadout.AddAccessory(armorLists[currentArmorIndex][counter], chosenAccessorySlot);
            }
        }
        UpdateColors();
    }

    public void UpdateColors() {
        if (myLoadout.mainHand != null) {
            mainH.GetComponent<Image>().color = Color.green;
        }
        if (myLoadout.offHand != null) {
            offH.GetComponent<Image>().color = Color.green;
        }
        if (myLoadout.wornAccessories[0] != null) {
            acces1.GetComponent<Image>().color = Color.green;
        }
        if (myLoadout.wornAccessories[1] != null) {
            acces2.GetComponent<Image>().color = Color.green;
        }
        for (int i = 0; i < myLoadout.wornArmor.Length; i++) {
            if (myLoadout.wornArmor[i] != null) {
                switch (i) {
                    case 0:
                        head.GetComponent<Image>().color = Color.green;
                        break;
                    case 1:
                        arms.GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        chest.GetComponent<Image>().color = Color.green;
                        break;
                    case 3:
                        legs.GetComponent<Image>().color = Color.green;
                        break;
                    case 4:
                        boots.GetComponent<Image>().color = Color.green;
                        break;
                }
            }

        }
        for (int i = 0; i < myLoadout.combatConsumableIndexes.Count; i++) {
            if (myLoadout.combatConsumableIndexes[i] != null) {
                switch (i) {
                    case 0:
                        cons1.GetComponent<Image>().color = Color.green;
                        break;
                    case 1:
                        cons2.GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        cons3.GetComponent<Image>().color = Color.green;
                        break;
                    case 3:
                        cons4.GetComponent<Image>().color = Color.green;
                        break;
                }
            }
        }
    }

    public void Confirm() {
        loadoutSlotPanel.SetActive(true);
        LoadoutsContainer myContainer = GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().loadoutList;
        for(int i = 0; i < myContainer.GetLoadoutCount(); i++) {
            if(myContainer.GetLoadout(i) != null) {
                switch (i) {
                    case 0:
                        lSlot1.GetComponent<Image>().color = Color.green;
                        break;
                    case 1:
                        lSlot2.GetComponent<Image>().color = Color.green;
                        break;
                    case 2:
                        lSlot3.GetComponent<Image>().color = Color.green;
                        break;
                    case 3:
                        lSlot4.GetComponent<Image>().color = Color.green;
                        break;
                    case 4:
                        lSlot5.GetComponent<Image>().color = Color.green;
                        break;
                    case 5:
                        lSlot6.GetComponent<Image>().color = Color.green;
                        break;
                    case 6:
                        lSlot7.GetComponent<Image>().color = Color.green;
                        break;
                    case 7:
                        lSlot8.GetComponent<Image>().color = Color.green;
                        break;
                    case 8:
                        lSlot9.GetComponent<Image>().color = Color.green;
                        break;
                    case 9:
                        lSlot10.GetComponent<Image>().color = Color.green;
                        break;
                }
            }
        }
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
