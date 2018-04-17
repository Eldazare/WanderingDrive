using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mapbox.Unity.Utilities;
using Mapbox.Utils;


namespace Mapbox.Unity.Location{

	public class move : MonoBehaviour {


		//public GameObject EditorProvider;
		public EditorLocationProvider Editor;

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {
			Movement ();
		}


		void LatLong () {

			float latitude = 0.0f;
			float longitude = 0.0f;

			if (Input.GetAxis ("Horizontal") != 0) {
				latitude += 0.01f;
			}

			string location = latitude.ToString () + ", " + longitude;

			//Editor.
			//EditorProvider.GetComponent<EditorLocationProvider>().

		}

		void Movement () {

			float y = Input.GetAxis ("Vertical") * 60;
			float x = Input.GetAxis ("Horizontal") * 60;

			//Debug.Log (x + " by " + y);

			//transform.Translate (x, y, 0);
			GetComponent<Rigidbody> ().velocity = new Vector3 (x, 0, y);


		}
	}
}
