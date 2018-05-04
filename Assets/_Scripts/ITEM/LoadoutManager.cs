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
    public GameObject storeButton;
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
	public List<int> combatConsumables;

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
		combatConsumables = Inventory.inventoryConsumables [(int)ConsumableType.ComCon];
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
			if (counter >= combatConsumables.Count) {
				counter = 0;
			}
			currentItem = combatConsumables[counter];
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
        currentItemSubType = ItemSubType.ComCon;
        currentItem = combatConsumables[0];
        chosenConsSlot = slot;
        counter = 0;
        UpdateInfoTexts();
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
                itemImage = GameObject.FindGameObjectWithTag("MainH").GetComponent<Image>();
                itemImage.color = Color.green;
            }
            else if (chosenHand == 0) {
                myLoadout.AddOffHand(weaponList[counter]);
                itemImage = GameObject.FindGameObjectWithTag("OffH").GetComponent<Image>();
                itemImage.color = Color.green;
            }
        }
        else if (currentItemType == ItemType.Cons) {
            myLoadout.AddCombatConsumable(chosenConsSlot, combatConsumables.IndexOf(currentItem));
            itemImage = GameObject.FindGameObjectWithTag("cons" + chosenConsSlot.ToString()).GetComponent<Image>();
            itemImage.color = Color.green;
        }
        else if (currentItemType == ItemType.Arm) {
            if (currentItemSubType != ItemSubType.Accessory) {
				myLoadout.AddArmor(armorLists[currentArmorIndex][counter]);
                itemImage = GameObject.FindGameObjectWithTag(currentArmorIndex.ToString()).GetComponent<Image>();
                itemImage.color = Color.green;

            }
            else if (currentItemSubType == ItemSubType.Accessory) {
				myLoadout.AddAccessory(armorLists[currentArmorIndex][counter], chosenAccessorySlot);
                itemImage = GameObject.FindGameObjectWithTag("acces" + chosenAccessorySlot.ToString()).GetComponent<Image>();
                itemImage.color = Color.green;
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
