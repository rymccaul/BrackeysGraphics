using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject player;

    private bool tapLeft, tapRight, tapPlayer, holdLeft, holdRight, swipeLeft,
                                                swipeRight, swipeUp, swipeDown;
    private bool tapRequested;
    //private bool isDragging = false;
    private bool isHolding = false;
    private Vector2 startTouch, swipeDelta;

    private Vector3 playerScreenPosition;

    private float timeAtTouchDown;
    private float timeNow;

    private void Update()
    {
        //playerScreenPosition = gameCamera.WorldToScreenPoint(player.transform.position);
        //Debug.Log(playerScreenPosition);

        tapLeft = tapRight = tapPlayer = holdLeft = holdRight = swipeLeft =
                                    swipeRight = swipeUp = swipeDown = false;

        // Mouse controls
        if (Input.GetMouseButtonDown(0))
        {
            tapRequested = true;
            isHolding = true;
            startTouch = Input.mousePosition;
            timeAtTouchDown = Time.realtimeSinceStartup;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (tapRequested) {
                // If tapped position is within 50 pixels of the center of player
                // position, register as center/boost tap instead of lane change
                // tap.
                playerScreenPosition = gameCamera.WorldToScreenPoint(player.transform.position);

                if (Input.mousePosition.x > playerScreenPosition.x - 25
                        && Input.mousePosition.x < playerScreenPosition.x + 25
                        && Input.mousePosition.y > playerScreenPosition.y - 25
                        && Input.mousePosition.y < playerScreenPosition.y + 25)
                {
                    tapPlayer = true;
                }
                else if (Input.mousePosition.x > (Screen.width / 2))
                {
                   // Debug.Log("Right");
                    tapRight = true;
                }
                else if (Input.mousePosition.x < (Screen.width / 2))
                {
                    tapLeft = true;
                }
            }
            Reset();
        }

        // Mobile/Touch controls
        if(Input.touchCount > 0)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tapRequested = true;
                isHolding= true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                if (tapRequested) {
                    if (Input.touches[0].position.x > (Screen.width / 2))
                    {
                       // Debug.Log("Right");
                        tapRight = true;
                    } else if (Input.touches[0].position.x < (Screen.width / 2))
                    {
                        tapLeft = true;
                    }
                }
                Reset();
            }
        }

        //Calculate the distance
        swipeDelta = Vector2.zero;
        if (isHolding)
        {
            // Check if mouse or touch is dragging
            if(Input.touchCount > 0) { swipeDelta = Input.touches[0].position - startTouch; }
            else if (Input.GetMouseButton(0)) { swipeDelta = (Vector2)Input.mousePosition - startTouch; }
        }

        //Did we cross the dead zone?
        if(swipeDelta.magnitude > 100)
        {
            tapRequested = false;
            //Which direction are we swiping?
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                //Left or right?
                if (x > 0) { swipeRight = true; }
                else { swipeLeft = true; }
                //x > 0 ? swipeRight = true : swipeLeft = true;
            }
            else
            {
                //Up or down?
                if (y > 0) { swipeUp = true; }
                else { swipeDown = true; }
                // y > 0 ? swipeUp = true : swipeDown = true;
            }
            Reset();
        }
        else if (isHolding && (Time.realtimeSinceStartup - timeAtTouchDown > 0.1))
        {
            Debug.Log("Holding");
            // If not swiping and has held down for more than x milliseconds
            tapRequested = false;
            if (Input.GetMouseButton(0)){
              // Using mouse controls
              if (Input.mousePosition.x > (Screen.width / 2))
              {
                 // Debug.Log("Right");
                  holdRight = true;
              }
              else if (Input.mousePosition.x < (Screen.width / 2))
              {
                  holdLeft = true;
              }
            }

            // Using touch controls
            if(Input.touchCount > 0)
            {
              if (Input.touches[0].position.x > (Screen.width / 2))
              {
                 // Debug.Log("Right");
                  holdRight = true;
              } else if (Input.touches[0].position.x < (Screen.width / 2))
              {
                  holdLeft = true;
              }
            }
        }

        //Debug.Log(Time.realtimeSinceStartup - timeAtTouchDown);
    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isHolding = false;
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool TapLeft { get { return tapLeft; } }
    public bool TapRight { get { return tapRight; } }
    public bool TapPlayer { get { return tapPlayer; } }
    public bool HoldLeft { get { return holdLeft; } }
    public bool HoldRight { get { return holdRight; } }
}
