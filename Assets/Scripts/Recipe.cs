using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe {

    public int Recipe_id;
    public List<_material> MaterialList;
    public int newItemType;
    public int newItemID;

    public Recipe(List<_material> _list, int _id, int newItem)
    {
        MaterialList = _list;
        Recipe_id = _id;
        newItemType = newItem;

    }

}
