using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //testo della vittoria
    [SerializeField]
    GameObject youWinText;

    //bottone per tornare indietro di una mossa
    [SerializeField]
    GameObject ButtonUndo;

    SwipeDetector swipeDetector;

    //numero di ingredienti e di pezzi di pane presenti nella scena
    int numberIngredients;
    int numberBreads;


    void Start()
    {

        swipeDetector = FindObjectOfType<SwipeDetector>();

        //inizzializzo le variabili
        numberIngredients = swipeDetector.Ingridients.Count;
        numberBreads = 2;
    }

    void Update()
    {
        //per ogni oggetto pane controllo gli oggetti figli, se uno dei due pezzi ha come figli tutti gli altri ingredienti più l'altro pezzo di pane, vuol dire che il player ha vinto
        foreach (GameObject bread in swipeDetector.Breads)
        {
            if ((bread.transform.hierarchyCount / 2) - 1 == numberIngredients + numberBreads - 1) 
            {
                youWinText.SetActive(true);
                ButtonUndo.SetActive(false);
                //Game Win
                Debug.Log("GAME WIN");
            }
        }
    }

    //Reset della scena con il loadScene
    public void ResetLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
