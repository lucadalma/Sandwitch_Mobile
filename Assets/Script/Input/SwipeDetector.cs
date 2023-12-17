using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;


    public List<GameObject> Ingridients;

    bool isMoving = false;

    bool moved;
    int moves;

    FindObjectToMove findObjectToMove;

    public GameObject gameObjectToMove;

    private void Start()
    {
        findObjectToMove = FindObjectOfType<FindObjectToMove>();

        moved = false;
    }

    // Update is called once per frame
    void Update()
    {

        gameObjectToMove = findObjectToMove.gameObjectToMove;

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                checkSwipe();
            }
        }

        ResetObjectToMove();
    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
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
    void OnSwipeUp()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        Debug.Log("Swipe UP");
        MoveObject(gameObjectToMove, Vector3.forward);
        moved = true;
    }

    void OnSwipeDown()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        Debug.Log("Swipe Down");
        MoveObject(gameObjectToMove, Vector3.back);
        moved = true;
    }

    void OnSwipeLeft()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        Debug.Log("Swipe Left");
        MoveObject(gameObjectToMove, Vector3.left);
        moved = true;
    }

    void OnSwipeRight()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        Debug.Log("Swipe Right");
        MoveObject(gameObjectToMove, Vector3.right);
        moved = true;
    }

    public void ResetObjectToMove()
    {
        if (moved == true)
        {
            gameObjectToMove = null;
            findObjectToMove.gameObjectToMove = null;
            moved = false;
        }
    }

    public void MoveObject(GameObject objToMove, Vector3 direction) 
    {
        GameObject nearestObject;


        bool canMove;

        float step = 100f * Time.deltaTime;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(objToMove.transform.position, direction, out hit, 1.1f))
        {
            nearestObject = hit.collider.gameObject;
            Vector3 positionNearestObject = nearestObject.transform.position;

            Debug.DrawRay(objToMove.transform.position, direction * hit.distance, Color.yellow);
            Debug.Log("Did Hit: " + nearestObject.name);

            if (isMoving)
                return;


            StartCoroutine(RotateIngridient(objToMove, direction));

            objToMove.transform.SetParent(nearestObject.transform);
            objToMove.layer = 9;


            

        }
    }

    IEnumerator RotateIngridient(GameObject objToMove, Vector3 direction) 
    {
        isMoving = true;
        float remainingAngle = 180;
        Vector3 rotationCenter = objToMove.transform.position + direction / 2 + Vector3.up / 5;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        Debug.Log("Entrato");

        while (remainingAngle > 0) 
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * 300, remainingAngle);
            objToMove.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;


    }

}
