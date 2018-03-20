using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_Recipe : MonoBehaviour {

    int id;
    List<int> list;
    Item newItem;

    public Consumable_Recipe(List<int> _list, int _id, Item newItem)
    {
        this.list = _list;
        this.id = _id;
        this.newItem = newItem;
    }
}
