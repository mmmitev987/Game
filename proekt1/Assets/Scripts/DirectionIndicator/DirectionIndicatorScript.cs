using UnityEngine;
using UnityEngine.SceneManagement;

public class DirectionIndicatorScript : MonoBehaviour
{
    public GameObject player;
    public GameObject indicatorArrow;
    public GameObject noTaskObject;
    public static string directionOfTask; // left,right, empty room
    // right = true, left = false




    private void OnEnable()
    {
        directionOfTask = "right";
    }

    void Update()
    {
        GameObject closestTask = GetClosestTask();
        if (closestTask == null) {
            indicatorArrow.SetActive(false);
            noTaskObject.SetActive(true);
            //Debug.Log("nema taskovi");
        }
        else{
            indicatorArrow.SetActive(true);
            noTaskObject.SetActive(false);
            //Debug.Log("ima taskovi");

        }
        //if (closestTask != null)
        //{
        //    //Debug.Log("Closest Task is: " + closestTask.name);
        //    if (directionOfTask == "right")
        //    {
        //        //Debug.Log("Closest Task is: RIGHT");
        //    }
        //    else if (directionOfTask == "left")
        //    {
        //        //Debug.Log("Closest Task is: LEFT");
        //    }
        //    else {
        //        //Debug.Log("No task in this room");
        //    }
        //}
    }

    GameObject GetClosestTask()
    {
        GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task Object");

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        float playerX = player.transform.position.x;
        
        if (tasks.Length == 0) {
            directionOfTask = "empty room";
            return null;
        }

        foreach (GameObject task in tasks)
        {
            float taskPosition = task.transform.position.x;
            float distanceX = Mathf.Abs(taskPosition - playerX);
            if (distanceX < minDistance)
            {
                if (playerX > taskPosition) {
                    directionOfTask = "left";
                    //directionOfTask = false;
                }
                else{
                    directionOfTask = "right";
                    //directionOfTask = true;
                }
                minDistance = distanceX;
                closest = task;
            }
        }

        return closest;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        directionOfTask = "right"; // reset direction every time a scene loads
    }

    //void Start()
    //{
    //    // Get all objects with the "Task" tag
    //    GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task Object");

    //    foreach (GameObject task in tasks)
    //    {
    //        Debug.Log("Found task: " + task.name);
    //    }
    //}

    //public string getDirectionOfTask() {
    //    return directionOfTask;
    //}
    //public void setDirectionOfTask(string v) {
    //    directionOfTask = v;
    //}
}
