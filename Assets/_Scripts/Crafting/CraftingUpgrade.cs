using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUpgrade : MonoBehaviour {

	public GameObject inventoryChoice; // Shows the choice of armor and weapon
	public GameObject inventoryResult; // shows the armors or weapons
	public Text recipeInfo;
	public Text resultInfo;

	private int recipeIndex;
	private List<RecipeUpgrade> upgradeList;
	private int recipeCount;
	// TODO: Choose equipment to upgrade

	private ItemType itemType;
	private ItemSubType itemSubtype;
	private bool itemSubtypeChosen = false;
	private int itemId;


	public void StoreItemType(int type){
		Debug.Log ("Got " + type);
		itemType = (ItemType)type;
		itemSubtypeChosen = false;
	}

	public void StoreEquipmentSubAndId(string subtype, int id){
		Debug.Log ("Got " + subtype + " and " + id);
		itemSubtype = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), subtype);
		itemId = id;
		itemSubtypeChosen = true;
	}

	public void GetUpgrades(){
		if (itemSubtypeChosen) {
			recipeIndex = 0;
			upgradeList = RecipeContainer.GetEquipmentUpgradeRecipes ((EquipmentSubtype)System.Enum.Parse(typeof(EquipmentSubtype), itemSubtype.ToString()), itemId);
			recipeCount = upgradeList.Count;
			if (upgradeList.Count != 0) {
				UpdateRecipeInfo ();
			} else {
				recipeInfo.text = "No Recipes for this";
				resultInfo.text = "";
			}
		} else {
			Debug.LogError ("No choices made");
		}
	}

	public void MoveRecipeRight(){
		recipeIndex++;
		if (recipeIndex >= recipeCount) {
			recipeIndex = 0;
		}
		UpdateRecipeInfo ();
	}

	public void MoveRecipeLeft(){
		recipeIndex--;
		if (recipeIndex < 0) {
			recipeIndex = recipeCount - 1;
		}
		UpdateRecipeInfo ();
	}

	private void UpdateRecipeInfo(){
		RecipeUpgrade recip = upgradeList [recipeIndex];
		recipeInfo.text = InfoBoxCreator.GetRecipeUpgradeInfoString (recip);
		resultInfo.text = InfoBoxCreator.GetMaterialInfoString (recip.result);
	}

	public void MergeUpgrade(){
		Merge.CombineUpgradeRecipe (upgradeList[recipeIndex]);
	}


	// TODO: Unify inventory abstraction?
	public void GenerateInventoryItemButtons(){
		foreach (Transform transf in inventoryResult.transform) {
			Destroy (transf.gameObject);
		}
		GameObject prefab = Resources.Load ("CraftingUi/Button") as GameObject;
		if (itemType == ItemType.Wep) {
			foreach (InventoryWeapon invWep in Inventory.inventoryWeapons) {
				GameObject button = Instantiate (prefab, inventoryResult.transform) as GameObject;
				Button but = button.GetComponent<Button> ();
				but.onClick.AddListener (delegate {
					StoreEquipmentSubAndId (invWep.subType, invWep.itemID);
				});
				button.GetComponentInChildren<Text> ().text = NameDescContainer.GetName ((NameType)System.Enum.Parse (typeof(NameType), invWep.subType.ToString ()), invWep.itemID);
			}
		} else if (itemType == ItemType.Arm) {
			foreach (InventoryArmor invArm in Inventory.inventoryArmor) {
				GameObject button = Instantiate (prefab, inventoryResult.transform) as GameObject;
				Button but = button.GetComponent<Button> ();
				but.onClick.AddListener (delegate {
					StoreEquipmentSubAndId (invArm.subType, invArm.itemID);
				});
				button.GetComponentInChildren<Text> ().text = NameDescContainer.GetName ((NameType)System.Enum.Parse (typeof(NameType), invArm.subType.ToString ()), invArm.itemID);
			}
		} else {
			Debug.Log ("False itemtype for creating buttons?");
		}
	}



	public void QuitCrafting(){
		GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject> ().EndCrafting ();
	}
}