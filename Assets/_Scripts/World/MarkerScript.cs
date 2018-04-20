using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;

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

	[SerializeField]
	GameObject _markerPrefab;
	[SerializeField]
	List<GameObject> _spawnedObjects;








	public List<GameObject> localNodeList;
	public float trueRadius;
	public int nodeCount;








	// Use this for initialization
	void Start () {



		//NodeSpriteContainer.LoadSpriteData ();

		_locations = new List<Vector2d> ();
		_spawnedObjects = new List<GameObject>();

		// It appears that the markers will not end up where they're supposed to, unless you invoke with +0.4 seconds.
		// I don't know why this happens but if I had to guess, the map is probably doing some loading so the prefabs don't spawn quite in the right place.
		Invoke ("InitialMarkerPlop", 0.8f);


		InvokeRepeating ("DistanceCheck", 1.0f, 1.0f);

	}




	public void InitialMarkerPlop () {

		//_locations.Clear ();
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
				Debug.Log ("Distance spawn:" + distance);

				if (distance < InitialNonSpawnRange) {

					// If the random coordinate is too close to the player, the algorithm adds a random amount of X and Y distances,
					// that total the minimum of NonSpawnRange. 
					Debug.Log ("Correcting...");
					double correctionX = Random.Range (0, InitialNonSpawnRange);
					double correctionY = InitialNonSpawnRange - correctionX;

					int modifierX = 1;
					int modifierY = 1;
					if (randomX < 0) {
						modifierX = -1;
					}
					if (randomY < 0) {
						modifierY = -1;
					}

					randomX += (correctionX * modifierX);
					randomY += (correctionY * modifierY);

				}


				vec.x += randomX; 
				vec.y += randomY;
				_locations.Add (vec);


			}

			// Instantiating a marker for each coordinate on list and adding it to the _spawnedObjects list
			for (int i = 0; i < _locations.Count; i++)
			{
				//GameObject instance = Instantiate(_markerPrefab);
				//instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i]);
				//_spawnedObjects.Add(instance);
				//SpawnNode (nodeType, nodeID, nodeLoc, nodeTime);
				SpawnNode (_locations[i]);
			}
		}
	}



	// "Plops" markers in place.
	public void PlopMarkers () {

		// _locations list needs to be clear for this.
		_locations.Clear ();

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
				Debug.Log ("Distance spawn:" + distance);



				// If the random coordinate is too close to the player, the algorithm adds a random amount of X and Y distances,
				// that total the minimum of NonSpawnRange. 
				if (distance < NonSpawnRange) {
					Debug.Log ("Correcting...");
					double correctionX = Random.Range (0, NonSpawnRange);
					double correctionY = NonSpawnRange - correctionX;

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
				SpawnNode (_locations[i]);
			}
		}
	}



	// Checks the distance between the player and each marker on map.
	// This may not be the most efficient method.
	void DistanceCheck () {

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

			PlopMarkers ();
		}
		rem.Clear ();


	}






	// LOCAL
	private void SpawnNode(Vector2d location){

		//Sprite theSprite = NodeSpriteContainer.GetSprite (nodeType, id);
		//GameObject node = Instantiate(new GameObject(), new Vector3(0,0,0), Quaternion.identity) as GameObject;
		GameObject node = Instantiate(_markerPrefab);
		node.transform.localPosition = _map.GeoToWorldPosition(location);

		//node.AddComponent<SpriteRenderer> ().sprite = theSprite;
		WorldNode wNode = node.AddComponent<WorldNode> ();
		NodeSpawner.WorldNodeRandoms (wNode);
		wNode.GetNodeColor ();
		wNode.latitude = location.x;
		wNode.longitude = location.y;

		localNodeList.Add (node); // <- This may be reduntant
		_spawnedObjects.Add(node);
		// TODO: World to local position
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

