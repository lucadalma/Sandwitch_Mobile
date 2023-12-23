using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    //variabili utili per prendermi la posizione finale ed iniziale del tocco del player (swipe)
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    //soglia utile per capire quando registrare un movimento
    public float SWIPE_THRESHOLD = 20f;

    //lista di tutti gli ingredienti
    public List<GameObject> Ingridients;

    //lista dei pezzi di pane
    public List<GameObject> Breads;

    //Variabile bool per capire se l'oggetto da muovere si sta ancora muovendo
    bool isMoving = false;
    //variabile di check per capire se il player ha mosso il dito
    bool moved;

    //riferimenti ad altri script
    FindObjectToMove findObjectToMove;
    UndoManager undoManager;

    //oggetto da muovere
    public GameObject gameObjectToMove;

    private void Start()
    {
        // inizializzo variabili
        undoManager = FindObjectOfType<UndoManager>();

        findObjectToMove = FindObjectOfType<FindObjectToMove>();

        moved = false;
    }

    void Update()
    {
        //mi prendo l'oggetto da muovere dallo script FindObjectToMove
        gameObjectToMove = findObjectToMove.gameObjectToMove;

        //per ogni input
        foreach (Touch touch in Input.touches)
        {
            //quando il player tocca per la prima volta lo schermo la posizione di inzio e fine è la stessa
            if (touch.phase == TouchPhase.Began)
            {
                fingerUp = touch.position;
                fingerDown = touch.position;
            }
            //appena muove il dito 
            if (touch.phase == TouchPhase.Moved)
            {
                // se voglio che registi lo swipe senza che alzi il dito
                if (!detectSwipeOnlyAfterRelease)
                {
                    //aggiorno la posizione finale
                    fingerDown = touch.position;
                    //controllo direzione swipe
                    checkSwipe();
                }
            }

            //quando il player toglie lo dito dallo schermo aggiorno la posizione finale 
            if (touch.phase == TouchPhase.Ended)
            {
                fingerDown = touch.position;
                //controllo direzione swipe
                checkSwipe();
            }
        }
        // resetto le variabili degli oggetti da muovere
        ResetObjectToMove();
    }

    //funzione per capire in che direzione faccio swipe
    void checkSwipe()
    {
        //Controllo se è uno swipe verticale controllando se la differenza dal punto iniziale e
        //il punto iniziale sono maggiori della sogli di "sensibilità" e del punto iniziale e finale in x
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //swipe verso l'alto
            if (fingerDown.y - fingerUp.y > 0)
            {
                OnSwipeUp();
            }
            //swipe verso il basso
            else if (fingerDown.y - fingerUp.y < 0)
            {
                OnSwipeDown();
            }
            //resetto le posizioni mettendole uguali
            fingerUp = fingerDown;
        }
        //Controllo se è uno swipe orrizzontale controllando se la differenza dal punto iniziale e
        //il punto iniziale sono maggiori della sogli di "sensibilità" e del punto iniziale e finale in y
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //controllo se lo swipe è verso destra
            if (fingerDown.x - fingerUp.x > 0)
            {
                OnSwipeRight();
            }
            // o sinistra
            else if (fingerDown.x - fingerUp.x < 0)
            {
                OnSwipeLeft();
            }
            //resetto le posizioni mettendole uguali
            fingerUp = fingerDown;
        }
        //Nessun movimento
        else
        {
            //Debug.Log("Nessun movimento registrato");
        }
    }

    //funzione per calcolarmi il movimento verticale del dito
    float verticalMove()
    {
        //calcolo il numero assoluto dei due punti solo in y(verticale)
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }
    //funzione per calcolarmi il movimento orizozntale del dito
    float horizontalValMove()
    {
        //calcolo il numero assoluto dei due punti solo in y(orizzontale)
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    // se il movimeto è verticale
    void OnSwipeUp()
    {
        //controllo se ho un oggetto da muovere
        if (gameObjectToMove == null)
        {
            return;
        }
        //muovo l'oggetto veroso la direzione desiderata
        MoveObject(gameObjectToMove, Vector3.forward);
        moved = true;
    }
    //se il moviemto è verso il basso
    void OnSwipeDown()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        MoveObject(gameObjectToMove, Vector3.back);
        moved = true;
    }
    // se il movimeto è verso sinistra
    void OnSwipeLeft()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        MoveObject(gameObjectToMove, Vector3.left);
        moved = true;
    }
    // verso destra
    void OnSwipeRight()
    {
        if (gameObjectToMove == null)
        {
            return;
        }
        MoveObject(gameObjectToMove, Vector3.right);
        moved = true;
    }


    //se l'oggetto è stato mosso utilizzo questa funzione per resettare i valori dell'oggetto damuovore
    public void ResetObjectToMove()
    {
        if (moved == true)
        {
            gameObjectToMove = null;
            findObjectToMove.gameObjectToMove = null;
            moved = false;
        }
    }
    // funzioe che utilizzo per muovere l'oggetto selezionato, avendo sia l'oggetto che la direzione verso la queale lo voglio muovere
    public void MoveObject(GameObject objToMove, Vector3 direction)
    {
        //oggetto destinazione
        GameObject nearestObject;

        //variabili che utilizzerò dopo per controllare quanti figli avranno l'oggetto che voglio muovere e l'oggetto "destinazione"
        int ObjTargetChildCount = 0;
        int ObjToMoveChildCount = 0;

        //raycast che parte dall'oggetto che voglio muovere, il raycast va in direzione dove volgio muovere l'oggetto e trova il più vicino in quella direzione
        RaycastHit hit;
        if (Physics.Raycast(objToMove.transform.position, direction, out hit, 1.1f))
        {
            //oggetto destinazione assegnato2
            nearestObject = hit.collider.gameObject;

            //posizione dell'oggetto destinazione
            Vector3 positionNearestObject = nearestObject.transform.position;

            //Debug del ray
            Debug.DrawRay(objToMove.transform.position, direction * hit.distance, Color.yellow);

            //se l'oggetto si stamuovendo allora esci
            if (isMoving)
                return;
            //conto gli oggetti figli
            ObjTargetChildCount = nearestObject.transform.childCount;
            ObjToMoveChildCount = objToMove.transform.childCount;
            //trovo l'utimo figlio
            GameObject lastChild = nearestObject.transform.GetChild(ObjTargetChildCount - 1).gameObject;

            //questa parte viente utilizzata per controllare se l'oggetto che voglio muovere è un pezzo di pane e se posso muoverlo
            bool canMoveBread = false;

            if (objToMove.transform.name == "Bread")
            {
                if (nearestObject.transform.name != "Bread")
                {
                    canMoveBread = false;
                }
                else if (nearestObject.transform.name == "Bread")
                {
                    //controllo se tutti la somma dei figli dei due pezzi è uguale al numero di ingredienti,
                    //questo vuoldire che tutti gli ingredienti sono sopra ai pezzi di pane e posso muovere il pane
                    if ((objToMove.transform.hierarchyCount / 2) - 1 + (nearestObject.transform.hierarchyCount / 2) - 1 >= Ingridients.Count)
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

            //se non posso muovere il pane
            if (objToMove.transform.name == "Bread" && canMoveBread == false)
            {
                Debug.Log("Non puoi spostare il pane");
            }
            //se posso muovere il pane o se l'oggetto che voglio muovere è un igrediente qualsiasi
            else 
            {
                //inizio una coroutine per ruotare l'ingrediente
                StartCoroutine(RotateIngridient(objToMove, nearestObject, direction, lastChild, ObjTargetChildCount, ObjToMoveChildCount));
            
            }

            //Debug.Log(lastChild.name);

        }
    }

    //coroutine per ruotare l'oggetto e posizionarlo alla posizione giusta
    IEnumerator RotateIngridient(GameObject objToMove, GameObject nearestObj, Vector3 direction, GameObject lastChild, int ObjTargetChildCount, int ObjToMoveChildCount)
    {


        isMoving = true;
        //angolo totale di rotazione
        float remainingAngle = 180;

        //punto centrale di rotazione
        Vector3 rotationCenter = objToMove.transform.position + direction / 2 + Vector3.up / 5;

        //asse di rotazioen
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        //Debug.Log("Entrato");


        //se non ho ruotato ancora di 180 gradi
        while (remainingAngle > 0)
        {

            float rotationAngle = Mathf.Min(Time.deltaTime * 300, remainingAngle);
            //rotazione intorno al punto
            objToMove.transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);

            //tolgo la rotazine che ho già fatto
            remainingAngle -= rotationAngle;
            yield return null;


        }

        //l'oggetto non si muove più
        isMoving = false;

        //caso stano in cui un child di un ingredient, ha anchesso dei child attaccati, questo succede perchè l'oggetto che ruoto verso un altro alla fine diventa child dell'oggetto destinzione
        bool strangeCase = false;

        // se l'oggetto che voglio muovere ha già un oggetto figlio attaccato (un altro ingredinte)
        if (objToMove.transform.childCount > 1)
        {
            List<GameObject> childs = new List<GameObject>();
            for (int i = 0; i < objToMove.transform.childCount; i++)
            {
                //controllo se questo figlio o figli, hanno loro figli a loro volta (altri ingredient)
                childs.Add(objToMove.transform.GetChild(i).gameObject);
                if (childs[i].transform.childCount > 1) 
                {
                    //allora gestisco la cosa in modo diverso in base ai casi
                    strangeCase = true;
                }

            }
        }

        //Debug.Log("StrangeCase: " + strangeCase);


        //se è un caso strano
        if (ObjToMoveChildCount > 1 && strangeCase)
        {

            //devo capire di quanto devo alzarmi per trovarmi nella posizione giusta
            float numberOfIncrementation = objToMove.transform.hierarchyCount / 2;
            //Debug.Log(numberOfIncrementation);

            //alla fine della rotaizone imposto l'altezza in cui deve trovasi l'oggetto che ho mosso alla fine della rotazione
            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + (0.25f * numberOfIncrementation), objToMove.transform.position.z);
        }
        //se non è uno strano caso ma comunque l'oggetto che voglio muovere ha un figlio 
        else if (ObjToMoveChildCount > 1 && !strangeCase)
        {
            //mi alzo
            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + (0.25f * ObjToMoveChildCount), objToMove.transform.position.z);
        }
        //sennò vuol dire che gli oggetti non hanno ingredienti sopra e si posso spostare tranquillamente
        else
        {
            objToMove.transform.position = new Vector3(objToMove.transform.position.x,
                lastChild.transform.position.y + 0.25f, objToMove.transform.position.z);
        }

        //l'oggetto che muovo diventa figlio di quello destinazione
        objToMove.transform.SetParent(nearestObj.transform);
        //gli aggiorno la layerMask
        objToMove.layer = 9;
        //aggiorno la storia dei movimenti
        undoManager.MemorizeHistory();
    }
}
