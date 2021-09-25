using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] public UnityEvent OnSwipeRight;
    [SerializeField] public UnityEvent OnSwipeLeft;

    [SerializeField] private TinderLikeAnimation tinderLikeAnimation;

    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    [SerializeField] private float swipeThreshold = 250f;

    private void OnValidate()
    {
        if (tinderLikeAnimation == null)
            Debug.LogError("tinderLikeAnimation cannot be null");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                //Debug.Log("TouchPhase: began");

                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                //Debug.Log("TouchPhase: moved");

                fingerDown = touch.position;
                moveObject();

                if (!detectSwipeOnlyAfterRelease)
                {
                    checkSwipe();
                }

            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                //Debug.Log("TouchPhase: ended");

                fingerDown = touch.position;
                checkSwipe();

                tinderLikeAnimation.ResetPosition();
            }

            //Debug.Log($"fingerUp: {fingerUp}");
            //Debug.Log($"fingerDown: {fingerDown}");
        }
    }

    void moveObject()
    {
        //Debug.Log($"fingerDown.x - fingerUp.x: {fingerDown.x - fingerUp.x}");

        tinderLikeAnimation.SetRelativeTargetPosition(fingerDown.x - fingerUp.x);
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > swipeThreshold && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                onSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                onSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > swipeThreshold && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                onSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                onSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void onSwipeUp()
    {
        //Debug.Log("Swipe UP");
    }

    void onSwipeDown()
    {
        //Debug.Log("Swipe Down");
    }

    void onSwipeLeft()
    {
        Debug.Log("Swipe Left");

        tinderLikeAnimation.MoveOutLeft();

        if (OnSwipeLeft != null)
            OnSwipeLeft.Invoke();
    }

    void onSwipeRight()
    {
        Debug.Log("Swipe Right");

        tinderLikeAnimation.MoveOutRight();

        if (OnSwipeRight != null)
            OnSwipeRight.Invoke();
    }
}