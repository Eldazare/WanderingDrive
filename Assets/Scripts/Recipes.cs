using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe {

    int id;
    List<Crafting_Item> list;

    public Recipe(List<Crafting_Item> _list, int _id) {
        this.list = _list;
        this.id = _id;

    }

    public Item Merge(List<Crafting_Item> _list, int _id)
    {
        _list = list;
        if (id == 0) {
            if (true)
            {
                //Luo uusi tavara
            }

            Item _item = new Item();
            return _item;
        }
        if (id == 1)
        {
            if (true)
            {
                //Luo uusi tavara
            }

            Item _item = new Item();
            return _item;
        }

        return null;
    }

}
