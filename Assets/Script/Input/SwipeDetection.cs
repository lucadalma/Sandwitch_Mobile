using UnityEngine;
using System.Collections;

public class SwipeDetection : MonoBehaviour
{

    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position

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

        SwipeObject(gameObjectToMove);

        ResetObjectToMove();
    }

    public void SwipeObject(GameObject gameObjectToMove)
    {
        if (gameObjectToMove == null) 
        {
            return;
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if ((fp.x - lp.x) > 80) // left swipe
                {
                    Debug.Log("left swipe here...");
                    gameObjectToMove.transform.Translate(Vector3.left * 10);
                    moved = true;
                }
                else if ((fp.x - lp.x) < -80) // right swipe
                {
                    Debug.Log("right swipe here...");
                }
                else if ((fp.y - lp.y) < -80) // up swipe
                {
                    Debug.Log("up swipe here...");
                }
                else if ((fp.y - lp.y) > 80) // down swipe
                {
                    Debug.Log("down swipe here...");
                }
            }
        }
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
}


