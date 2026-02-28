using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [Header("Main Canvases")]
    public GameObject taskCanvas;
    public GameObject explanationCanvas;

    [Header("Buttons")]
    public GameObject questionMarkButton;
    public GameObject closeButtonForSceneUnloading;
    //public GameObject closeButton;

    void Start()
    {
        // Initial State
        taskCanvas.SetActive(true);
        explanationCanvas.SetActive(false);
        questionMarkButton.SetActive(true);

        // Add listener to QuestionMarkButton
        questionMarkButton.GetComponent<Button>().onClick.AddListener(OnQuestionMarkButtonClicked);
        //closeButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClicked);
    }

    public void OnQuestionMarkButtonClicked()
    {
        explanationCanvas.SetActive(true);
        taskCanvas.SetActive(false);
        
        questionMarkButton.SetActive(false);
        closeButtonForSceneUnloading.SetActive(false);


    }
    public void OnCloseButtonClicked()
    {
        explanationCanvas.SetActive(false);
        taskCanvas.SetActive(true);
        
        questionMarkButton.SetActive(true);
        closeButtonForSceneUnloading.SetActive(true);
    }
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("DragAndDropTask");
        GameManagerScript.instance.BlockKeyboard = false;
    }
}
