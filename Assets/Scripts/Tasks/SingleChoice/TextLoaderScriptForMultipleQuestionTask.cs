// Updated TextLoaderScriptForSingleChoiceQuestionTask.cs
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLoaderScriptForSingleChoiceQuestionTask : MonoBehaviour
{
    public GameObject wordPrefab;
    public Transform columnGrid;
    public Transform columnLeft;
    public Transform columnRight;
    public TMP_Text sentenceText;

    private string IdOfCurrentTaskObjectThatPlayerIsSolving;

    // Remove singleChoiceJson reference since we'll get it from JSONManager
    // public TextAsset singleChoiceJson;

    // Add UI feedback text
    private string correctAnswerText = "Точен одговор. Браво!!!";
    private string wrongAnswerText = "Погрешен одговор.";

    public static List<string> wordList;
    public static string question;
    public static string answer;
    public static string explanation;

    void Start()
    {
        InitializeUIText();

        IdOfCurrentTaskObjectThatPlayerIsSolving = GameManagerScript.instance.getCurrentTaskObjectThatPlayerIsSolving().GetComponent<TaskObjectIdScript>().TaskId;
        loadContentFromJson();

        SpawnWords();
        SetSentenceTexts();

        LayoutRebuilder.ForceRebuildLayoutImmediate(columnGrid.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(columnLeft.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(columnRight.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceText.transform.parent.parent.GetComponent<RectTransform>());
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

    void SpawnWords()
    {
        int idx = 0;

        // Clear existing children
        foreach (Transform child in columnLeft)
            Destroy(child.gameObject);
        foreach (Transform child in columnRight)
            Destroy(child.gameObject);

        // Left column: up to 2
        for (int i = 0; i < 2 && idx < wordList.Count; i++, idx++)
            SpawnTo(wordList[idx], columnLeft);

        // Right column: up to 2
        for (int i = 0; i < 2 && idx < wordList.Count; i++, idx++)
            SpawnTo(wordList[idx], columnRight);
    }

    private void SpawnTo(string word, Transform parent)
    {
        GameObject go = Instantiate(wordPrefab, parent);
        var tmp = go.GetComponentInChildren<TMP_Text>();
        if (tmp) tmp.text = word;
    }

    void SetSentenceTexts()
    {
        sentenceText.text = question;
    }

    public void loadContentFromJson()
    {
        // Get the appropriate JSON file based on language
        TextAsset jsonFile = JSONManager.Instance.GetMultipleChoiceJSON();

        if (jsonFile == null)
        {
            Debug.LogWarning("Missing JSON");
            return;
        }

        var wrapper = JsonUtility.FromJson<GameDataModels.SingleChoiceWrapper>(jsonFile.text);

        foreach (var q in wrapper.questions)
        {
            if (string.IsNullOrEmpty(q.id)) continue;

            if (q.id == IdOfCurrentTaskObjectThatPlayerIsSolving)
            {
                // Directly use the text from JSON (already translated if English)
                question = q.prompt;
                wordList = new List<string>(q.options);
                explanation = q.help;

                if (q.correct_index >= 0 && q.correct_index < q.options.Count)
                {
                    answer = wordList[q.correct_index];
                }
                break;
            }
        }
    }

    // Method to handle language changes
    void OnLanguageChanged()
    {
        ReloadWithLanguage();
    }

    public void ReloadWithLanguage()
    {
        // Update UI text
        InitializeUIText();

        // Reload content
        loadContentFromJson();

        // Respawn words with new language
        SpawnWords();
        SetSentenceTexts();

        // Rebuild layouts
        LayoutRebuilder.ForceRebuildLayoutImmediate(columnGrid.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(columnLeft.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(columnRight.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(sentenceText.transform.parent.parent.GetComponent<RectTransform>());
    }
}