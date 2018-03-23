using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _material  {

    public int ItemID;
    public int ItemType;

    public _material(int ItemID, int ItemType)
    {
        this.ItemID = ItemID;
    }

    public int GetItemtype()
    {
        return ItemType;
    }

    public int GetItemID()
    {
        return ItemID;
    }

     
}
