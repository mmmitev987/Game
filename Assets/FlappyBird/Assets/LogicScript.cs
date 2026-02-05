using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public Text scoreText;
    public GameObject gameOverScreen;
    public AudioSource gameOverMusic;
    public AudioSource gameMusic;

    private bool targetReached = false;
    private int targetScore = 5;
    private bool Game_Over = false;
    private GameManagerScript gameManager;


    void Start()
    {
        gameManager = GameManagerScript.instance;
        gameMusic.Play();
    }


    void FixedUpdate()
    {
        if (!Game_Over && !targetReached) {
            if (playerScore >= targetScore) {
                OnTargetScoreReached();
            }
        
        }
    }

    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        Debug.Log("Adding +" + scoreToAdd + " score");
        playerScore += scoreToAdd;
        scoreText.text = playerScore.ToString();
    }

    public void restartGame()
    {
        Debug.Log("Restart Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("Start Menu");
    }

    public void gameOver()
    {
        if (!gameOverScreen.activeSelf)
        {
            gameMusic.Stop();
            gameOverScreen.SetActive(true);
            gameOverMusic.Play();
        }

    }





    private void OnTargetScoreReached()
    {
        targetReached = true;

        // Mark Doodle Jump as completed
        PlayerPrefs.SetInt("FlappyBirdCompleted", 1);
        PlayerPrefs.Save();

        // Play success sound if available

        // Show success message on screen
        StartCoroutine(ShowSuccessAndTransition());
    }

    private IEnumerator ShowSuccessAndTransition()
    {
        // Create or show success message
        //GameObject successMessage = new GameObject("SuccessMessage");
        //Canvas canvas = successMessage.AddComponent<Canvas>();
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        //GameObject textObj = new GameObject("SuccessText");
        //textObj.transform.SetParent(successMessage.transform);
        //Text text = textObj.AddComponent<Text>();
        //text.text = $"SUCCESS!\nScore: {Score}\nLoading Hallway...";
        // text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        //text.fontSize = 40;
        //text.color = Color.green;
        //text.alignment = TextAnchor.MiddleCenter;

        //RectTransform rect = textObj.GetComponent<RectTransform>();
        //rect.anchorMin = new Vector2(0, 0);
        //rect.anchorMax = new Vector2(1, 1);
        //rect.offsetMin = Vector2.zero;
        //rect.offsetMax = Vector2.zero;

        // Wait for 2 seconds to show success message
        //yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(0.1f);

        // Destroy the success message
        //Destroy(successMessage);

        // Unload Doodle Jump scene and load Hallway


        string prevScene = gameManager.mostRecentSchoolScene;
        //Hallway -> Classroom1
        //Hallway -> Classroom4
        //gameManager.po
        float t = gameManager.spawnXByScene["Hallway"];
        if (t == -9.320066f)
        {
            gameManager.UnloadFlappyBirdAndLoadClassroom("Classroom1");
        }
        else //t == 74.69f
        {
            gameManager.UnloadFlappyBirdAndLoadClassroom("Classroom4");

        }

        //if (gameManager != null)
        //{
        //    gameManager.UnloadFlappyBirdAndLoadClassroom("Classroom1");
        //}
        //else
        //{
        //    // Fallback: Direct scene loading
        //    //SceneManager.UnloadSceneAsync("DoodleJumpGatekeeper");
        //    SceneManager.LoadScene("Hallway");
        //}
    }

    public bool Get_GameOver() {
        return Game_Over;
    }


}
