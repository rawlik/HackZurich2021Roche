using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinderLikeAnimation : MonoBehaviour
{
    float moveOutTarget = 1000;
    float resetTimeAfterSwipe = 0.5f;
    float initialPosition;
    float targetPosition;
    bool isMovingOut = false;

    Vector3 newPositionVector = new Vector3();

    void Start()
    {
        initialPosition = transform.position.x;
        targetPosition = initialPosition;
        newPositionVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"targetPosition {targetPosition}");
        newPositionVector.x = Mathf.Lerp(transform.position.x, targetPosition, 0.2f);
        transform.position = newPositionVector;
    }

    public void SetRelativeTargetPosition(float target)
    {
        if (!isMovingOut)
            targetPosition = initialPosition + target;
    }

    public void ResetPosition()
    {
        if (!isMovingOut)
            targetPosition = initialPosition;
    }

    public void MoveOutRight()
    {
        Debug.Log("Move out right");
        isMovingOut = true;
        targetPosition = initialPosition + moveOutTarget;
        StartCoroutine(ReturnCoroutine());
    }

    public void MoveOutLeft()
    {
        Debug.Log("Move out left");
        isMovingOut = true;
        targetPosition = initialPosition - moveOutTarget;
        StartCoroutine(ReturnCoroutine());
    }

    IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(resetTimeAfterSwipe);

        isMovingOut = false;
        targetPosition = initialPosition;
    }
}
