using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LoadoutManager : MonoBehaviour {

	// TODO: AddToLoadoutContainer? (Created loadout has to be stored somewhere)
	// TODO: Exit + ComCon buttons don't have onClick()


    public Image itemImage;
    public Text itemInfo;
	public Text itemInfoPage;
	public Text itemConsumableAmountInInventory;
    public GameObject loadoutSlotPanel;
    public GameObject [] containerSlots = new GameObject[10];
    public GameObject[] consSlots = new GameObject[4];
    public GameObject[] armorSlots = new GameObject[5];
    public GameObject storeButton;
	public GameObject changeConsumableButton;
    public GameObject acces1, acces2, mainH, offH;
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

	//public List<int> combatConsumables;
	//public List<int> universalConsumables;

    //public List<InventoryArmor> currentList;
	private int currentArmorIndex = -1;
	private int currentConsumableIndex = -1;
    public int currentItem;
    public int counter;
	public int currentMaxCount;
    public int chosenHand;
    public int chosenConsSlot;
    public int chosenAccessorySlot;
    ItemSubType currentItemSubType;
    ItemType currentItemType;
	private bool validItem = false;

    void Start(){
        emptyLoadout = true;
		weaponList = Inventory.inventoryWeapons;
		consumables = Inventory.inventoryConsumables;
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
			if (currentItem >= weaponList.Count) {
				counter = 0;
			}
			currentItem = weaponList[counter].itemID;
			currentItemSubType = (ItemSubType)System.Enum.Parse (typeof(ItemSubType), weaponList [counter].subType);
			break;
		case ItemType.Cons:
			List<int> consList = consumables [(int)System.Enum.Parse (typeof(ConsumableType), currentItemSubType.ToString ())];
			Debug.Log ("debug conslist count: "+consList.Count);
			if (counter >= consList.Count) {
				counter = 0;
			}
			currentItem = counter;
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
			List<int> consList = consumables [(int)System.Enum.Parse (typeof(ConsumableType), currentItemSubType.ToString ())];
			if (counter < 0) {
				counter = consList.Count - 1;
			}
			currentItem = consList[counter];
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
		currentMaxCount = weaponList.Count;
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
		currentItem = 0;
        chosenConsSlot = slot;
        counter = 0;
		currentMaxCount = consumables [(int)ConsumableType.ConsumableCombat].Count;
        UpdateInfoTexts();
    }

	public void ChangeConsType(){
		currentItemType = ItemType.Cons;
		if (currentItemSubType != ItemSubType.ConsumableCombat) {
			currentItemSubType = ItemSubType.ConsumableCombat;
		} else {
			currentItemSubType = ItemSubType.ConsumableUniversal;
		}
		counter = 0;
		currentMaxCount = consumables [(int)System.Enum.Parse (typeof(ConsumableType), currentItemSubType.ToString ())].Count;
		UpdateInfoTexts();
	}

    public void ChangeAccessorySlot(int slot) {
        currentItemType = ItemType.Arm;
        currentItemSubType = ItemSubType.Accessory;
		currentArmorIndex = (int)ArmorType.Accessory;
        chosenAccessorySlot = slot;
        counter = 0;
		currentMaxCount = armorLists [currentArmorIndex].Count;
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
		currentMaxCount = armorLists [currentArmorIndex].Count;
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
		CheckConsumableChangePossibility ();
		itemInfoPage.text = "Item: "+(counter+1) + "/" + currentMaxCount;
		itemConsumableAmountInInventory.text = "";

		// VV~~ ItemText and validItem set below ~~VV
		if (currentMaterial.GetName() == "default"){
			itemInfo.text = "Item not implemented. Sorry.";
			validItem = false;
			return;
		}
		if (currentItemType == ItemType.Cons) {
			int amountInInventoryCons = consumables [(int)System.Enum.Parse (typeof(ConsumableType), currentItemSubType.ToString ())] [currentItem];
			if (amountInInventoryCons == 0) {
				itemInfo.text = "Consumable not in inventory";
				validItem = false;
				return;
			} else {
				itemConsumableAmountInInventory.text = "In inventory: "+amountInInventoryCons;
			}
		} 
        itemInfo.text = InfoBoxCreator.GetMaterialInfoString(currentMaterial);
		validItem = true;
    }

    private void ClearInfoTexts() {
        itemInfo.text = "~Empty~";
		CheckConsumableChangePossibility ();
        //Tai jotain muuta järkevämpää?
    }

	private void CheckConsumableChangePossibility(){
		if (currentItemType == ItemType.Cons) {
			changeConsumableButton.SetActive (true);
		} else {
			changeConsumableButton.SetActive (false);
		}
	}

    public void AddToLoadout() {
		if (!validItem) {
			Debug.Log ("Invalid item, not added");
			return;
		}
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
			myLoadout.AddCombatConsumable(chosenConsSlot, (int)System.Enum.Parse(typeof(ConsumableType), currentItemSubType.ToString()) , currentItem);
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
                armorSlots[i].GetComponent<Image>().color = Color.green;
            }

        }
        for (int i = 0; i < myLoadout.combatConsumableIndexes.Count; i++) {
            if (myLoadout.combatConsumableIndexes[i] != null) {
                consSlots[i].GetComponent<Image>().color = Color.green;
            }
        }
    }

    public void Confirm() {
        loadoutSlotPanel.SetActive(true);
        LoadoutsContainer myContainer = GameObject.FindGameObjectWithTag("UndyingObject").GetComponent<UndyingObject>().loadoutList;
        for(int i = 0; i < myContainer.GetLoadoutCount(); i++) {
            if(myContainer.GetLoadout(i) != null) {
                containerSlots[i].GetComponent<Image>().color = Color.green;
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
