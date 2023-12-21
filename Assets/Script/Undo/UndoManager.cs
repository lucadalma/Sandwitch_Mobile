using UnityEngine;
using System.Collections.Generic;

public class UndoManager : MonoBehaviour
{
    private List<List<InformationsGameObject>> moveHistory = new List<List<InformationsGameObject>>();

    public List<GameObject> mouvableIngredients = new List<GameObject>();


    void Start()
    {
        MemorizeHistory();

    }

    public void MemorizeHistory()
    {
        List<InformationsGameObject> objInformations = new List<InformationsGameObject>();

        foreach (GameObject obj in mouvableIngredients)
        {
            InformationsGameObject objInformation = new InformationsGameObject();
            objInformation.gameObj = obj;
            objInformation.position = obj.transform.position;
            objInformation.rotation = obj.transform.rotation;
            objInformation.parent = obj.transform.parent;
            objInformation.layer = obj.layer;

            objInformations.Add(objInformation);
        }

        moveHistory.Add(objInformations);
    }

    public void DeleteLastMove()
    {
        if (moveHistory.Count > 1)
        {
            moveHistory.RemoveAt(moveHistory.Count - 1);
            RestoreBeforeMove();
        }
    }

    private void RestoreBeforeMove()
    {
        List<InformationsGameObject> objInformations = moveHistory[moveHistory.Count - 1];

        for (int i = 0; i < objInformations.Count; i++)
        {
            InformationsGameObject objInformation = objInformations[i];
            objInformation.gameObj.transform.position = objInformation.position;
            objInformation.gameObj.transform.rotation = objInformation.rotation;
            objInformation.gameObj.transform.parent = objInformation.parent;
            objInformation.gameObj.layer = objInformation.layer;
        }
    }
}

public struct InformationsGameObject
{
    public GameObject gameObj;
    public Vector3 position;
    public Quaternion rotation;
    public Transform parent;
    public LayerMask layer;
}