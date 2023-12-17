using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectToMove : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    public GameObject gameObjectToMove;

    SwipeDetector swipeDetector;

    private void Start()
    {
        swipeDetector = FindObjectOfType<SwipeDetector>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                Debug.DrawLine(ray.origin, ray.direction * 10);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {

                    if (hit.collider != null && hit.transform.CompareTag("Ingridients"))
                    {
                        IngridientScript ingridient = hit.transform.gameObject.GetComponent<IngridientScript>();

                        gameObjectToMove = ingridient.gameObject;

                    }
                    else if (hit.collider != null && hit.transform.CompareTag("Bread"))
                    {
                        BreadScript bread = hit.transform.gameObject.GetComponent<BreadScript>();

                        gameObjectToMove = bread.gameObject;
                    }
                    else if (hit.collider != null)
                    {
                        //Debug.Log("Hit nothing");
                        swipeDetector.ResetObjectToMove();
                        return;
                    }

                    Debug.Log(gameObjectToMove.name);
                }
            }
        }
    }
}
