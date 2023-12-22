using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField]
    List<Object> scenes;

    public void LoadSpecificScene(int numberScene) 
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            if (i == numberScene)
                SceneManager.LoadScene(scenes[i].name);
        }
    }
}
