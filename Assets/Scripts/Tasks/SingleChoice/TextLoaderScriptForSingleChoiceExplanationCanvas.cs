using TMPro;
using UnityEngine;

public class TextLoaderScriptForSingleChoiceExplanationCanvas : MonoBehaviour
{
    public TMP_Text explanationText;
    void Start()
    {
        explanationText.text = TextLoaderScriptForSingleChoiceQuestionTask.explanation;
    }

    
}
