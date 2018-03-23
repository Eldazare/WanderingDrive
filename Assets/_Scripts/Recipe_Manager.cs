using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe_Manager  {

    public Recipe GetRecipeData(int RecipeType, int RecipeID)
    {
        //hae data
        List<_material> list = new List<_material>();  // tämän tilalle data managerin tiedot
        Recipe recipe = new Recipe(list, 4, 5);   //tässä myös
        return recipe;
    }

	
}
