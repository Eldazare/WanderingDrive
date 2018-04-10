using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	Vector3 startPos, targetLoc;
	GameObject targetObject;
	Quaternion startRota, targetRota;
	float lerpSpeed = 2;
	public MenuController menuController; //Drag from hierarchy
	public bool follow;
	float startTime, movingLength;

	// Use this for initialization
	void Start () {
		//set lerping speed
		startPos = transform.position;
		startRota = transform.rotation;
	}
	public void MoveCamera(GameObject target){
		CancelInvoke("MoveFromTarget");
		CancelInvoke("MoveCamera");
		CancelInvoke("FollowTargetRepeat");
		targetObject = target;
		targetLoc = targetObject.transform.position;
		startTime = Time.time;
		movingLength = Vector3.Distance(transform.position, targetLoc);
		InvokeRepeating("MoveToTarget", 0, Time.deltaTime);
	}
	public void ResetCamera(){
		CancelInvoke("MoveFromTarget");
		CancelInvoke("MoveCamera");
		CancelInvoke("FollowTargetRepeat");
		menuController.enemyPartCanvas.SetActive(false);
		targetLoc = startPos;
		startTime = Time.time;
		movingLength = Vector3.Distance(transform.position, targetLoc);
		InvokeRepeating("MoveFromTarget", Time.deltaTime, Time.deltaTime);
	}
	public void FollowTarget(GameObject target){
		CancelInvoke("MoveFromTarget");
		CancelInvoke("MoveCamera");
		CancelInvoke("FollowTargetRepeat");
		targetObject = target;
		InvokeRepeating("FollowTargetRepeat", 0, Time.deltaTime);
	}

	void FollowTargetRepeat(){
		transform.position = targetObject.transform.position;
	}
	
	void MoveToTarget(){
		float distanceCovered = (Time.time-startTime)*lerpSpeed;
		if(Vector3.Distance(transform.position, targetLoc)>0.1){
			transform.position = Vector3.Lerp(transform.position, targetLoc, distanceCovered/movingLength);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetObject.transform.rotation, distanceCovered/movingLength);
		}else{
			menuController.proceed = true;
			CancelInvoke("MoveToTarget");
		}
	}
	
	void MoveFromTarget(){
		float distanceCovered = (Time.time-startTime)*lerpSpeed*0.2f;
		if(Vector3.Distance(transform.position, targetLoc)>0.1){
			transform.position = Vector3.Lerp(transform.position, targetLoc, distanceCovered/movingLength);
			transform.rotation = Quaternion.Lerp(transform.rotation, startRota, distanceCovered/movingLength);
		}else{
			CancelInvoke("MoveFromTarget");
		}
	}
}
