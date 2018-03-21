using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Recipes : Recipe {


    public Item_Recipes(List<int> _list, int _id, int newItem) : base ( _list, _id, newItem)
    {
        this.list = _list;
        this.Recipe_id = _id;
        this.newItem = newItem;

    }


}
