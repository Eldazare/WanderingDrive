using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material  {

    public int itemId;
    public int itemType;

 public Material(int ItemID, int ItemType) {
        this.itemId = ItemID;
    }

public int GetItemtype() {
        return itemType;
    }

public int GetItemID() {
        return itemId;
    }

     
}
