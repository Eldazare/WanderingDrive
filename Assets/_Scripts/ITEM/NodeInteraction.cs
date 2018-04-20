using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeInteraction {

	private static UndyingObject theObject;
	private static bool initialized = false;
	private static int gatheringAmount;


	//TYPES: (INT because database cannot really store Enum)
	// 0 = Gather
	// 1 = Single small monster - Indexes same as small monsters
	// 2 = Multimonster (defined in config)
	// 3 = Big monster - Indexes same as big monsters
	// (4 = Dungeon)

	public static void InitializeUndyingObject (UndyingObject obj){
		theObject = obj;
		initialized = true;
		gatheringAmount = DataManager.ReadDataInt ("Gather_GatherNormalAmount");
	}

	public static void GiveInteractedNodeForward(GameObject go){
		theObject.interactedNode = go;
	}

	public static void HandleNodeInteraction(int type, int id){
		if (initialized) {
			switch (type) {
			case 0:
				theObject.GetGatherinNode (id, gatheringAmount);
				break;
			case 1:
				List<NodeEnemy> enemyList1 = new List<NodeEnemy> ();
				enemyList1.Add (new NodeEnemy (id, "EnemySmall"));
				theObject.CombatPrompt (enemyList1);
				break;
			case 2:
				List<NodeEnemy> enemyList2 = new List<NodeEnemy> ();
				// Note: undefinedin config?
				string[] enemyData = DataManager.ReadDataString ("Node_multimonster_" + id).Split ("_".ToCharArray ());
				foreach (string str in enemyData) {
					int eId = str [1];
					string eSubtype = "";
					switch (str [0]) {
					case 'S':
						eSubtype = "EnemySmall";
						break;
					case 'L':
						eSubtype = "EnemyLarge";
						break;
					default:
						Debug.LogError ("MultiBattle config contained false enemyType identifier (id: " + id + "), " + str [0]);
						return;
					}
					enemyList2.Add (new NodeEnemy (id, eSubtype));
				}
				theObject.CombatPrompt (enemyList2);
				break;
			case 3:
				List<NodeEnemy> enemyList3 = new List<NodeEnemy> ();
				enemyList3.Add (new NodeEnemy (id, "EnemyLarge"));
				theObject.CombatPrompt (enemyList3);
				break;
			case 4:
				Debug.Log ("Dungeon node noted");
				break;
			default:
				Debug.LogError ("FALSE node type identification given (" + type + ").");
				break;
			}
		} else {
			Debug.LogError ("Use of unitialized NodeInteraction");
		}
	}
}
