using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeUpgrade {

	public EquipmentSubtype subtype;
	public int id;
	public RecipeMaterial baseEquipment;
	public List<RecipeMaterial> materialList;
	public RecipeMaterial result;

	public RecipeUpgrade(EquipmentSubtype recipeType){
		this.subtype = recipeType;
		materialList = new List<RecipeMaterial>{ };
	}
}
