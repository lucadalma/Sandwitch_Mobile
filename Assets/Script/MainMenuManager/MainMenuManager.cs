using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //mi passo le varie scene da caricare (i vari livelli) 
    [SerializeField]
    List<Object> scenes;

    // in base all'ordine in sui metto le scene nell'array, le richiamo usando un indice
    //questa funzione viene chiamata dal bottone del livello nella scena mainMenu
    public void LoadSpecificScene(int numberScene) 
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            if (i == numberScene)
                SceneManager.LoadScene(scenes[i].name);
        }
    }
}
