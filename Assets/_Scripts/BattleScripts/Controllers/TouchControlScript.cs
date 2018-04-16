using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlScript : MonoBehaviour {

    private Vector3 fp; //First touch position
    private Vector3 lp; //Last touch position
    private float dragDistance; //minimum distance for a swipe to be registered

    public PlayerCombatScript player;
    public bool enemyTurn;
    bool isTouchableArea;

    void Start () {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
    }
    void Update () {
        if (Input.touchCount == 1 && enemyTurn) // user is touching the screen with a single touch
        {
            if (Input.GetTouch (0).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch (0).position);

                RaycastHit hit;

                if (Physics.Raycast (ray, out hit)) {
                    if (hit.collider != null && hit.collider.name == "Quad") {
                        isTouchableArea = true;
                    }
                } else {
                    isTouchableArea = false;
                }

            }
            Touch touch = Input.GetTouch (0); // get the touch
            if (touch.phase == TouchPhase.Began && isTouchableArea) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            } else if (touch.phase == TouchPhase.Moved && isTouchableArea) // update the last position based on where they moved
            {
                lp = touch.position;
            } else if (touch.phase == TouchPhase.Ended && isTouchableArea) //check if the finger is removed from the screen
            {
                lp = touch.position; //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs (lp.x - fp.x) > dragDistance || Mathf.Abs (lp.y - fp.y) > dragDistance) { //It's a drag
                    //check if the drag is vertical or horizontal
                    if (Mathf.Abs (lp.x - fp.x) > Mathf.Abs (lp.y - fp.y)) { //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x)) //If the movement was to the right)
                        { //Right swipe
                            Debug.Log ("Right Swipe");
                            player.Dodge (1);
                        } else { //Left swipe
                            Debug.Log ("Left Swipe");
                            player.Dodge (0);
                        }
                    } else { //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y) //If the movement was up
                        { //Up swipe
                            Debug.Log ("Up Swipe");
                            player.Dodge (0);
                        } else { //Down swipe
                            Debug.Log ("Down Swipe");
                            player.Dodge (1);
                        }
                    }
                } else { //It's a tap as the drag distance is less than 20% of the screen height
                    if (isTouchableArea) {
                        Debug.Log ("Tap");
                        player.Block ();
                    }
                }
            }
        }
    }
}