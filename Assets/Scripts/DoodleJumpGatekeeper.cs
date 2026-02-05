// DoodleJumpGatekeeperManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DoodleJumpGatekeeperManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text challengeText;
    public TMP_Text scoreText;
    public GameObject startPanel;
    public GameObject gamePanel;

    [Header("Doodle Jump Game")]
    public GameObject doodleJumpGame; // Drag your Doodle Jump game object here

    [Header("Target Score")]
    public int requiredScore = 7000;

    private Game_Controller gameController;

    void Start()
    {
        // Initially show start panel, hide game
        startPanel.SetActive(true);
        gamePanel.SetActive(false);

        // Update challenge text
        challengeText.text = $"Score {requiredScore} points to unlock the Hallway!";

        // Disable Doodle Jump game initially
        if (doodleJumpGame != null)
            doodleJumpGame.SetActive(false);
    }

    public void StartGame()
    {
        // Switch to game view
        startPanel.SetActive(false);
        gamePanel.SetActive(true);

        // Enable and reset Doodle Jump game
        if (doodleJumpGame != null)
        {
            doodleJumpGame.SetActive(true);
            gameController = doodleJumpGame.GetComponent<Game_Controller>();

            // Reset the game
            if (gameController != null)
                gameController.ResetGame();
        }

        // Unblock keyboard for game controls
        GameManagerScript.instance.BlockKeyboard = false;
    }

    public void ExitChallenge()
    {
        // Return to Backyard
        SceneManager.UnloadSceneAsync("DoodleJumpGatekeeper");
        GameManagerScript.instance.BlockKeyboard = false;
    }

    void Update()
    {
        // Update score display if game is active
        if (gameController != null && gamePanel.activeSelf)
        {
            int currentScore = gameController.GetCurrentScore();
            scoreText.text = $"Score: {currentScore} / {requiredScore}";

            // The actual transition happens in Game_Controller when score >= 7000
        }
    }
}