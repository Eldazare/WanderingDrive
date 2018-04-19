using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour {

	// object that exists in TheWorld, that is used to spawn (local) and control the nodes
	// Contains php script addresses.... maybe
	// Dunno what kind of database we should use

	public List<GameObject> localNodeList;
	public float trueRadius;
	public int nodeCount;

	private int minTime = 260;
	private int maxTime = 600;

	private static List<int> nodeTypeWeightList;
	private static List<List<int>> nodeTypesWeights;


	public static void InitializeWeights(){
		nodeTypeWeightList = new List<int> ();
		string[] typeWeights = DataManager.ReadDataString ("Node_TypeWeights").Split("/".ToCharArray());
		foreach (string str in typeWeights) {
			nodeTypeWeightList.Add (int.Parse (str));
		}
		int nodeTypeAmount = DataManager.ReadDataInt ("Node_NodeTypeAmount");
		nodeTypesWeights = new List<List<int>> ();
		for (int i = 0; i < nodeTypeAmount; i++) {
			List<int> nodeTypeWeight = new List<int> ();
			string[] weightArr = DataManager.ReadDataString ("Node_Weights_" + i).Split ("/".ToCharArray ());
			foreach (string str in weightArr) {
				nodeTypeWeight.Add (int.Parse (str));
			}
			nodeTypesWeights.Add (nodeTypeWeight);
		}
	}


	public void LoadNodes(double currentLatitude, double currentLongitude){
		foreach (GameObject go in localNodeList){
			Destroy (go);
		}
		localNodeList = new List<GameObject> ();
		LocalNodeGeneration (currentLatitude, currentLongitude);
		// TODO: Generate nodes for this lat & long (in database)
		// TODO: Get nodes for this lat & long (from database)
		/*
		string[] echoRows = "0;0;0;0;0\n0;0;0;0;0".Split("\n".ToCharArray());
		foreach (string dataString in echoRows) {
			string[] echoData = dataString.Split(";".ToCharArray());
			float nodeLatitude = float.Parse (echoData [0]);
			float nodeLongitude = float.Parse (echoData [1]);
			int nodeType = int.Parse (echoData [2]);
			int nodeId = int.Parse (echoData [3]);
			int nodeTime = int.Parse (echoData [4]);
			SpawnNode (nodeType, nodeId, nodeLatitude, nodeLongitude, nodeTime);
		}
		*/
	}


	// LOCAL
	private void SpawnNode(int nodeType, int id, double latitude, double longitude, int time){
		Sprite theSprite = NodeSpriteContainer.GetSprite (nodeType, id);
		GameObject node = Instantiate(new GameObject(), new Vector3(0,0,0), Quaternion.identity) as GameObject;
		node.AddComponent<SpriteRenderer> ().sprite = theSprite;
		WorldNode wNode = node.AddComponent<WorldNode> ();
		wNode.nodeType = nodeType;
		wNode.id = id;
		wNode.latitude = latitude;
		wNode.longitude = longitude;
		localNodeList.Add (node);
		// TODO: World to local position
	}

	private void LocalNodeGeneration(double localLatitude, double localLongitude){
		for (int i = 0; i < nodeCount; i++) {
			float locX = Random.Range (-1.0f, 1.0f);
			float locY = Random.Range (-1.0f, 1.0f);
			float locDist = Random.Range (0.0f, 1.0f);
			Vector2 tempVector = new Vector2 (locX, locY).normalized * locDist * trueRadius;
			double nodeLat = tempVector.x + localLatitude;
			double nodeLong = tempVector.y + localLongitude;

			int nodeType = GetRandomIndexByWeight (nodeTypeWeightList);
			int nodeID = GetRandomIndexByWeight (nodeTypesWeights [nodeType]);
			int nodeTime = Random.Range (minTime, maxTime);
			SpawnNode (nodeType, nodeID, nodeLat, nodeLong, nodeTime);
		}
	}

	private int GetRandomIndexByWeight(List<int> weights){
		int count = weights.Count;
		int total = 0;
		int calc = 0;
		foreach (int wInt in weights) {
			total += wInt;
		}
		int randomInt = Random.Range (0, total);
		for (int i = 0; i < count; i++) {
			calc += weights [i];
			if (calc > randomInt) {
				return i;
			}
		}
		Debug.LogError ("RandomIndexByWeight went over...");
		return -1;
	}
}
