using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadTaskObjectsScriptClassroom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnDisable()
    {
        GameManagerScript gameManager = GameManagerScript.instance;

        gameManager.DestroyTasksForScene("Classroom1");
        gameManager.DestroyTasksForScene("Classroom2");
        gameManager.DestroyTasksForScene("Classroom3");
        gameManager.DestroyTasksForScene("Classroom4");
    }
}
