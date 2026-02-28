using TMPro;
using UnityEngine;

public class TextLoaderScriptForPhotoTaskExplanationCanvas : MonoBehaviour
{
    public TMP_Text explanationText;
    void Start()
    {
        explanationText.text = TextLoaderScriptForPhotoTaskScript.explanation;
       
    }


}
