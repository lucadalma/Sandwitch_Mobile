using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    //Variabili per la grandezza della griglia
    private int height = 4;
    private int width = 4;
    private float GridSpaceSize = 1f;


    //Prefab cella griglia
    [SerializeField] private GameObject gridCellPrefab;

    //Array dove salvo i prefab generati per la creazione della griglia
    public GameObject[,] gameGrid;

    void Start()
    {
        CreateGrid();
    }


    //Funzione creazione griglia
    private void CreateGrid()
    {
        //imposto nell'array l'altezza e la larghezza della griglia
        gameGrid = new GameObject[height, width];

        //check del prefab se è assegnato
        if (gridCellPrefab == null) 
        {
            return;
        }

        //doppio ciclo for per instanziare le celle nelle varie posizioni
        for (int z = 0; z < height; z++) 
        {
            for (int x = 0; x < width; x++)
            {
                gameGrid[x, z] = Instantiate(gridCellPrefab, new Vector3(x * GridSpaceSize, 0 , z * GridSpaceSize), Quaternion.identity);
                gameGrid[x, z].transform.parent = transform;
                gameGrid[x, z].gameObject.name = "Grid Space ( X: " + x.ToString() + " , Z: " + z.ToString() + ")";
            }        
        }
    }
}
