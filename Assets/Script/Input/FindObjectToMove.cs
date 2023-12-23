using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectToMove : MonoBehaviour
{
    //Variabili per il raycast
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    //oggetto che verrà selezionato attraverso il raycast
    public GameObject gameObjectToMove;

    SwipeDetector swipeDetector;

    private void Start()
    {
        swipeDetector = FindObjectOfType<SwipeDetector>();
    }

    void Update()
    {
        //se è un solo tocco
        if (Input.touchCount == 1)
        {
            //prendo il tocco da parte dell'utente
            Touch touch = Input.GetTouch(0);

            //utilizzo ilk touch phase began per fare un raycast che non duri troppo
            if (touch.phase == TouchPhase.Began)
            {
                //casto il raycast nella posizione del tocco
                ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                //Debug.DrawLine(ray.origin, ray.direction * 10);

                //se il raycasto colpisce qualcosa
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
                {
                    //se colpisce un ingrediente
                    if (hit.collider != null && hit.transform.CompareTag("Ingridients"))
                    {
                        //assegno l'ingrediente al gameObjectToMove
                        IngridientScript ingridient = hit.transform.gameObject.GetComponent<IngridientScript>();

                        gameObjectToMove = ingridient.gameObject;

                    }
                    //se colpisce un pezzo pane
                    else if (hit.collider != null && hit.transform.CompareTag("Bread"))
                    {

                        //assegno il pezzo di pane al gameObjectToMove
                        BreadScript bread = hit.transform.gameObject.GetComponent<BreadScript>();

                        gameObjectToMove = bread.gameObject;
                    }
                    //se non colpisco niente
                    else if (hit.collider != null)
                    {

                        //resetto le variabili
                        //Debug.Log("Hit nothing");
                        swipeDetector.ResetObjectToMove();
                        return;
                    }

                    //Debug.Log(gameObjectToMove.name);
                }
            }
        }
    }
}
