using UnityEngine;
using System.Collections;

public class SwipeDetection : MonoBehaviour
{

    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position

    bool canMove;

    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    GameObject gameObjectToMove;

    private void Start()
    {
        canMove = false;
    }

    void Update()
    {
        SwipeObject(gameObjectToMove);
    }

    public void SwipeObject(GameObject gameObjectToMove)
    {


        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {

                //ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                //Debug.DrawLine(ray.origin, ray.direction * 10);

                //if (Physics.Raycast(ray, out hit, mask))
                //{

                //    if (hit.collider != null && hit.transform.CompareTag("Ingridients"))
                //    {
                //        IngridientScript ingridient = hit.transform.gameObject.GetComponent<IngridientScript>();

                //        gameObjectToMove = ingridient.gameObject;

                //    }
                //    else if (hit.collider != null && hit.transform.CompareTag("Bread"))
                //    {
                //        BreadScript bread = hit.transform.gameObject.GetComponent<BreadScript>();

                //        gameObjectToMove = bread.gameObject;
                //    }
                //    else if (hit.collider != null)
                //    {
                //        Debug.Log("Hit nothing");
                //        return;
                //    }

                //    Debug.Log(gameObjectToMove.name);
                //}


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
}


