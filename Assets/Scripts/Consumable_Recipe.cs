using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_Recipe : Recipe {

    public Consumable_Recipe(List<int> _list, int _id, int newItem) : base(_list, _id, newItem)
    {
        this.list = _list;
        this.Recipe_id = _id;
        this.newItem = newItem;

    }
}
