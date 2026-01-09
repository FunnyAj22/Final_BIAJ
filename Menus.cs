using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Credits()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void HowToPlay()
    {
        SceneManager.LoadSceneAsync(2);
    }
}

