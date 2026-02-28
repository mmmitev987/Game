using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour
{

    private GameObject Player;

    private float Max_Height = 0;
    public Text Txt_Score;

    private int Score;
    private int targetScore = 7000; // Score needed to unlock hallway
    private bool targetReached = false;

    private Vector3 Top_Left;
    private Vector3 Camera_Pos;

    private bool Game_Over = false;

    public Text Txt_GameOverScore;
    public Text Txt_GameOverHighsocre;

    // Reference to GameManager for scene loading
    private GameManagerScript gameManager;

    void Awake()
    {
        Player = GameObject.Find("Doodler");
        gameManager = GameManagerScript.instance;

        // Initialize boundary 
        Camera_Pos = Camera.main.transform.position;
        Top_Left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }

    void FixedUpdate()
    {
        if (!Game_Over && !targetReached)
        {
            // Calculate max height
            if (Player.transform.position.y > Max_Height)
            {
                Max_Height = Player.transform.position.y;

                // Calculate current score
                Score = (int)(Max_Height * 50);

                // Check if target score reached
                if (Score >= targetScore)
                {
                    OnTargetScoreReached();
                }
            }

            // Check player fall
            if (Player.transform.position.y - Camera.main.transform.position.y < Get_DestroyDistance())
            {
                // Play game over sound
                GetComponent<AudioSource>().Play();

                // Set game over
                Set_GameOver();
                Game_Over = true;
            }
        }
    }

    void OnGUI()
    {
        // Update score display
        Txt_Score.text = Score.ToString();
    }

    private void OnTargetScoreReached()
    {
        targetReached = true;

        // Mark Doodle Jump as completed
        PlayerPrefs.SetInt("DoodleJumpCompleted", 1);
        PlayerPrefs.Save();

        // Play success sound if available
        AudioManagerScript audioManager = FindObjectOfType<AudioManagerScript>();
        if (audioManager != null && audioManager.taskCorrect != null)
        {
            audioManager.playSFX(audioManager.taskCorrect);
        }

        // Show success message on screen
        StartCoroutine(ShowSuccessAndTransition());
    }

    private IEnumerator ShowSuccessAndTransition()
    {
        // Create or show success message
        GameObject successMessage = new GameObject("SuccessMessage");
        Canvas canvas = successMessage.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        GameObject textObj = new GameObject("SuccessText");
        textObj.transform.SetParent(successMessage.transform);
        Text text = textObj.AddComponent<Text>();
        text.text = $"SUCCESS!\nScore: {Score}\nLoading Hallway...";
       // text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 40;
        text.color = Color.green;
        text.alignment = TextAnchor.MiddleCenter;

        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Wait for 2 seconds to show success message
        yield return new WaitForSeconds(0.3f);

        // Destroy the success message
        Destroy(successMessage);

        ; //ili treba da odime: Backayrd -> Hallway
                            // ili:               Hallway -> Classroom3

        string prevScene = gameManager.mostRecentSchoolScene;
        if (prevScene == "Backyard")//Backayrd -> Hallway
        {
            gameManager.UnloadDoodleJumpAndLoadHallway();
            
        }
        else
        { // Hallway -> Classroom3
            gameManager.UnloadDoodleJumpAndLoadClassroom("Classroom3");
        }

        // Unload Doodle Jump scene and load Hallway
        //if (gameManager != null)
        //{
        //    gameManager.UnloadDoodleJumpAndLoadHallway();
        //}
        //else
        //{
        //    // Fallback: Direct scene loading
        //    //SceneManager.UnloadSceneAsync("DoodleJumpGatekeeper");
        //    SceneManager.LoadScene("Hallway"/*, LoadSceneMode.Single*/);
        //}
    }

    public bool Get_GameOver()
    {
        return Game_Over;
    }

    public float Get_DestroyDistance()
    {
        return Camera_Pos.y + Top_Left.y;
    }

    void Set_GameOver()
    {
        if (Data_Manager.Get_HighScore() < Score)
            Data_Manager.Set_HighScore(Score);

        Txt_GameOverScore.text = Score.ToString();
        Txt_GameOverHighsocre.text = Data_Manager.Get_HighScore().ToString();
        GameObject Background_Canvas = GameObject.Find("Background_Canvas");

        // Active game over menu
        Button_OnClick.Set_GameOverMenu(true);

        // Enable animation
        Background_Canvas.GetComponent<Animator>().enabled = true;

        // Save file
        File_Manager.Save_Info();
    }

    public int GetCurrentScore()
    {
        return Score;
    }

    public void ResetGame()
    {
        Max_Height = 0;
        Score = 0;
        Game_Over = false;
        targetReached = false;

        // Reset player position and other game elements
        if (Player != null)
        {
            Player.transform.position = Vector3.zero;
            // Reset player components if needed
        }

        // Update score display
        if (Txt_Score != null)
            Txt_Score.text = "0";
    }
}