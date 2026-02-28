using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class TextLoaderScriptForExplanationCanvas : MonoBehaviour
{
    public Transform wordGrid; // Parent of the Word buttons (Grid)
    public TMP_Text explanationText; // The TMP_Text under Sentence/Image
    public GameObject wordPrefab;              // Prefab with TMP_Text and Button
    
    

    void Start()
    {
        SpawnExplanationWords();
        //Layout Rebuild
        LayoutRebuilder.ForceRebuildLayoutImmediate(wordGrid.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(explanationText.transform.parent.parent.GetComponent<RectTransform>());
    }


    void SpawnExplanationWords()
    {
        foreach (KeyValuePair<string, string> pair in TextLoaderScriptForTaskCanvas.explanationOfWords)
        {
            GameObject newWord = Instantiate(wordPrefab, wordGrid);
            TMP_Text tmp = newWord.GetComponentInChildren<TMP_Text>();
            Button button = newWord.GetComponent<Button>();

            tmp.text = pair.Key;

            string explanation = pair.Value; // capture value for closure
            button.onClick.AddListener(() =>
            {
                explanationText.text = explanation;
            });
            
        }
    }
}

