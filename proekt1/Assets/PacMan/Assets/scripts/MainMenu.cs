using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuPacMan : MonoBehaviour
{
    // this is a script with functions that are used by button on the start screen

    const string gameSceneName = "GamePacMan";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
