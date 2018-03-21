using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe {

    public int Recipe_id;
    public List<int> list;
    public int newItem;

    public Recipe(List<int> _list, int _id, int newItem)
    {
        this.list = _list;
        this.Recipe_id = _id;
        this.newItem = newItem;

    }

    public int GetData(int itemID)
    {

        return newItem;
    }


}
