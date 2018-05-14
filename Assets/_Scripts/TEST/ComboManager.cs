using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {

	bool version2 = true;
	// TODO: Marker MonoBehaviour classes:
	// AliveTime

	// TODO: Combat controller script which resumes combat and takes List<bool>

	private float circleRadius;
	public GameObject circleMarker;
	public GameObject swipeMarker;

	public GraphicRaycaster gr;
	public GameObject comboBackground;
	public GameObject markerCanvasParent;

	List<ComboPieceAbstraction> comboContent;
	List<bool> comboResult;
	List<float> percentageTimesLeft;
	bool returned = false;
	bool awaitingInput = false;
	int direction = -1;
	public Image successImage;

	GameObject spawnedMarker;
	float aliveTimeLeft = 0;
	float aliveTimeMax = 0;
	float FadeRate = 5f;
     private float targetAlpha;

	/* void Start(){
		StartCombo (new List<ComboPieceAbstraction>());
	} */

	// TEST ONLY
	void Update () {
		aliveTimeLeft -= Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.P)) {
			Debug.Log ("P ~clicked~");
			GetTapInput (Input.mousePosition);
		}
		if (Input.GetKeyDown (KeyCode.Keypad6)) {
			Debug.Log ("Keypad - 0");
			GetSwipeInput (0);
		}
		if (Input.GetKeyDown (KeyCode.Keypad9)) {
			Debug.Log ("Keypad - 1");
			GetSwipeInput (1);
		}
		if (Input.GetKeyDown (KeyCode.Keypad8)) {
			Debug.Log ("Keypad - 2");
			GetSwipeInput (2);
		}
		if (Input.GetKeyDown (KeyCode.Keypad7)) {
			Debug.Log ("Keypad - 3");
			GetSwipeInput (3);
		}
		if (Input.GetKeyDown (KeyCode.Keypad4)) {
			Debug.Log ("Keypad - 4");
			GetSwipeInput (4);
		}
		if (Input.GetKeyDown (KeyCode.Keypad1)) {
			Debug.Log ("Keypad - 5");
			GetSwipeInput (5);
		}
		if (Input.GetKeyDown (KeyCode.Keypad2)) {
			Debug.Log ("Keypad - 6");
			GetSwipeInput (6);
		}
		if (Input.GetKeyDown (KeyCode.Keypad3)) {
			Debug.Log ("Keypad - 7");
			GetSwipeInput (7);
		}

	}

	public void GetSwipeInput (int direction) {
		if (awaitingInput) {
			awaitingInput = false;
			if (this.direction == direction) {
				ReturnInput (true);
			} else {
				ReturnInput (false);
			}
		}
	}

	public void GetTapInput (Vector2 pixelCoord) {
		if (awaitingInput) {
			awaitingInput = false;
			PointerEventData ped = new PointerEventData (null);
			ped.position = pixelCoord;
			List<RaycastResult> results = new List<RaycastResult> ();
			gr.Raycast (ped, results);
			foreach (RaycastResult result in results) {
				if (result.gameObject.CompareTag ("ComboComponent")) {
					ReturnInput (true);
					return;
				}
			}
			ReturnInput (false);
		}
	}

	public void ReturnInput (bool success) {
		Destroy(spawnedMarker);
		returned = true;
		comboResult.Add (success);
		percentageTimesLeft.Add (aliveTimeLeft / aliveTimeMax);
		Debug.Log (success);
		if (success) {
			SuccessLerp ();
			Debug.Log (aliveTimeLeft.ToString ("F2") + " seconds left");
		} else {
			FailedLerp ();
		}
	}

	void SuccessLerp () {
		successImage.color = Color.green;
		var colorz = successImage.color;
		colorz.a = 100/255f;
		successImage.color = colorz;
		StartCoroutine(FadeOut());
	}
	void FailedLerp () {
		successImage.color = Color.red;
		var colorz = successImage.color;
		colorz.a = 100/255f;
		successImage.color = colorz;
		StartCoroutine(FadeOut());
	}
	IEnumerator FadeOut () {
		while (true) {
			yield return new WaitForSeconds(Time.deltaTime);
			Color curColor = successImage.color;
			float alphaDiff = Mathf.Abs (curColor.a - targetAlpha);
			if (alphaDiff > 0.0001f) {
				curColor.a = Mathf.Lerp (curColor.a, 0, FadeRate * Time.deltaTime);
				successImage.color = curColor;
			}else{
				break;
			}
		}
	}

	public void StartCombo (List<ComboPieceAbstraction> comboList) {
		float aliveTimeForMarkers = 5f;
		if (comboList.Count == 0) {
			comboContent = new List<ComboPieceAbstraction> ();
			comboContent.Add (new ComboPieceAbstraction (ComboPieceType.Tap, aliveTimeForMarkers));
			comboContent.Add (new ComboPieceAbstraction (ComboPieceType.Swipe, aliveTimeForMarkers));
			comboContent.Add (new ComboPieceAbstraction (ComboPieceType.Swipe, aliveTimeForMarkers));
		} else {
			comboContent = comboList;
		}
		StartCoroutine (ComboMaster (comboContent));
	}

	void SpawnComboMarker (ComboPieceAbstraction abstr) {
		direction = -1;
		aliveTimeLeft = abstr.aliveTime;
		aliveTimeMax = abstr.aliveTime;
		switch (abstr.type) {
		case ComboPieceType.Tap:
				spawnedMarker = Instantiate (circleMarker, markerCanvasParent.transform) as GameObject;
				RectTransform comboCompRectTransform = spawnedMarker.GetComponent<RectTransform> ();
				if (version2) {
					Rect rectOfMarkerCanvas = markerCanvasParent.GetComponent<RectTransform> ().rect;
					float randX = Random.Range (rectOfMarkerCanvas.width / -2, rectOfMarkerCanvas.width / 2);
					float randY = Random.Range (rectOfMarkerCanvas.height / -2, rectOfMarkerCanvas.height / 2);
					Debug.Log ("Offsets " + randX.ToString ("F2") + "  " + randY.ToString ("F2"));
					comboCompRectTransform.localPosition = new Vector2 (randX, randY);

				}
				break;
		case ComboPieceType.Swipe:
				spawnedMarker = Instantiate (swipeMarker, markerCanvasParent.transform) as GameObject;
				direction = 0;
				if (version2) {
					direction = Random.Range (0, 8);
					spawnedMarker.transform.rotation = Quaternion.Euler (0, 0, direction * 45f);
				}
				break;
			default:
				Debug.LogError ("Undefined combo marker");
				break;
		}
		awaitingInput = true;
	}

	private IEnumerator ComboMaster (List<ComboPieceAbstraction> comboList) {
		comboBackground.SetActive (true);
		comboResult = new List<bool> ();
		percentageTimesLeft = new List<float> ();
		foreach (ComboPieceAbstraction combP in comboList) {
			yield return ComboPart (combP);
		}
		// RETURN
		float multiplier = 0;
		foreach (var item in comboResult) {
			if (item) {
				multiplier += 0.5f;
			}
		}
		// TODO: Use usable percentageTimesLeft
		GetComponent<CombatController> ().ProceedAfterPlayerCombo (multiplier);
		Debug.Log ("Combo complete");
		comboBackground.SetActive (false);
	}

	private IEnumerator ComboPart (ComboPieceAbstraction combP) {
		SpawnComboMarker (combP);
		Debug.Log ("awaiting input");
		while (true) {
			if (returned) {
				break;
			}
			yield return null;
		}
		returned = false;
	}
}
