using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InteractionWIthObject : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    //public UnityEvent interactAction;
    public string taskSceneName;
    public bool loadSceneTrueLoadTaskFalse;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    GameManagerScript gameManager;
    AudioManagerScript audioManager;

    
    
    void Start()
    {
        
        gameManager = GameManagerScript.instance;
        audioManager = AudioManagerScript.instance;
        TaskObjectIdScript parentObjectScript = GetComponentInParent<TaskObjectIdScript>();
        if (parentObjectScript != null)
        {
            taskSceneName = parentObjectScript.TaskSceneName;

        }
    }

    // Update is called once per frame
    // In InteractionWIthObject.cs, modify the Update() method:
    void Update()
    {
        if (isInRange && Input.GetKeyDown(interactKey) && gameManager.BlockKeyboard == false)
        {
            if (loadSceneTrueLoadTaskFalse == true)
            {
                gameManager.mostRecentSchoolScene = SceneManager.GetActiveScene().name;
                // SPECIAL CASE: Backyard to Hallway door requires Doodle Jump completion
                if (taskSceneName == "Hallway" && SceneManager.GetActiveScene().name == "Backyard")
                {
                    // Check if player has completed Doodle Jump requirement
                    if (!HasCompletedDoodleJumpRequirement())
                    {
                        // Load Doodle Jump challenge instead of Hallway
                        LoadDoodleJumpChallenge();

                        return;
                    }
                }
                if (taskSceneName == "Classroom1")
                {
                    LoadFlappyBirdChallenge();
                    gameManager.spawnXByScene["Hallway"] = -9.320066f;


                }
                else if (taskSceneName == "Classroom2")
                {
                    LoadPacManChallenge(); 
                    gameManager.spawnXByScene["Hallway"] = 10.97f;

                }
                else if (taskSceneName == "Classroom3")
                {
                    LoadDoodleJumpChallenge();

                    gameManager.spawnXByScene["Hallway"] = 40.11f;

                }
                else if (taskSceneName == "Classroom4")
                {
                    LoadFlappyBirdChallenge();
                    gameManager.spawnXByScene["Hallway"] = 74.69f;

                }
                else if (taskSceneName == "Cafeteria")
                { // Cafeteria
                    LoadPacManChallenge(); 
                        gameManager.spawnXByScene["Hallway"] = 86.35f;
                    //gameManager.loadSceneReplace(taskSceneName);
                }
                else {
                    gameManager.loadSceneReplace(taskSceneName);
                }
                // Regular scene loading for other doors
                //if (taskSceneName == "Classroom3" || taskSceneName == "Classroom4")
                //{
                //    gameManager.loadSceneReplaceFromDoorToClassroom(taskSceneName);
                //}
                //else
                //{
                //    gameManager.loadSceneReplace(taskSceneName);
                //}
            }
            else
            {
                GameObject taskObjectRoot = transform.parent ? transform.parent.gameObject : null;
                if (taskObjectRoot != null)
                {
                    gameManager.setCurrentTaskObjectThatPlayerIsSolving(taskObjectRoot);
                }
                gameManager.loadSceneAdditive(taskSceneName);
                AudioManagerScript.instance.playSFX(AudioManagerScript.instance.taskOpen);
            }
        }
    }

    private bool HasCompletedDoodleJumpRequirement()
    {
        // Check if player has completed Doodle Jump requirement
        // You can store this in PlayerPrefs or GameManager
        // return PlayerPrefs.GetInt("DoodleJumpCompleted", 0) == 1;
        return false;
    }

    private bool HasCompletedFlappyBirdRequirement()
    {
        // Check if player has completed Doodle Jump requirement
        // You can store this in PlayerPrefs or GameManager
        // return PlayerPrefs.GetInt("FlappyBirdCompleted", 0) == 1;
        return false;
    }

    private void LoadDoodleJumpChallenge()
    {
        //SceneManager.LoadScene("DoodleJumpClassroom");
        
        SceneManager.LoadScene("Main_MenuDoodleJump");
        audioManager.PauseMusic();
        gameManager.disableCanvasForTheCrtificateAndPauseButtonTopRight();

        // Load Doodle Jump as a task (not a scene)
        //GameManagerScript.instance.loadSceneAdditive("DoodleJumpClassroom");
    }
    private void LoadFlappyBirdChallenge()
    {

        SceneManager.LoadScene("Start Menu");
        audioManager.PauseMusic();
        gameManager.disableCanvasForTheCrtificateAndPauseButtonTopRight();
        // Load Doodle Jump as a task (not a scene)
        //GameManagerScript.instance.loadSceneAdditive("DoodleJumpClassroom");
    }

    private void LoadPacManChallenge()
    {
        SceneManager.LoadScene("StartScreen");
        audioManager.PauseMusic();
        gameManager.disableCanvasForTheCrtificateAndPauseButtonTopRight();
        // Load Doodle Jump as a task (not a scene)
        //GameManagerScript.instance.loadSceneAdditive("DoodleJumpClassroom");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var gameObj = collision.gameObject;
        if (gameObj.CompareTag("Player"))
        {
            isInRange = true;
            if (gameObject.CompareTag("Room"))
            {
                gameObj.GetComponent<PlayerManager>().NotifyPLayer(1);
            }
            else
            {
                gameObj.GetComponent<PlayerManager>().NotifyPLayer(0);
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var gameObj = collision.gameObject;
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            if (gameObject.CompareTag("Room"))
            {
                gameObj.GetComponent<PlayerManager>().DeNotifyPLayer(1);
            }
            else
            {
                gameObj.GetComponent<PlayerManager>().DeNotifyPLayer(0);
            }

        }
    }
}