using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{

    private int height = 4;
    private int width = 4;
    private float GridSpaceSize = 1f;

    [SerializeField] private GameObject gridCellPrefab;
    public GameObject[,] gameGrid;

    void Start()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        gameGrid = new GameObject[height, width];

        if (gridCellPrefab == null) 
        {
            return;
        }

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
