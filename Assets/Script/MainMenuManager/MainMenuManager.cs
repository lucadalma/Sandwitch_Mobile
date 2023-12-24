using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //mi passo le varie scene da caricare (i vari livelli) 
    [SerializeField]
    List<string> nameScenes;

    // In base al nome se è uguale carico i vari livelli
    //questa funzione viene chiamata dal bottone del livello nella scena mainMenu
    public void LoadSpecificScene(string levelName) 
    {
        for (int i = 0; i < nameScenes.Count; i++)
        {
            if (nameScenes[i] == levelName)
                SceneManager.LoadScene(nameScenes[i]);
        }
    }
}
