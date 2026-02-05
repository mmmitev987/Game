using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class DoodleJumpTaskManager : MonoBehaviour
{
    [Header("Doodle Jump References")]
    public GameObject doodleJumpGameObject; // The entire Doodle Jump game
    public Game_Controller doodleJumpController; // Reference to the Doodle Jump controller

    [Header("UI References")]
    public GameObject taskCanvas;
    public GameObject gameCanvas;
    public GameObject winScreen;
    public GameObject loseScreen;

    [Header("Score Requirements")]
    public int requiredScore = 1000; // Score needed to pass

    [Header("UI Elements")]
    public TMP_Text currentScoreText;
    public TMP_Text requiredScoreText;
    public TMP_Text resultScoreText;

    private int currentScore = 0;
    private bool gameCompleted = false;
    private bool gameFailed = false;

    void Start()
    {
        // Initialize UI
        requiredScoreText.text = requiredScore.ToString();
        gameCanvas.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

        // Start with task explanation
        taskCanvas.SetActive(true);

        // Get reference to Doodle Jump controller
        if (doodleJumpController == null && doodleJumpGameObject != null)
        {
            doodleJumpController = doodleJumpGameObject.GetComponent<Game_Controller>();
        }
    }

    void Update()
    {
        if (!gameCompleted && !gameFailed && doodleJumpController != null)
        {
            // Check if game is over
            if (doodleJumpController.Get_GameOver())
            {
                OnDoodleJumpGameOver();
            }
        }
    }

    public void StartDoodleJumpGame()
    {
        // Switch to game view
        taskCanvas.SetActive(false);
        gameCanvas.SetActive(true);

        // Enable Doodle Jump game
        if (doodleJumpGameObject != null)
        {
            doodleJumpGameObject.SetActive(true);
            // Reset game state if needed
            // You might need to add a Reset() method to Game_Controller
        }

        GameManagerScript.instance.BlockKeyboard = false; // Allow keyboard for Doodle Jump
    }

    private void OnDoodleJumpGameOver()
    {
        // Get final score from the Doodle Jump controller
        // Note: You'll need to modify Game_Controller to expose the current score
        // or use the score calculation from OnGUI

        // For now, let's assume we can get the score via reflection or by adding a public method
        int finalScore = GetCurrentScoreFromDoodleJump();
        currentScore = finalScore;

        // Check if player passed
        if (finalScore >= requiredScore)
        {
            ShowWinScreen();
        }
        else
        {
            ShowLoseScreen();
        }
    }

    private int GetCurrentScoreFromDoodleJump()
    {
        // Try to get score from Game_Controller
        // You may need to modify Game_Controller to expose the score
        // For now, return a placeholder
        return Random.Range(500, 1500); // Replace with actual score retrieval
    }

    private void ShowWinScreen()
    {
        gameCompleted = true;
        resultScoreText.text = $"Score: {currentScore}/{requiredScore}";
        winScreen.SetActive(true);
        gameCanvas.SetActive(false);

        // Play win sound
        AudioManagerScript.instance.playSFX(AudioManagerScript.instance.taskCorrect);

        // Mark task as completed after delay
        Invoke("CompleteTask", 2f);
    }

    private void ShowLoseScreen()
    {
        gameFailed = true;
        resultScoreText.text = $"Score: {currentScore}/{requiredScore}";
        loseScreen.SetActive(true);
        gameCanvas.SetActive(false);

        // Allow retry
        Invoke("EnableRetryButton", 1f);
    }

    public void RetryGame()
    {
        // Reset game state
        gameCompleted = false;
        gameFailed = false;
        loseScreen.SetActive(false);

        // Restart Doodle Jump game
        StartDoodleJumpGame();

        // You might need to add a Reset method to Game_Controller
        // or reload the Doodle Jump scene/additive
    }

    private void CompleteTask()
    {
        // Call GameManager to mark task as completed
        GameManagerScript.instance.unloadTaskSceneAfterCorrectlyFInishedTask("DoodleJumpClassroom");
    }

    public void CloseTask()
    {
        // Called when player gives up or needs to exit
        GameManagerScript.instance.unloadTaskSceneAfterWronglyAnsweredTask("DoodleJumpClassroom");
    }

    private void EnableRetryButton()
    {
        // Enable retry button on lose screen
        Button retryButton = loseScreen.GetComponentInChildren<Button>();
        if (retryButton != null)
        {
            retryButton.interactable = true;
        }
    }
}