using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTaskObjectsInSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameManagerScript gameManager;
    public string sceneNameForTaskObjectDictionary;
    void Start()
    {
        gameManager = GameManagerScript.instance;
        gameManager.InstantiateTasksForScene(sceneNameForTaskObjectDictionary);
    }

    // Update is called once per frame
    

    void OnDisable()
    {
        gameManager.DestroyTasksForScene(sceneNameForTaskObjectDictionary);
    }

}
