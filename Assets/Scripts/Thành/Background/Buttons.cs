using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{ 

    public void PLayGame()
    {
        SceneManager.LoadSceneAsync(1);
        

    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
