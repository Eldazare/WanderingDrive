using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ComboManager : MonoBehaviour {


	bool version2 = true;
	// TODO: Marker MonoBehaviour classes:
	// AliveTime

	// TODO: Combat controller script which resumes combat and takes List<bool>

	private float circleRadius;

	public GameObject circleMarker;
	public GameObject swipeMarker;

	public GraphicRaycaster gr;
	public GameObject markerCanvasParent;

	List<ComboPieceAbstraction> comboContent;
	List<bool> comboResult;
	bool returned = false;
	bool awaitingInput = false;
	int direction = -1;

	GameObject spawnedMarker;
	float aliveTimeLeft = 0;

	void Start(){
		StartCombo (new List<ComboPieceAbstraction>());
	}

	// TEST ONLY
	void Update(){
		aliveTimeLeft -= Time.deltaTime;

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Mouse down");
			GetTapInput (Input.mousePosition);
		}
		if (Input.GetKeyDown(KeyCode.Keypad6)){
			Debug.Log ("Keypad - 0");
			GetSwipeInput (0);
		}
		if (Input.GetKeyDown(KeyCode.Keypad9)){
			Debug.Log ("Keypad - 1");
			GetSwipeInput (1);
		}
		if (Input.GetKeyDown(KeyCode.Keypad8)){
			Debug.Log ("Keypad - 2");
			GetSwipeInput (2);
		}
		if (Input.GetKeyDown(KeyCode.Keypad7)){
			Debug.Log ("Keypad - 3");
			GetSwipeInput (3);
		}
		if (Input.GetKeyDown(KeyCode.Keypad4)){
			Debug.Log ("Keypad - 4");
			GetSwipeInput (4);
		}
		if (Input.GetKeyDown(KeyCode.Keypad1)){
			Debug.Log ("Keypad - 5");
			GetSwipeInput (5);
		}
		if (Input.GetKeyDown(KeyCode.Keypad2)){
			Debug.Log ("Keypad - 6");
			GetSwipeInput (6);
		}
		if (Input.GetKeyDown(KeyCode.Keypad3)){
			Debug.Log ("Keypad - 7");
			GetSwipeInput (7);
		}
	}

	public void GetSwipeInput(int direction){
		if (awaitingInput) {
			awaitingInput = false;
			if (this.direction == direction) {
				ReturnInput (true);
			} else {
				ReturnInput (false);
			}
		}
	}

	public void GetTapInput(Vector2 pixelCoord){
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

	public void ReturnInput(bool success){
		Destroy (spawnedMarker);
		returned = true;
		comboResult.Add (success);
		Debug.Log (success);
		if (success) {
			Debug.Log (aliveTimeLeft.ToString("F2") + " seconds left");
		}
	}

	public void StartCombo(List<ComboPieceAbstraction> comboList){
		// TODO: Non debug combolist
		float aliveTimeForMarkers = 5f;
		comboContent = new List<ComboPieceAbstraction> ();
		comboContent.Add (new ComboPieceAbstraction ("circle", aliveTimeForMarkers));
		comboContent.Add (new ComboPieceAbstraction ("swipe", aliveTimeForMarkers));
		comboContent.Add (new ComboPieceAbstraction ("swipe", aliveTimeForMarkers));
		StartCoroutine (ComboMaster (comboContent));
	}


	void SpawnComboMarker(ComboPieceAbstraction abstr){
		direction = -1;
		aliveTimeLeft = abstr.aliveTime;
		switch (abstr.type) {
		case "circle":
			spawnedMarker = Instantiate (circleMarker, markerCanvasParent.transform) as GameObject;
			RectTransform comboCompRectTransform = spawnedMarker.GetComponent<RectTransform> ();
			if (version2) {
				Rect rectOfMarkerCanvas = markerCanvasParent.GetComponent<RectTransform> ().rect;
				float randX = Random.Range (rectOfMarkerCanvas.width / -2, rectOfMarkerCanvas.width / 2);
				float randY = Random.Range (rectOfMarkerCanvas.height / -2, rectOfMarkerCanvas.height / 2);
				Debug.Log("Offsets " + randX.ToString("F2") + "  " + randY.ToString("F2"));
				comboCompRectTransform.localPosition = new Vector2 (randX, randY);

			}
			break;
		case "swipe":
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

	private IEnumerator ComboMaster(List<ComboPieceAbstraction> comboList){
		comboResult = new List<bool> ();
		foreach (ComboPieceAbstraction combP in comboList) {
			yield return ComboPart (combP);
		}
		// RETURN
		Debug.Log("Combo complete");
	}

	private IEnumerator ComboPart(ComboPieceAbstraction combP){
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
