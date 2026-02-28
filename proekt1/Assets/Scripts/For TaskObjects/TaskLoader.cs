using UnityEngine;
using UnityEngine.SceneManagement;

public class TaskLoader : MonoBehaviour
{
    //public string TaskSceneName = "DragAndDropTask";
    public void loadScene(string TaskSceneName) {
        SceneManager.LoadScene(TaskSceneName, LoadSceneMode.Additive);
        GameManagerScript.instance.BlockKeyboard = true;
    }
}
