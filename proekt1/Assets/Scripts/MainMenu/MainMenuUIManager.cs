using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    GameManagerScript gameManager;
    public Button playButton;
    
    void Start()
    {
        playButton.interactable = false;
        gameManager = GameManagerScript.instance;

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayButtonState();
    }

    void UpdatePlayButtonState()
    {
        bool hasName = !string.IsNullOrEmpty(gameManager.getPlayerName());
        bool hasAge = gameManager.ageGroup != 0;

        playButton.interactable = hasName && hasAge;
    }
}
