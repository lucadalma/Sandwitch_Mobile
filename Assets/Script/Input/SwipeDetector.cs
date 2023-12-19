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

    public List<GameObject> Breads;

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
        //Debug.Log("Swipe UP");
        MoveObject(gameObjectToMove, Vector3.forward);
        moved = true;
    }

    void OnSwipeDown()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        //Debug.Log("Swipe Down");
        MoveObject(gameObjectToMove, Vector3.back);
        moved = true;
    }

    void OnSwipeLeft()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        //Debug.Log("Swipe Left");
        MoveObject(gameObjectToMove, Vector3.left);
        moved = true;
    }

    void OnSwipeRight()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        //Debug.Log("Swipe Right");
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

        int ObjTargetChildCount = 0;
        int ObjToMoveChildCount = 0;


        float step = 100f * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(objToMove.transform.position, direction, out hit, 1.1f))
        {
            nearestObject = hit.collider.gameObject;
            Vector3 positionNearestObject = nearestObject.transform.position;

            Debug.DrawRay(objToMove.transform.position, direction * hit.distance, Color.yellow);
            //Debug.Log("Did Hit: " + nearestObject.name);

            if (isMoving)
                return;

            ObjTargetChildCount = nearestObject.transform.childCount;
            ObjToMoveChildCount = objToMove.transform.childCount;
            GameObject lastChild = nearestObject.transform.GetChild(ObjTargetChildCount - 1).gameObject;

            bool canMoveBread = false;

            if (objToMove.transform.name == "Bread")
            {
                if (nearestObject.transform.name != "Bread")
                {
                    canMoveBread = false;
                }
                else if (nearestObject.transform.name == "Bread")
                {
                    if ((objToMove.transform.hierarchyCount / 2) + (nearestObject.transform.hierarchyCount / 2) >= Ingridients.Count)
                    {
                        canMoveBread = true;
                    }
                }
                else
                {
                    canMoveBread = false;
                }
            }
            else 
            {
                canMoveBread = false;
            }

            if (objToMove.transform.name == "Bread" && canMoveBread == false)
            {
                Debug.Log("Non puoi spostare il pane");
            }
            else 
            {
                StartCoroutine(RotateIngridient(objToMove, nearestObject, direction, lastChild, ObjTargetChildCount, ObjToMoveChildCount));
            
            }

            Debug.Log(lastChild.name);

        }
    }

    IEnumerator RotateIngridient(GameObject objToMove, GameObject nearestObj, Vector3 direction, GameObject lastChild, int ObjTargetChildCount, int ObjToMoveChildCount)
    {


        isMoving = true;
        float remainingAngle = 180;
        Vector3 rotationCenter = objToMove.transform.position + direction / 2 + Vector3.up / 5;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        //Debug.Log("Entrato");

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * 300, remainingAngle);
            objToMove.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;


        }


        isMoving = false;

        bool strangeCase = false;

        if (objToMove.transform.childCount > 1)
        {
            List<GameObject> childs = new List<GameObject>();
            for (int i = 0; i < objToMove.transform.childCount; i++)
            {
                childs.Add(objToMove.transform.GetChild(i).gameObject);
                if (childs[i].transform.childCount > 1) 
                {
                    strangeCase = true;
                }

            }
        }

        Debug.Log("StrangeCase: " + strangeCase);

        if (ObjToMoveChildCount > 1 && strangeCase)
        {


            float numberOfIncrementation = objToMove.transform.hierarchyCount / 2;
            Debug.Log(numberOfIncrementation);


            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + (0.25f * numberOfIncrementation), objToMove.transform.position.z);
        }
        else if (ObjToMoveChildCount > 1 && !strangeCase)
        {
            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + (0.25f * ObjToMoveChildCount), objToMove.transform.position.z);
        }
        else
        {
            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + 0.25f, objToMove.transform.position.z);
        }

        objToMove.transform.SetParent(nearestObj.transform);
        objToMove.layer = 9;


    }




}
