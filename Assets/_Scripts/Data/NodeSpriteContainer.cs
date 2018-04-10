using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeSpriteContainer  {

	// Organized by type, then by ID
	private static List<Sprite[]> nodeSpriteData; 

	public static void LoadSpriteData(){
		string begin = "Sprites/Nodes/";
		for(int i = 0; i<5;i++){
			Sprite[] sprites = Resources.LoadAll (begin + "t" + i) as Sprite[];
			nodeSpriteData.Add (sprites);
		}
	}

	public static Sprite GetSprite(int type, int id){
		return nodeSpriteData [type] [id];
	}
}
