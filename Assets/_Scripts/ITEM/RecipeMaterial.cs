using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMaterial  {

	public ItemType type; 
	public ItemSubType subtype; 
    public int itemId;
	public string itemName;
	public int amount;



    public RecipeMaterial(string identifier)
    {
        string[] matArr = identifier.Split("_".ToCharArray());
        type = (ItemType)System.Enum.Parse(typeof(ItemType), matArr[0]);
        subtype = (ItemSubType)System.Enum.Parse(typeof(ItemSubType), matArr[1]);
        itemId = int.Parse(matArr[2]);
        if (matArr.Length > 3)
        {
            amount = int.Parse(matArr[3]);
        }
        else
        {
            amount = 1;
        }
        itemName = NameDescContainer.GetName((NameType)System.Enum.Parse(typeof(NameType), subtype.ToString()), itemId);
    }

    public RecipeMaterial(ItemType type, ItemSubType subtype, int id) {
        this.type = type;
        this.subtype = subtype;
        this.itemId = id;
        this.amount = 1;
    }


	public string GetIdentifier(){
		return type.ToString() + "_" + subtype.ToString() + "_" + itemId;
	}

	public string GetName(){
		return itemName;
	}
}
