using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SubmitButtonSingleChoiceTaskScript : MonoBehaviour
{
    //public Button submitButton = GetComponent<Button>();
    public TMP_Text sentenceText;
    public GameObject wordGrid;
    public GameObject leftColumn;
    public GameObject rightColumn;
    public GameObject closeButtonForSceneUnloading;

    public bool isEnglish = false;

    private GameManagerScript gameManager;

    public static string selectedWord = ""; // Holds the last selected word

    //private string correctAnswer = TextLoaderScriptForSingleChoiceQuestionTask.answer; // Hardcoded correct answer
    //private string originalQuestion = TextLoaderScriptForSingleChoiceQuestionTask.question;

    void Start()
    {
        //submitButton.onClick.AddListener(CheckAnswer);
        GetComponent<Button>().onClick.AddListener(CheckAnswer);
        gameManager = GameManagerScript.instance;
    }

    void CheckAnswer()
    {
        int languageIndex = PlayerPrefs.GetInt("Lang", 1);
        isEnglish = (languageIndex == 1);
        if (selectedWord == "")
        {
            if (isEnglish)
            {
                sentenceText.text = "You have not selected any of the suggested answers.";
            }
            else
            {
                sentenceText.text = "Немате одбрано ниту еден понуден одговор.";
            }
            StartCoroutine(ResetSentenceTextAfterDelay(2f));
            
        }else if (selectedWord == TextLoaderScriptForSingleChoiceQuestionTask.answer)
        {
            if (isEnglish)
            {
                sentenceText.text = "Bravo! Correct answer.";
            }
            else
            {
                sentenceText.text = "Браво! Точен одговор."; 
            }
            //GameManagerScript.instance.tasksCompleted++;
            AudioManagerScript instance = AudioManagerScript.instance;
            instance.playSFX(instance.taskCorrect);
            gameManager.unloadTaskSceneAfterCorrectlyFInishedTask("SingleChoiceTask");
        }
        else
        {
            if (isEnglish)
            {
                sentenceText.text = "Wrong answer.";
            }
            else
            {
                sentenceText.text = "Погрешен одговор.";
            }
            closeButtonForSceneUnloading.SetActive(false);

            StartCoroutine(ResetSentenceTextAfterDelay(2f));

            gameManager.unloadTaskSceneAfterWronglyAnsweredTask("SingleChoiceTask");
        }

    }
    private IEnumerator ResetSentenceTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //sentenceText.text = TextLoaderScriptForSingleChoiceQuestionTask.question;
        //LayoutRebuilder.ForceRebuildLayoutImmediate(wordGrid.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(leftColumn.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(rightColumn.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceText.transform.parent.parent.GetComponent<RectTransform>());
       
    }
    
}
//LayoutRebuilder.ForceRebuildLayoutImmediate(wordGrid.GetComponent<RectTransform>());
//LayoutRebuilder.ForceRebuildLayoutImmediate(explanationText.transform.parent.parent.GetComponent<RectTransform>());