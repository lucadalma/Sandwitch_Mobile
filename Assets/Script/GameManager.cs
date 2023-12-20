using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject youWinText;

    SwipeDetector swipeDetector;

    int numberIngredients;
    int numberBreads;

    void Start()
    {
        swipeDetector = FindObjectOfType<SwipeDetector>();

        numberIngredients = swipeDetector.Ingridients.Count;
        numberBreads = 2;
    }

    void Update()
    {
        foreach (GameObject bread in swipeDetector.Breads)
        {
            if ((bread.transform.hierarchyCount / 2) - 1 == numberIngredients + numberBreads - 1) 
            {
                youWinText.SetActive(true);
                //Game Win
                Debug.Log("GAME WIN");
            }
        }
    }

    public void ResetLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
