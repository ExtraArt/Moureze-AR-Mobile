using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ScrollingPanorama : MonoBehaviour
{ 
    // flag to keep track whether we are dragging or not
    public bool isDragging = false;

    // starting point of a camera movement
    float startTouchX;
    float startTouchY;

    public Camera cam;
    private Vector2 touchPosition = default;

    void Start()
    {
        // Get our camera component
        cam = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // if we press the touch the screen and we haven't started dragging
        if (Input.touchCount > 0 && !isDragging)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;

                // save the mouse starting position
                startTouchX = touchPosition.x;
                startTouchY = touchPosition.y;
            }
            else
            {
                isDragging = false;
            }
        }
    }

    void LateUpdate()
    {
        // Check if we are dragging
        if (isDragging == true)
        {
            //Calculate current touch position
            float endMouseX = touchPosition.x;
            float endMouseY = touchPosition.y;

            //Difference (in screen coordinates)
            float diffX = endMouseX - startTouchX;
            float diffY = endMouseY - startTouchY;

            //New center of the screen
            float newCenterX = Screen.width / 2 + diffX;
            float newCenterY = Screen.height / 2 + diffY;

            //Get the world coordinate , this is where we want to look at
            Vector3 LookHerePoint = cam.ScreenToWorldPoint(new Vector3(newCenterX, newCenterY, cam.nearClipPlane));

            //Make our camera look at the "LookHerePoint"
            transform.LookAt(LookHerePoint);

            //starting position for the next call
            startTouchX = endMouseX;
            startTouchY = endMouseY;
        }
    }
}