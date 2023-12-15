using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIngredientsScript : MonoBehaviour
{

    [SerializeField]
    GameObject topBread, bottonBread;

    [SerializeField]
    GameObject tomato;

    [SerializeField]
    GameObject insalata;

    [SerializeField]
    GameObject salame;

    GameGrid gameGridManager;

    private void Start()
    {
        gameGridManager = FindObjectOfType<GameGrid>();
    }

    private void Update()
    {
        
    }



}
