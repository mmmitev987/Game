// Updated TextLoaderScriptForPhotoTaskScript.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLoaderScriptForPhotoTaskScript : MonoBehaviour
{
    public GameObject optionPrefab;
    public Transform optionsGrid;
    public TMP_Text messageTextElement;
    public TMP_Text nextToKidMessageElement;

    private string IdOfCurrentTaskObjectThatPlayerIsSolving;
    public GameObject closeButtonForSceneUnloading;

    // Remove this and use JSONManager instead
    // public TextAsset photoTaskJson;

    public static string explanation;
    private string message;
    private List<string> options;
    private static string answer;
    private static string prompt;
    private GameManagerScript gameManager;

    private string correctAnswerText = "Точен одговор. Браво!!!";
    private string wrongAnswerText = "Погрешен одговор.";

    void Start()
    {
        InitializeUIText();

        gameManager = GameManagerScript.instance;
        IdOfCurrentTaskObjectThatPlayerIsSolving = gameManager.getCurrentTaskObjectThatPlayerIsSolving().GetComponent<TaskObjectIdScript>().TaskId;
        loadContentFromJson();

        SpawnOptions();
        SetMessageTexts();

        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsGrid.GetComponent<RectTransform>());
    }

    private void InitializeUIText()
    {
        if (JSONManager.Instance != null && JSONManager.Instance.IsEnglish())
        {
            correctAnswerText = "Correct answer. Well done!!!";
            wrongAnswerText = "Wrong answer.";
        }
        else
        {
            correctAnswerText = "Точен одговор. Браво!!!";
            wrongAnswerText = "Погрешен одговор.";
        }
    }

    void SpawnOptions()
    {
        // Clear existing options
        foreach (Transform child in optionsGrid)
        {
            Destroy(child.gameObject);
        }

        foreach (string option in options)
        {
            GameObject newWord = Instantiate(optionPrefab, optionsGrid);

            TMP_Text textComponent = newWord.GetComponentInChildren<TMP_Text>();
            textComponent.text = option;

            Button button = newWord.GetComponentInChildren<Button>();

            // Add listener with the option text
            string optionText = option;
            button.onClick.AddListener(() => WhenOptionIsSelected(optionText));
        }
    }

    void SetMessageTexts()
    {
        messageTextElement.text = message;
        nextToKidMessageElement.text = prompt;
    }

    public void WhenOptionIsSelected(string selectedOption)
    {
        if (selectedOption == answer)
        {
            nextToKidMessageElement.text = correctAnswerText;
            AudioManagerScript instance = AudioManagerScript.instance;
            instance.playSFX(instance.taskCorrect);
            gameManager.unloadTaskSceneAfterCorrectlyFInishedTask("PhotoTask");
        }
        else
        {
            nextToKidMessageElement.text = wrongAnswerText;
            closeButtonForSceneUnloading.SetActive(false);
            StartCoroutine(putDelay(2f));
            gameManager.unloadTaskSceneAfterWronglyAnsweredTask("PhotoTask");
        }

        // Disable all buttons after selection
        foreach (Transform child in optionsGrid)
        {
            Button btn = child.GetComponentInChildren<Button>();
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
    }

    private IEnumerator putDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void loadContentFromJson()
    {
        // Get the appropriate JSON file based on language
        TextAsset jsonFile = JSONManager.Instance.GetImageMCQJSON();

        if (jsonFile == null)
        {
            Debug.LogWarning("Missing JSON");
            return;
        }

        var wrapper = JsonUtility.FromJson<GameDataModels.ImageWrapper>(jsonFile.text);

        foreach (var q in wrapper.questions)
        {
            if (string.IsNullOrEmpty(q.id)) continue;

            if (q.id == IdOfCurrentTaskObjectThatPlayerIsSolving)
            {
                // Directly use the text (already translated if English)
                message = q.text;
                options = new List<string>(q.options);
                prompt = q.prompt;
                explanation = q.help;

                if (q.correct_index >= 0 && q.correct_index < q.options.Count)
                {
                    answer = options[q.correct_index];
                }
                break;
            }
        }
    }

    void OnLanguageChanged()
    {
        ReloadWithLanguage();
    }

    public void ReloadWithLanguage()
    {
        InitializeUIText();
        loadContentFromJson();
        SpawnOptions();
        SetMessageTexts();
        LayoutRebuilder.ForceRebuildLayoutImmediate(optionsGrid.GetComponent<RectTransform>());
    }
}