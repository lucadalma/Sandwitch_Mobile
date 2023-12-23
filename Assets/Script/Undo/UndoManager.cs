using UnityEngine;
using System.Collections.Generic;

public class UndoManager : MonoBehaviour
{

    /*
    Lista composta da più liste di una struct (InformationsGameObject), 
    dove mi salvo le iformazioni che mi servono per resettare la scena di un solo passaggio
     
    */
    private List<List<InformationsGameObject>> moveHistory = new List<List<InformationsGameObject>>();

    //Lista di tutti gli oggetti che posso muovere (ingredienti e pezzi di pane)
    [SerializeField]
    public List<GameObject> mouvableIngredients = new List<GameObject>();


    void Start()
    {
        //mi memorizzo la posizione iniziale degli oggetti in start
        MemorizeHistory();

    }

    //funzione per il salvataggio in memoria (nella lista movehistori)
    public void MemorizeHistory()
    {
        //lista alla quale aggiungero una struct per ogni mossa di ogni oggetto, che poi, questa lista, l'aggiungerò alla lista movehistory
        List<InformationsGameObject> objsInformations = new List<InformationsGameObject>();

        //mi ciclo tutti gli oggetti presenti in scena
        foreach (GameObject obj in mouvableIngredients)
        {
            // mi creo una nuova istanza della struct per ogni ingrediente
            InformationsGameObject objInformation = new InformationsGameObject();

            //assegno tutte le informazioni che mi servo
            objInformation.gameObj = obj;
            objInformation.position = obj.transform.position;
            objInformation.rotation = obj.transform.rotation;
            objInformation.parent = obj.transform.parent;
            objInformation.layer = obj.layer;

            //aggiungo ogni singolo oggetto alla struct intermedia objsInformations
            objsInformations.Add(objInformation);
        }

        //aggiungo objsInformations alla lista principale
        moveHistory.Add(objsInformations);
    }

    //Funzione per eliminare l'ultima mossa fatta dall'utente
    public void DeleteLastMove()
    {
        //controllo se almeno ha fatto una mossa
        if (moveHistory.Count > 1)
        {
            //rimuovo l'ultima lista di informazioni da infondo la lista principale
            moveHistory.RemoveAt(moveHistory.Count - 1);

            //ripristino la mossa precedente a quella salvata
            RestoreBeforeMove();
        }
    }

    //funzione per il ripristino della scena allo stato precendente
    private void RestoreBeforeMove()
    {
        // mi creo una lista 
        List<InformationsGameObject> objsInformations = moveHistory[moveHistory.Count - 1];

        for (int i = 0; i < objsInformations.Count; i++)
        {
            InformationsGameObject objInformation = objsInformations[i];
            objInformation.gameObj.transform.position = objInformation.position;
            objInformation.gameObj.transform.rotation = objInformation.rotation;
            objInformation.gameObj.transform.parent = objInformation.parent;
            objInformation.gameObj.layer = objInformation.layer;
        }
    }
}


// una struct che utilizzo per salvarmi le informazioni utili per il reset di una mossa e per rendermi più facile l'accesso alle informazioni
//al posto di utilizzare il semplice gameObject
public struct InformationsGameObject
{
    public GameObject gameObj;
    public Vector3 position;
    public Quaternion rotation;
    public Transform parent;
    public LayerMask layer;
}