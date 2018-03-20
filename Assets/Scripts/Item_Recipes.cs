using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Recipes : MonoBehaviour {

    int id;
    List<int> list;
    Item newItem;

    public Item_Recipes(List<int> _list, int _id, Item newItem)
    {
        this.list = _list;
        this.id = _id;
        this.newItem = newItem;

    }
}
