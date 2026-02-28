// Updated TextLoaderScriptForTaskCanvas.cs
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLoaderScriptForTaskCanvas : MonoBehaviour
{
    public GameObject wordPrefab;
    public Transform wordsParent;
    public List<Transform> sentanceColumns;
    public List<Transform> forLayoutRebuild;

    private string IdOfCurrentTaskObjectThatPlayerIsSolving;

    // Remove this and use JSONManager instead
    // public TextAsset dragAndDropJson;

    public static List<string> wordList;
    public static Dictionary<string, string> correctMatches;
    public static Dictionary<string, string> explanationOfWords;

    void Start()
    {
        IdOfCurrentTaskObjectThatPlayerIsSolving = GameManagerScript.instance.getCurrentTaskObjectThatPlayerIsSolving().GetComponent<TaskObjectIdScript>().TaskId;
        loadContentFromJson();

        SpawnWords();
        SetSentenceTexts();

        foreach (Transform column in sentanceColumns)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(column.GetComponent<RectTransform>());

            foreach (Transform t in forLayoutRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(t.GetComponent<RectTransform>());
            }
        }
    }

    void SpawnWords()
    {
        // Clear existing words
        foreach (Transform child in wordsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (string word in wordList)
        {
            GameObject newWord = Instantiate(wordPrefab, wordsParent);

            TMP_Text textComponent = newWord.GetComponentInChildren<TMP_Text>();
            textComponent.text = word;
        }
    }

    void SetSentenceTexts()
    {
        List<Transform> allSentances = new List<Transform>();

        foreach (Transform column in sentanceColumns)
        {
            foreach (Transform sentance in column)
            {
                allSentances.Add(sentance);
            }
        }

        List<string> sentenceTexts = correctMatches.Values.ToList();
        for (int i = 0; i < allSentances.Count && i < sentenceTexts.Count; i++)
        {
            Transform sentance = allSentances[i];
            TMP_Text text = sentance.Find("Image").GetComponentInChildren<TMP_Text>();
            text.text = sentenceTexts[i];
        }
    }

    public void loadContentFromJson()
    {
        wordList = new List<string>();
        correctMatches = new Dictionary<string, string>();
        explanationOfWords = new Dictionary<string, string>();

        // Get the appropriate JSON file based on language
        TextAsset jsonFile = JSONManager.Instance.GetDragAndDropJSON();

        if (jsonFile == null)
        {
            Debug.LogWarning("Missing JSON");
            return;
        }

        var wrapper = JsonUtility.FromJson<GameDataModels.DragAndDropWrapper>(jsonFile.text);

        foreach (var q in wrapper.questions)
        {
            if (string.IsNullOrEmpty(q.id)) continue;

            if (q.id == IdOfCurrentTaskObjectThatPlayerIsSolving)
            {
                List<GameDataModels.DragAndDropItem> tmp = q.pairs;

                foreach (var item in tmp)
                {
                    // Directly use the text (already translated if English)
                    wordList.Add(item.left);
                    correctMatches.Add(item.left, item.right);
                    explanationOfWords.Add(item.left, item.hint);
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
        loadContentFromJson();
        SpawnWords();
        SetSentenceTexts();

        foreach (Transform column in sentanceColumns)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(column.GetComponent<RectTransform>());

            foreach (Transform t in forLayoutRebuild)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(t.GetComponent<RectTransform>());
            }
        }
    }
}