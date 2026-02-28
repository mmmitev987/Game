using UnityEngine;

public class TaskObjectIdScript : MonoBehaviour
{
    //// Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    [SerializeField] private string taskId; // id od JSON-ot
    [SerializeField] private string taskSceneName;

    public string TaskId => taskId;
    public string TaskSceneName => taskSceneName;

    public void SetTaskId(string id)
    {
        taskId = id;
        gameObject.name = $"TaskObject_{id}";
    }
    public void SetSceneName(string t)
    {
        taskSceneName = t;
    }
}
