using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;
    public bool BlockKeyboard = false;
    [SerializeField] Animator transitionAnim;
    public int tasksCompleted;
    private string playerName;
    public int ageGroup; //1 grupa: 6 do 11 god, 2 grupa:  12 do 15 god i 3 grupa:  16 do 18 god



    public GameObject PauseButton;
    public GameObject CertificateButton;
    public Dictionary<string, float> spawnXByScene; // Spawn position of the player in each scene


    // Predefined spawn positions per scene for task objects
    public Dictionary<string, List<Vector3>> availablePositionsForTaskObjectsPerScene;
    public Dictionary<string, Dictionary<Vector3, GameObject>> activeTasks;
    // vo ova active tasks stringot e koja e scenata, znaci imam sceni ("Backyard","Hallway","Cafeteria","Classroom1","Classroom2,"Classroom3"Classroom3")
    //i za sekoe od niv imam nov dict kade key e pozicijata na taskot, a vo GameObject na pocetok imam null samo. A posle koga ke dojde taa scena ke si se naprai
    // objekt od taskObject na dadenata pozicija

    // Prefab to spawn
    public GameObject taskPrefab;
    private GameObject currentTaskObjectThatPlayerIsSolving;


    // multiple choice task: q001 - q034
    // drag and drop task: q035 - q046
    // image task: q047 - q054
    //private List<string> allPossibleIdsForTaskContent = new List<string>();
    private Dictionary<string, List<string>> allPossibleIdsForTaskContent = new Dictionary<string, List<string>> {
            { "SingleChoiceTask", new List<string>() },
            { "DragAndDropTask",   new List<string>() },
            { "PhotoTask",         new List<string>() }
        };

    // ime na task scena i vo listata se site voznozni ids za toj vid na task
    private List<string> usedIdsForTaskContent = new List<string>(); // iskoristeni ids

    [Header("JSON files")]
    public TextAsset singleChoiceJson;  // expects fields: id, age_group
    public TextAsset dragAndDropJson;     // expects fields: id, age_group
    public TextAsset imageTaskJson;       // expects fields: id, age_group

    [Header("Doodle Jump Gatekeeper")]
    public bool doodleJumpCompleted = false;
    public int doodleJumpHighScore = 0;

    [Header("PacMan Gatekeeper")]
    public bool pacManCompleted = false;
    //public int doodleJumpHighScore = 0;

    public string mostRecentSchoolScene; // se koristi samo za doodleJump da se vidi od kaj e laodnat

    public Dictionary<string, float> spaspawnXByScene;

    AudioManagerScript audioManager;

    public  Canvas canvasForTheCrtificateAndPauseButtonTopRight;
    
    private void Awake()
    {
        audioManager = AudioManagerScript.instance;

        mostRecentSchoolScene = "Backyard";
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            tasksCompleted = 0;
        }
        else
        {
            Destroy(gameObject);
        }



        activeTasks = new();

        spawnXByScene = new Dictionary<string, float>
            {
                { "Backyard", 44.52f },
                { "Hallway", -14.46f },
                { "Cafeteria", -11.79f },
                { "Classroom", -11.92f }
            };

        availablePositionsForTaskObjectsPerScene = new Dictionary<string, List<Vector3>>
        {
            { "Backyard", new List<Vector3>
                {
                    new Vector3(-7.84f, -2.34913f, -1f),
                    new Vector3(-1.16f, -2.34913f, -1f),
                    new Vector3(10.27f, -2.34913f, -1f),
                    new Vector3(15.83f, -2.34913f, -1f),
                    new Vector3(21.13f, -2.34913f, -1f),
                    new Vector3(33.76f, -2.34913f, -1f),
                    new Vector3(57.49f, -2.34913f, -1f),
                    new Vector3(64.29f, -2.34913f, -1f),
                    new Vector3(72.28f, -2.34913f, -1f),

                }
            },
            { "Hallway", new List<Vector3>
                {
                    new Vector3(-1.34f, -2.660829f, -1),
                    new Vector3(17.48f, -2.660829f, -1),
                    new Vector3(28.54f, -2.660829f, -1),
                    new Vector3(48.67f, -2.660829f, -1),
                    new Vector3(57.19f, -2.660829f, -1),
                    new Vector3(66.76f, -2.660829f, -1),

                }
            },
            { "Cafeteria", new List<Vector3>
                {
                    new Vector3(-5.56f, -2.88f, -1),
                    new Vector3(7.79f, -2.88f, -1),
                    new Vector3(19f, -2.88f, -1),
                    new Vector3(30.7f, -2.88f, -1),
                    new Vector3(40.41f, -2.88f, -1),
                    new Vector3(51.01f, -2.88f, -1),
                    new Vector3(70.29f, -2.88f, -1),


                }
            },
            { "Classroom", new List<Vector3>
                {
                    new Vector3(-4.2f, -3.157123f, -1),
                    new Vector3(4.77f, -3.157123f, -1),
                    new Vector3(10.08f, -3.157123f, -1),
                    new Vector3(30.79f, -3.157123f, -1),
                    new Vector3(41f, -3.157123f, -1),

                }
            }
        };
        BuildActiveTasks();
        //LogActiveTasks();
        //InitializeAllIdsForTaskContent();

    }

    public void loadSceneAdditive(string TaskSceneName)
    {
        SceneManager.LoadScene(TaskSceneName, LoadSceneMode.Additive);
        instance.BlockKeyboard = true;

    }


    public void loadSceneReplace(string TaskSceneName)
    {
        StartCoroutine(loadSceneReplaceCoroutine(TaskSceneName));
    }
    private IEnumerator loadSceneReplaceCoroutine(string taskSceneName)
    {

        string currentScene = SceneManager.GetActiveScene().name;
        if (spawnXByScene.ContainsKey(currentScene))
        { // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) yield return null;
            spawnXByScene[currentScene] = player.transform.position.x;

        }

        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        var op = SceneManager.LoadSceneAsync(taskSceneName, LoadSceneMode.Single);
        while (!op.isDone) yield return null;   // wait until fully loaded
        yield return null;                      // let Awake/Start run this frame

        positionPlayerOnScene(taskSceneName);

        transitionAnim.SetTrigger("Start");
    }


    public void loadSceneReplaceFromDoorToClassroom(string TaskSceneName) //Samo za classrooms e ova
    {
        StartCoroutine(loadSceneReplaceCoroutineFromDoorToClassroom(TaskSceneName));
        //InstantiateTasksForScene(TaskSceneName);
    }
    private IEnumerator loadSceneReplaceCoroutineFromDoorToClassroom(string taskSceneName) //Samo za classrooms e ova
    {

        string currentScene = SceneManager.GetActiveScene().name;
        if (spawnXByScene.ContainsKey(currentScene))
        { // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) yield return null;
            spawnXByScene[currentScene] = player.transform.position.x;

        }

        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        var op = SceneManager.LoadSceneAsync("Classroom", LoadSceneMode.Single);
        while (!op.isDone) yield return null;   // wait until fully loaded
        yield return null;                      // let Awake/Start run this frame


        InstantiateTasksForScene(taskSceneName);

        positionPlayerOnScene("Classroom");

        transitionAnim.SetTrigger("Start");
    }



    public void unloadTaskSceneAfterCorrectlyFInishedTask(string taskSceneName)
    {
        
        StartCoroutine(unloadTaskSceneAfterCorrectlyFInishedTaskCoroutine(1, taskSceneName));

    }
    private IEnumerator unloadTaskSceneAfterCorrectlyFInishedTaskCoroutine(int seconds, string taskSceneName)
    {
        
        yield return new WaitForSeconds(seconds);
        tasksCompleted++;
        if (currentTaskObjectThatPlayerIsSolving)
        {


            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "Classroom")
            {
                foreach (var classroom in new List<string> { "Classroom1", "Classroom2", "Classroom3", "Classroom4" })
                {
                    foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(activeTasks[classroom]))
                    {
                        if (kv.Value == currentTaskObjectThatPlayerIsSolving)
                        {
                            activeTasks[classroom].Remove(kv.Key);
                            break;

                        }
                    }
                }
            }
            else
            {
                foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(activeTasks[sceneName]))
                {
                    if (kv.Value == currentTaskObjectThatPlayerIsSolving)
                    {
                        activeTasks[sceneName].Remove(kv.Key);
                        break;
                    }
                }
            }

            Destroy(currentTaskObjectThatPlayerIsSolving);
            currentTaskObjectThatPlayerIsSolving = null;
        }

        SceneManager.UnloadSceneAsync(taskSceneName);
        SceneManager.LoadScene("Certificate", LoadSceneMode.Additive);
    }

    public bool CanAccessHallway()
    {
        return doodleJumpCompleted || PlayerPrefs.GetInt("DoodleJumpCompleted", 0) == 1;
    }

    public void CompleteDoodleJump()
    {
        doodleJumpCompleted = true;
        PlayerPrefs.SetInt("DoodleJumpCompleted", 1);
        PlayerPrefs.Save();
    }


    public void UnloadPacManAndLoadCafeteria()
    {
        DirectionIndicatorScript.directionOfTask = "right"; //  taa strelkata so go pokazuva pravecot na sledniot task
        StartCoroutine(UnloadPacManAndLoadCafeteriaCoroutine());
    }
    private IEnumerator UnloadPacManAndLoadCafeteriaCoroutine()
    {
        // Mark Doodle Jump as completed
        pacManCompleted = true;
        PlayerPrefs.SetInt("PacManCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("GamePacMan");

        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        // Load Hallway
        yield return StartCoroutine(loadSceneReplaceCoroutine("Cafeteria"));
    }





    // In GameManagerScript.cs
    public void UnloadDoodleJumpAndLoadHallway()
    {
        DirectionIndicatorScript.directionOfTask = "right"; //  taa strelkata so go pokazuva pravecot na sledniot task
        StartCoroutine(UnloadDoodleJumpAndLoadHallwayCoroutine());
    }

    private IEnumerator UnloadDoodleJumpAndLoadHallwayCoroutine()
    {
        // Mark Doodle Jump as completed
        doodleJumpCompleted = true;
        PlayerPrefs.SetInt("DoodleJumpCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("DoodleJumpClassroom");

        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        // Load Hallway
        yield return StartCoroutine(loadSceneReplaceCoroutine("Hallway"));
    }

    public void UnloadDoodleJumpAndLoadClassroom(string taskSceneName)
    {
        StartCoroutine(UnloadDoodleJumpAndLoadClassroomCoroutine(taskSceneName));
    }

    private IEnumerator UnloadDoodleJumpAndLoadClassroomCoroutine(string taskSceneName)
    {
        // Mark Doodle Jump as completed
        doodleJumpCompleted = true;
        PlayerPrefs.SetInt("DoodleJumpCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("DoodleJumpClassroom");

        // Load Hallway
        //OD tuka
        string currentScene = "Hallway";


        // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol

        
        //spawnXByScene[currentScene] = 40.11f; //za classroom3
        




        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        var op = SceneManager.LoadSceneAsync("Classroom", LoadSceneMode.Single);
        while (!op.isDone) yield return null;   // wait until fully loaded
        yield return null;                      // let Awake/Start run this frame

        InstantiateTasksForScene(taskSceneName);

        positionPlayerOnScene(taskSceneName);

        transitionAnim.SetTrigger("Start");
        //yield return StartCoroutine(loadSceneReplaceCoroutine("Hallway"));
    }



    public void UnloadFlappyBirdAndLoadClassroom(string taskSceneName)
    {
        StartCoroutine(UnloadFlappyBirdAndLoadClassroomCoroutine(taskSceneName));
    }

    private IEnumerator UnloadFlappyBirdAndLoadClassroomCoroutine(string taskSceneName)
    {
        // Mark Doodle Jump as completed
        doodleJumpCompleted = true;
        PlayerPrefs.SetInt("FlappyBirdCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("Game");

        // Load Hallway

        //OD tuka
        string currentScene = "Hallway";

        

        // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol

        //if (taskSceneName == "Classroom1")
        //{
        //    spawnXByScene[currentScene] = -9.320066f;//za calssroom1

        //}
        //else if (taskSceneName == "Classroom4")
        //{
        //    spawnXByScene[currentScene] = 74.69f; //za calssroom4

        //}




        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        
        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        var op = SceneManager.LoadSceneAsync("Classroom", LoadSceneMode.Single);
        while (!op.isDone) yield return null;   // wait until fully loaded
        yield return null;                      // let Awake/Start run this frame

        InstantiateTasksForScene(taskSceneName);

        positionPlayerOnScene(taskSceneName);

        transitionAnim.SetTrigger("Start");
        //Do tuka

        //yield return StartCoroutine(loadSceneReplaceCoroutine("Hallway"));
    }

    public void UnloadPacManAndLoadClassroom(string taskSceneName)
    {
        StartCoroutine(UnloadPacManAndLoadClassroomCoroutine(taskSceneName));
    }

    private IEnumerator UnloadPacManAndLoadClassroomCoroutine(string taskSceneName)
    {
        // Mark Doodle Jump as completed
        pacManCompleted = true;
        PlayerPrefs.SetInt("PacManCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("GamePacMan");

        // Load Hallway

        //OD tuka
        string currentScene = "Hallway";


        // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol

        //spawnXByScene[currentScene] = 10.97f; // za classroom2


        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1f);

        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        var op = SceneManager.LoadSceneAsync("Classroom", LoadSceneMode.Single);
        while (!op.isDone) yield return null;   // wait until fully loaded
        yield return null;                      // let Awake/Start run this frame

        InstantiateTasksForScene(taskSceneName);

        positionPlayerOnScene(taskSceneName);

        transitionAnim.SetTrigger("Start");
        //Do tuka

        //yield return StartCoroutine(loadSceneReplaceCoroutine("Hallway"));
    }

    public void UnloadPacManAndLoadCafeteria(string taskSceneName)
    {
        //StartCoroutine(UnloadPacManAndLoadCafeteriaCoroutine(taskSceneName));
        pacManCompleted = true;
        PlayerPrefs.SetInt("PacManCompleted", 1);
        PlayerPrefs.Save();

        // Unload Doodle Jump scene
        SceneManager.UnloadSceneAsync("GamePacMan");

        // Load Hallway

        //OD tuka
        string currentScene = "Hallway";


        // ako doagja od nekoja scena(park, hodnik,classroom,cafeteria) togas da ja zacuva vrednosta od kaj dosol. KAJ da se vrati

        //spawnXByScene[currentScene] = 86.35f; // SMENI za cafeteria 


        //transitionAnim.SetTrigger("End");
        //yield return new WaitForSeconds(1f);

        audioManager.UnpauseMusic();
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);

        loadSceneReplace(taskSceneName);//taskSceneName = Cafeteria
    }

    //private IEnumerator UnloadPacManAndLoadCafeteriaCoroutine(string taskSceneName)
    //{
        
    //    pacManCompleted = true;
    //    PlayerPrefs.SetInt("PacManCompleted", 1);
    //    PlayerPrefs.Save();

    //    SceneManager.UnloadSceneAsync("GamePacMan");

    //    string currentScene = "Hallway";




    //    spawnXByScene[currentScene] = 0.0f; 

    //    loadSceneReplace(taskSceneName);
    //}

    

    public void unloadTaskSceneAfterWronglyAnsweredTask(string taskSceneName)
    {
        StartCoroutine(unloadTaskSceneAfterWronglyAnsweredTaskCoroutine(3, taskSceneName));

    }
    private IEnumerator unloadTaskSceneAfterWronglyAnsweredTaskCoroutine(int seconds, string taskSceneName)
    {
        //Debug.Log("TUKAAA");
        yield return new WaitForSeconds(seconds);

        // Track where we removed the task so we can add a new one in the SAME scene bucket
        bool removedFound = false;
        Vector3 removedPos = default;
        string removedClassroomKey = null;

        if (currentTaskObjectThatPlayerIsSolving)
        {
            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Classroom")
            {
                foreach (var classroom in new List<string> { "Classroom1", "Classroom2", "Classroom3", "Classroom4" })
                {
                    foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(activeTasks[classroom]))
                    {
                        if (kv.Value == currentTaskObjectThatPlayerIsSolving)
                        {
                            activeTasks[classroom].Remove(kv.Key);
                            removedFound = true;
                            removedPos = kv.Key;
                            removedClassroomKey = classroom;
                            break;
                        }
                    }
                    if (removedFound) break;
                }
            }
            else
            {
                foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(activeTasks[sceneName]))
                {
                    if (kv.Value == currentTaskObjectThatPlayerIsSolving)
                    {
                        activeTasks[sceneName].Remove(kv.Key);
                        removedFound = true;
                        removedPos = kv.Key;
                        break;
                    }
                }
            }

            Destroy(currentTaskObjectThatPlayerIsSolving);
            currentTaskObjectThatPlayerIsSolving = null;
        }

        // Close the task scene (SingleChoiceTask / DragAndDropTask / PhotoTask, etc.)
        SceneManager.UnloadSceneAsync(taskSceneName);

        // --- Add one more active task in the same scene bucket and spawn it ---
        // Determine the base scene and the correct activeTasks key to modify
        string baseScene = SceneManager.GetActiveScene().name;

        if (baseScene == "Classroom")
        {
            // We must add to the same classroom bucket we removed from (if known)
            string bucket = removedClassroomKey ?? "Classroom1"; // OVA NEMA POTREBA SEKAD KE E removedClassroomKey
            if (activeTasks.ContainsKey(bucket))// bucket e scene name
            {
                // Pool of possible positions comes from "Classroom"
                if (availablePositionsForTaskObjectsPerScene.TryGetValue("Classroom", out var pool) && pool != null && pool.Count > 0)
                {
                    // Prefer an unused position in this bucket; if all used, fall back to the removedPos (if captured) or any from pool
                    var used = activeTasks[bucket].Keys;
                    Vector3 newPos = removedFound ? removedPos : pool[UnityEngine.Random.Range(0, pool.Count)];
                    foreach (var p in pool)
                    {
                        if (!used.Contains(p))
                        {
                            newPos = p;
                            break;
                        }
                    }

                    // Add an empty slot and instantiate just this scene bucket
                    if (!activeTasks[bucket].ContainsKey(newPos))
                        activeTasks[bucket].Add(newPos, null);

                    InstantiateTasksForScene(bucket);
                }
                else
                {
                    Debug.LogWarning("No available positions configured for Classroom.");
                }
            }
        }
        else
        {
            // Non-classroom scene: add to the same scene key
            if (activeTasks.ContainsKey(baseScene))
            {
                if (availablePositionsForTaskObjectsPerScene.TryGetValue(baseScene, out var pool) && pool != null && pool.Count > 0)
                {
                    // Prefer a position not already in the dict; otherwise reuse the removed one (if known) or pick any
                    var used = activeTasks[baseScene].Keys;
                    Vector3 newPos = removedFound ? removedPos : pool[UnityEngine.Random.Range(0, pool.Count)];
                    foreach (var p in pool)
                    {
                        if (!used.Contains(p))
                        {
                            newPos = p;
                            break;
                        }
                    }

                    if (!activeTasks[baseScene].ContainsKey(newPos))
                        activeTasks[baseScene].Add(newPos, null);

                    InstantiateTasksForScene(baseScene);
                }
                else
                {
                    Debug.LogWarning($"No available positions configured for scene '{baseScene}'.");
                }
            }
        }

        BlockKeyboard = false;
    }


    public void setActivePauseAndCertificateButtons()
    {
        PauseButton.SetActive(true);
        CertificateButton.SetActive(true);
    }
    public void setInactivePauseAndCertificateButtons()
    {
        PauseButton.SetActive(false);
        CertificateButton.SetActive(false);
    }
    public void getNameFromInputField(string name)
    {
        playerName = name;
        //Debug.Log(playerName);
    }
    public string getPlayerName()
    {
        return playerName;
    }

    public void OnAgeGroupButtonClicked(Button clickedButton) //1 grupa: 6 do 11 god, 2 grupa:  12 do 15 god i 3 grupa:  16 do 18 god
    {
        TMP_Text textComponent = clickedButton.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            string selectedText = textComponent.text;
            //Debug.Log("|" + selectedText + "|");

            // Example logic
            if (selectedText == "6 - 11")
            {
                ageGroup = 1;

            }
            else if (selectedText == "12 - 15")
            {
                ageGroup = 2;
            }
            else if (selectedText == "16 - 18")
            {
                ageGroup = 3;
            }

        }
        LoadAllTaskBanks(ageGroup);
    }

    public void positionPlayerOnScene(string sceneName)
    {
        if (!spawnXByScene.TryGetValue(sceneName, out float targetX)) return;
        //Debug.Log(sceneName);
        //Debug.Log(targetX);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        //Debug.Log("najden player");
        var p = player.transform.position;
        //Debug.Log(p);
        player.transform.position = new Vector3(targetX, p.y, p.z);

    }








    // ova nadole e za taskObjects niz sceni
    public void BuildActiveTasks()
    {
        activeTasks.Clear();

        foreach (var kv in availablePositionsForTaskObjectsPerScene)
        {
            var sceneName = kv.Key;
            var positions = kv.Value;

            // Classroom is special: split into Classroom1..Classroom4 with 2 tasks each
            if (sceneName == "Classroom")
            {
                BuildClassrooms(sceneName, positions, 4, 2); // 4 rooms, 2 tasks each => 8 total
            }
            else
            {
                BuildScene(sceneName, positions, 5); // 5 tasks per non-classroom scene
            }
        }
    }

    void BuildScene(string sceneName, List<Vector3> available, int count)
    {
        if (available == null || available.Count == 0) return;

        var picked = PickUniqueRandom(available, Mathf.Min(count, available.Count));
        var dict = new Dictionary<Vector3, GameObject>();

        foreach (var pos in picked)
        {
            //var go = Instantiate(taskPrefab, pos, Quaternion.identity);
            //dict[pos] = go;
            dict[pos] = null;
        }

        activeTasks[sceneName] = dict;
    }

    void BuildClassrooms(string baseName, List<Vector3> available, int classroomCount, int tasksPerClassroom)
    {
        if (available == null || available.Count == 0) return;

        //int totalNeeded = Mathf.Min(classroomCount * tasksPerClassroom, available.Count);
        int totalNeeded = classroomCount * tasksPerClassroom;

        var shuffled = new List<Vector3>(available);
        Shuffle(shuffled);
        //var slice = shuffled.GetRange(0, totalNeeded);


        int idx = 0;
        int totalMade = 0;
        for (int c = 1; c <= classroomCount; c++)
        {
            string roomKey = $"{baseName}{c}";
            var dict = new Dictionary<Vector3, GameObject>();
            //int toPlace = Mathf.Min(tasksPerClassroom, totalNeeded - idx);
            int toPlace = 2;

            for (int k = 0; k < toPlace; k++)
            {
                var pos = shuffled[idx++];
                totalMade++;
                //var go = Instantiate(taskPrefab, pos, Quaternion.identity);
                //dict[pos] = go;
                dict[pos] = null;

                if (idx == available.Count - 1)
                {
                    idx = 0;
                }
            }

            activeTasks[roomKey] = dict;

            if (totalMade >= totalNeeded) break;
        }
    }

    List<Vector3> PickUniqueRandom(List<Vector3> source, int count)
    {
        var list = new List<Vector3>(source);
        Shuffle(list);
        if (count < list.Count) list.RemoveRange(count, list.Count - count);
        return list;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    //public void LogActiveTasks()
    //{
    //    if (activeTasks == null)
    //    {
    //        Debug.Log("activeTasks is null.");
    //        return;
    //    }

    //    var sb = new StringBuilder();
    //    sb.AppendLine($"ActiveTasks summary ({activeTasks.Count} scene keys):");

    //    foreach (var sceneKvp in activeTasks)
    //    {
    //        string scene = sceneKvp.Key;
    //        var dict = sceneKvp.Value;
    //        sb.AppendLine($"- {scene} -> {dict?.Count ?? 0} tasks");

    //        if (dict == null) continue;

    //        int i = 0;
    //        foreach (var taskKvp in dict)
    //        {
    //            Vector3 pos = taskKvp.Key;
    //            GameObject go = taskKvp.Value;
    //            string goName = go ? go.name : "null";
    //            bool isActive = go && go.activeInHierarchy;

    //            sb.AppendLine(
    //                $"    {i++}. pos=({pos.x:F2}, {pos.y:F2}, {pos.z:F2}) | go={goName} | activeInHierarchy={isActive}"
    //            );
    //        }
    //    }

    //    Debug.Log(sb.ToString());
    //}


    // Instantiate only the tasks for a given scene key, parented under 'parent' if provided
    public void InstantiateTasksForScene(string sceneKey)
    {
        //Debug.Log(sceneKey);
        if (!activeTasks.TryGetValue(sceneKey, out var dict)) return;

        foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(dict))
        {
            if (kv.Value == null)
            {
                //Debug.Log("tuka");
                var go = Instantiate(taskPrefab, kv.Key, Quaternion.identity);
                (string taskSceneName, string id) = GetRandomUniqueIdForTaskContent();
                go.GetComponent<TaskObjectIdScript>().SetTaskId(id);
                go.GetComponent<TaskObjectIdScript>().SetSceneName(taskSceneName);
                //Debug.Log(id + " " + taskSceneName);
                dict[kv.Key] = go;
            }
        }
    }

    // Destroy the spawned objects for a scene key, but keep the positions so we can re-spawn later
    public void DestroyTasksForScene(string sceneKey)
    {
        if (!activeTasks.TryGetValue(sceneKey, out var dict)) return;
        foreach (var kv in new List<KeyValuePair<Vector3, GameObject>>(dict))
        {
            if (kv.Value != null)
            {
                Destroy(kv.Value);
                dict[kv.Key] = null;
            }
        }
    }

    public void setCurrentTaskObjectThatPlayerIsSolving(GameObject taskObject)
    {
        currentTaskObjectThatPlayerIsSolving = taskObject;
    }
    public GameObject getCurrentTaskObjectThatPlayerIsSolving()
    {
        return currentTaskObjectThatPlayerIsSolving;
    }




    // ZA DA SE NAZNACAT IDS za task content, od jso-ot
    //private void InitializeAllIdsForTaskContent()
    //{
    //    allPossibleIdsForTaskContent["MultipleChoiceTask"] = new List<string>();
    //    allPossibleIdsForTaskContent["DragAndDropTask"] = new List<string>();
    //    allPossibleIdsForTaskContent["ImageTask"] = new List<string>();

    //    // multiple choice task: q001 - q034
    //    for (int i = 1; i <= 34; i++)
    //        allPossibleIdsForTaskContent["MultipleChoiceTask"].Add($"q{i:000}");

    //    // drag and drop task: q035 - q046
    //    for (int i = 35; i <= 46; i++)
    //        allPossibleIdsForTaskContent["DragAndDropTask"].Add($"q{i:000}");

    //    // image task: q047 - q054
    //    for (int i = 47; i <= 54; i++)
    //        allPossibleIdsForTaskContent["ImageTask"].Add($"q{i:000}");
    //}

    //public (string taskSceneName, string taskId) GetRandomUniqueIdForTaskContent()
    //{
    //    // If all IDs have been used, reset
    //    int totalIds = 34 + 12 + 8; // 54 total
    //    if (usedIdsForTaskContent.Count >= totalIds)
    //    {
    //        //Debug.Log("All task IDs have been used — resetting.");
    //        usedIdsForTaskContent.Clear();
    //    }

    //    // Build list of all available (scene, id) pairs
    //    List<(string scene, string id)> availableTasks = new List<(string, string)>(); // (ime na task scena, id)

    //    foreach (var kvp in allPossibleIdsForTaskContent)
    //    {
    //        foreach (var id in kvp.Value)
    //        {
    //            if (!usedIdsForTaskContent.Contains(id))
    //                availableTasks.Add((kvp.Key, id)); // (ime na task scena, id)
    //        }
    //    }

    //    if (availableTasks.Count == 0)
    //    {
    //        Debug.LogError("No available task IDs left!");
    //        return (null, null);
    //    }

    //    // Pick a random available pair
    //    int index = Random.Range(0, availableTasks.Count);
    //    var selected = availableTasks[index];

    //    usedIdsForTaskContent.Add(selected.id);

    //    //Debug.Log($"Selected Task Scene: {selected.scene}, ID: {selected.id}");

    //    return selected;
    //}

    private void LoadBank(TextAsset jsonAsset, string sceneKey, int ageGroup)
    {
        if (jsonAsset == null) { Debug.LogWarning($"Missing JSON for {sceneKey}"); return; }

        var wrapper = JsonUtility.FromJson<QuestionWrapper>(jsonAsset.text);


        foreach (var q in wrapper.questions)
        {

            if (string.IsNullOrEmpty(q.id)) continue;
            if (q.age_group == ageGroup)
                //Debug.Log(q.id);
                allPossibleIdsForTaskContent[sceneKey].Add(q.id);
        }
    }
    private void LoadAllTaskBanks(int ageGroup)
    {
        allPossibleIdsForTaskContent["SingleChoiceTask"].Clear();
        allPossibleIdsForTaskContent["DragAndDropTask"].Clear();
        allPossibleIdsForTaskContent["PhotoTask"].Clear();

        LoadBank(singleChoiceJson, "SingleChoiceTask", ageGroup);
        LoadBank(dragAndDropJson, "DragAndDropTask", ageGroup);
        LoadBank(imageTaskJson, "PhotoTask", ageGroup);

        usedIdsForTaskContent.Clear(); // new cohort → reset used IDs

        //Debug.Log($"Loaded IDs → MCQ:{allPossibleIdsForTaskContent["MultipleChoiceTask"].Count}  DnD:{allPossibleIdsForTaskContent["DragAndDropTask"].Count}  IMG:{allPossibleIdsForTaskContent["ImageTask"].Count}  for age {cohort}");
    }

    public (string taskSceneName, string taskId) GetRandomUniqueIdForTaskContent()
    {
        var available = new List<(string scene, string id)>();

        foreach (var kv in allPossibleIdsForTaskContent)
        {
            foreach (var id in kv.Value)
            {
                if (!usedIdsForTaskContent.Contains(id))
                    available.Add((kv.Key, id));
            }
        }

        if (available.Count == 0)
        {
            // all used → reset only the usage list, not the banks
            usedIdsForTaskContent.Clear();

            foreach (var kv in allPossibleIdsForTaskContent)
                foreach (var id in kv.Value)
                    available.Add((kv.Key, id));

            if (available.Count == 0)
            {
                Debug.LogError("No task IDs loaded. Check JSON assignments and ageGroup.");
                return (null, null);
            }
        }

        int idx = UnityEngine.Random.Range(0, available.Count);
        var chosen = available[idx];
        usedIdsForTaskContent.Add(chosen.id);
        return chosen;
    }


    public void disableCanvasForTheCrtificateAndPauseButtonTopRight() {
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(false);
    }

    public void enableCanvasForTheCrtificateAndPauseButtonTopRight()
    {
        canvasForTheCrtificateAndPauseButtonTopRight.gameObject.SetActive(true);
    }
}