﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;

using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.Location;

//using Mapbox.Examples;

public class MarkerScript : MonoBehaviour {

	/*
	This is heavily based on the "SpawnOnMap" script, that came with MapBoxSDK.
	*/
	public float SpawnRange = 0.001f;
	public float NonSpawnRange = 0.0f;
	float InitialNonSpawnRange = 0.0005f;
	public int NumberOfMarkers = 1;
	public float DespawnDistance = 100.0f;

	[SerializeField]
	GameObject _player;
	[SerializeField]
	AbstractMap _map;

	public List <Vector2d> _locations;
	public List <MarkerDataContainer> _storedMarkers;

	[SerializeField]
	GameObject _markerPrefab;
	[SerializeField]
	List<GameObject> _spawnedObjects;



	[SerializeField]
	List<GameObject> _mapTiles;

	UndyingObject theObject;


	//public List<GameObject> localNodeList;
	//public float trueRadius;
	//public int nodeCount;








	// Use this for initialization
	void Start () {






		SearchTiles ();
		//Invoke ("SearchTiles", 0.2f);
		theObject = GameObject.FindGameObjectWithTag ("UndyingObject").GetComponent<UndyingObject>();


		EditorLocationProvider ed = GameObject.Find ("EditorLocation").GetComponent<EditorLocationProvider> ();
		if (theObject.storedPlayerPosition.x != 0 && theObject.storedPlayerPosition.y != 0) {
			//Debug.Log ("REEEEEEEEEEEEEEEEEEEEEEEEEEEE");
			ed._latitudeLongitude = theObject.storedPlayerPosition.x + ", " + theObject.storedPlayerPosition.y;
		}



		if (_locations.Count == 0) {
			_locations = new List<Vector2d> ();
		}
		_storedMarkers = new List<MarkerDataContainer>();
		theObject.GetLastMarkers (_storedMarkers);
		if (_storedMarkers.Count > 0) {
			
			for (int i = 0; i < _storedMarkers.Count; i++) {
				_locations.Add (_storedMarkers [i]._latlong);
			}
		}


		_spawnedObjects = new List<GameObject>();
		if (_locations.Count == 0) {
			Debug.Log ("Empty _locations list");
			_locations = new List<Vector2d> ();
			Invoke ("NewPlop", 0.8f);
			//Invoke ("PlopMarkers", 0.8f);
		} else {
			Debug.Log ("NOT empty _locations list");
			Invoke ("storedPlop", 0.8f);
		}
			
		// It appears that the markers will not end up where they're supposed to, unless you invoke with +0.4 seconds.
		// I don't know why this happens but if I had to guess, the map is probably doing some loading so the prefabs don't spawn quite in the right place.

		InvokeRepeating ("DistanceCheck", 1.0f, 1.0f);


		Debug.Log ("LayerType: " + _map.VectorData.LayerType.ToString());



	}

	// Searches for the map tiles.
	// If it can't find all 9 of them, it tries again. Because apparently if the tiles are still loading, it can't find any of them. 
	void SearchTiles () {
		_mapTiles = new List<GameObject>(GameObject.FindGameObjectsWithTag("MapTile"));

		if (_mapTiles.Count >= 9) {
			Debug.Log ("All tiles accounted for.");
			//NewPlop ();
		} else {
			Invoke ("SearchTiles", 0.01f);
		}
	}


	// Update on PlopMarkers. Spawns 5 nodes per map tile. Currently only used for the initial plop, as it needs some work.
	void NewPlop(){
		SearchTiles ();
		_locations.Clear ();
		_storedMarkers.Clear ();


		if (_spawnedObjects.Count < NumberOfMarkers) {
			List<MarkerDataContainer> temp = new List<MarkerDataContainer> ();
			//int missingMarkers = NumberOfMarkers - _spawnedObjects.Count;
			foreach (GameObject tile in _mapTiles) {


				//Debug.Log ("kid count: " + tile.transform.childCount);

				if (tile.transform.childCount < (NumberOfMarkers / 9)) {
					for (int i = 0; i < ((NumberOfMarkers / 9) - tile.transform.childCount); i++) {
						float boundX = tile.GetComponent<MeshRenderer> ().bounds.size.x / 2;
						float boundZ = tile.GetComponent<MeshRenderer> ().bounds.size.z / 2;

						Vector3 spawnPos = tile.transform.position + new Vector3 (Random.Range (-boundX, boundX), 0, Random.Range (-boundZ, boundZ));

						Vector2d vec = _map.WorldToGeoPosition (spawnPos);
						_locations.Add (vec);
					}
				}

			}

			for (int i = 0; i < _locations.Count; i++) {
				SpawnNode (_locations [i], -1);
			}


			for (int i = 0; i < _spawnedObjects.Count; i++) {
				MarkerDataContainer mark = new MarkerDataContainer ();

				mark._nodeType = _spawnedObjects [i].GetComponent<WorldNode> ().nodeType;
				mark._id = _spawnedObjects [i].GetComponent<WorldNode> ().id;
				mark._time = _spawnedObjects [i].GetComponent<WorldNode> ().time;
				mark._latlong = _map.WorldToGeoPosition (_spawnedObjects [i].transform.position);

				temp.Add (mark);
				//temp.Add (_map.WorldToGeoPosition (_spawnedObjects [i].transform.position));
			}


			Debug.Log ("ADDED MARKS: " + temp.Count);
			
			theObject.SetLastMarkers (temp);
			SetChildren ();
		}
	}


	void storedPlop(){
		for (int i = 0; i < _locations.Count; i++) {
			SpawnNode (_storedMarkers[i]._latlong, i);
		}
		SetChildren ();
	}

	// "Plops" markers in place.
	public void PlopMarkers () {

		float nonRange = InitialNonSpawnRange;
		_locations.Clear ();
		_storedMarkers.Clear ();

		if (_spawnedObjects.Count < NumberOfMarkers) {
			
			// Adding coordinates to a list
			int l = _spawnedObjects.Count;
			for (int i = 0; i < (NumberOfMarkers - l); i++) {

				// Randomly generates X and Y coordinates between negative spawn range and spawn range
				// For instance, -0.001 and 0.001. The numbers are lat-long wise.
				Vector2d vec = _map.WorldToGeoPosition (_player.transform.position);
				double randomX = (double)Random.Range (-SpawnRange, SpawnRange);
				double randomY = (double)Random.Range (-SpawnRange, SpawnRange);


				// Calculates distance between the player and the marker coordinates (before instansiation), using the pythagoran theorem
				// Unsure if this is the most efficient way to do this, but it doesn't appear to produce lag.
				float xD = Mathf.Pow ((float)randomX, 2);
				float zD = Mathf.Pow ((float)randomY, 2);
				float distance = Mathf.Sqrt (xD + zD);
				//Debug.Log ("Distance spawn:" + distance);



				// If the random coordinate is too close to the player, the algorithm adds a random amount of X and Y distances,
				// that total the minimum of NonSpawnRange. 
				if (distance < NonSpawnRange) {
					//Debug.Log ("Correcting...");
					double correctionX = Random.Range (0, nonRange);
					double correctionY = nonRange - correctionX;

					// Determines the "closest way out". Or at least its direction.
					int modifierX = 1;
					int modifierY = 1;
					if (randomX < 0) {
						modifierX = -1;
					}
					if (randomY < 0) {
						modifierY = -1;
					}

					// Corrected coordinates
					randomX += (correctionX * modifierX);
					randomY += (correctionY * modifierY);

				}

				// Shoves the coordinates into the _locations list
				vec.x += randomX; 
				vec.y += randomY;
				_locations.Add (vec);


			}

			// Instantiating a marker for each coordinate on _locations list and adding it to the _spawnedObjects list
			for (int i = 0; i < _locations.Count; i++) {


				//GameObject instance = Instantiate(_markerPrefab);
				//instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
				//_spawnedObjects.Add(instance);


				//SpawnNode (nodeType, nodeID, nodeLoc, nodeTime);
				SpawnNode (_locations[i], -1);
			}

			InitialNonSpawnRange = NonSpawnRange;


			List<MarkerDataContainer> temp = new List<MarkerDataContainer> ();
			for (int i = 0; i < _spawnedObjects.Count; i++) {
				MarkerDataContainer mark = new MarkerDataContainer ();

				mark._nodeType = _spawnedObjects [i].GetComponent<WorldNode> ().nodeType;
				mark._id = _spawnedObjects [i].GetComponent<WorldNode> ().id;
				mark._time = _spawnedObjects [i].GetComponent<WorldNode> ().time;
				mark._latlong = _map.WorldToGeoPosition (_spawnedObjects [i].transform.position);

				temp.Add (mark);
				//temp.Add (_map.WorldToGeoPosition (_spawnedObjects [i].transform.position));
			}
			//Debug.Log ("ADDED MARKS: " + temp.Count);
			theObject.SetLastMarkers (temp);

			//_locations.Clear ();
		}
	}




	// Checks the distance between the player and each marker on map.
	// This may not be the most efficient method.
	void DistanceCheck () {
		
		theObject.storedPlayerPosition = _map.WorldToGeoPosition (_player.transform.position);

		//Debug.Log ("Distance Check");

		// Removal list. To avoid objects from going out of range.
		List<GameObject> rem = new List<GameObject> ();

		int l = _spawnedObjects.Count;
		for (int i = 0; i < l; i++) {

			// Calculating the distance between the player and a marker
			float xDist = _player.transform.position.x - _spawnedObjects [i].transform.position.x;
			float zDist = _player.transform.position.z - _spawnedObjects [i].transform.position.z;
			float xD = Mathf.Pow (xDist, 2);
			float zD = Mathf.Pow (zDist, 2);
			float distance = Mathf.Sqrt (xD + zD);

			//Debug.Log ("Distance:" + distance);

			if (distance > DespawnDistance) {
				// Sets an object for removal.
				rem.Add (_spawnedObjects[i]);

			}
		}

		// Destroys and removes the marker from _spawnedObjects list.
		// Then creates new marker(s) to take their place.
		for (int i = 0; i < rem.Count; i++) {
			_spawnedObjects.Remove(rem[i]);
			Destroy (rem[i]);


			//PlopMarkers ();
			NewPlop();
		}
		rem.Clear ();


	}






	// LOCAL
	private void SpawnNode(Vector2d location, int index){

		//Sprite theSprite = NodeSpriteContainer.GetSprite (nodeType, id);
		//GameObject node = Instantiate(new GameObject(), new Vector3(0,0,0), Quaternion.identity) as GameObject;
		GameObject node = Instantiate(_markerPrefab);
		node.transform.localPosition = _map.GeoToWorldPosition(location);

		//node.AddComponent<SpriteRenderer> ().sprite = theSprite;
		WorldNode wNode = node.AddComponent<WorldNode> ();
		if (index >= 0) {
			//Debug.Log ("Stored plop");
			wNode.nodeType = _storedMarkers [index]._nodeType;
			wNode.id = _storedMarkers [index]._id;
			wNode.time = _storedMarkers [index]._time;
			wNode.latitude = _storedMarkers [index]._latlong.x;
			wNode.longitude = _storedMarkers [index]._latlong.y;
			wNode.GetNodeColor ();
		} else {
			//Debug.Log ("Random plops");
			NodeSpawner.WorldNodeRandoms (wNode);
			wNode.GetNodeColor ();
			wNode.latitude = location.x;
			wNode.longitude = location.y;
		}
		//localNodeList.Add (node);
		_spawnedObjects.Add(node);
		// TODO: World to local position
	}


	void SetChildren(){
		foreach(GameObject ob in _spawnedObjects){

			GameObject nearest = null;
			float storedDist = 1000000000000.0f;

			foreach (GameObject tile in _mapTiles) {
				float xDist = ob.transform.position.x - tile.transform.position.x;
				float zDist = ob.transform.position.z - tile.transform.position.z;
				float xD = Mathf.Pow (xDist, 2);
				float zD = Mathf.Pow (zDist, 2);
				float distance = Mathf.Sqrt (xD + zD);

				if (nearest == null || storedDist > distance) {
					nearest = tile;
					storedDist = distance;
				}
			}
			ob.transform.parent = nearest.transform;
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

