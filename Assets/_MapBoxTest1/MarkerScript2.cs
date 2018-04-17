using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Mapbox.Unity.Map;

public class MarkerScript2 : MonoBehaviour {

	[SerializeField]
	AbstractMap _map;

	// Vector2d is different from Vector2. A lot of the MapboxSDK funtions seem to use it.
	// It's essentially the same thing as Vector2. It has X and Y coordinates, except it takes them as latitude and longitude.
	public Vector2d _location;

	
	// 60.221410, 24.804813 = metropolia, espoo
	void Update () {

		// This is basically all that's required to set up a marker via lat-long coordinates.
		// Google wasn't particularly helpful in finding this.
		transform.localPosition = _map.GeoToWorldPosition(_location);

	}
}
