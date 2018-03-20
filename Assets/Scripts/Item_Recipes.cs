using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Recipes : MonoBehaviour {

    int Recipe_id;
    List<int> list;
    int newItem;

    public Item_Recipes(List<int> _list, int _id, int newItem)
    {
        this.list = _list;
        this.Recipe_id = _id;
        this.newItem = newItem;

    }
}
