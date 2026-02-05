using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenuUi;
    public void OnGameResumePress() { 
        pauseMenuUi.SetActive(false);
    }
    public void OnGameExitPress() { 
            Application.Quit(); // use ne ja koristam funkcijava
    }
    public void OnEnterPausePress() {
        pauseMenuUi.SetActive(true);
    }
}

//public static class InputBlockerScript
//{

//    public static bool BlockKeyboard = false;
//}