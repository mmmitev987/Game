using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasSwithcerPhotoTaskScript : MonoBehaviour
{
    [Header("Main Canvases")]
    public GameObject taskCanvas;
    public GameObject explanationCanvas;

    [Header("Buttons")]
    public GameObject closeButtonForSceneUnloading;

    //public GameObject closeButton;

    void Start()
    {
        // Initial State
        taskCanvas.SetActive(true);
        explanationCanvas.SetActive(false);
        //questionMarkButton.SetActive(true);

        // Add listener to QuestionMarkButton
        //questionMarkButton.GetComponent<Button>().onClick.AddListener(OnQuestionMarkButtonClicked);
        //closeButton.GetComponent<Button>().onClick.AddListener(OnCloseButtonClicked);
    }

    public void OnQuestionMarkButtonClicked()
    {
        explanationCanvas.SetActive(true);
        taskCanvas.SetActive(false);
        closeButtonForSceneUnloading.SetActive(false);



    }
    public void OnCloseButtonClicked()
    {
        explanationCanvas.SetActive(false);
        taskCanvas.SetActive(true);
        closeButtonForSceneUnloading.SetActive(true);

    }
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("PhotoTask");
        GameManagerScript.instance.BlockKeyboard = false;
    }
}
