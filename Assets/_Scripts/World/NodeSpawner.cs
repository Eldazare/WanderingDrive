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

	private List<int> nodeTypeWeightList = new List<int>(){35, 45, 10, 10};

	private static List<int> nodeType0Weights = new List<int>(){33,33,34};
	private static List<int> nodeType1Weights = new List<int>(){50, 50};
	private static List<int> nodeType2Weights = new List<int>(){50, 50};
	private static List<int> nodeType3Weights = new List<int>(){100};
	private List<List<int>> nodeTypesWeights 
		= new List<List<int>>(){nodeType0Weights, nodeType1Weights, nodeType2Weights, nodeType3Weights};

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
		// TODO: Resources.Load("NODEPREFAB PER TYPE AND ID);
		GameObject nodePrefab = new GameObject();
		GameObject node = Instantiate(nodePrefab, new Vector3(0,0,0), Quaternion.identity);
		localNodeList.Add (node);
		WorldNode wNode = node.AddComponent<WorldNode> ();
		wNode.nodeType = nodeType;
		wNode.id = id;
		wNode.latitude = latitude;
		wNode.longitude = longitude;
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
			int nodeID = GetRandomIndexByWeight (nodeTypesWeights [i]);
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
