using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WhichWordIsSelectedScript : MonoBehaviour
{
    public string wordText; // Automatically assigned from TMP

    void Start()
    {
        wordText = GetComponentInChildren<TMP_Text>().text;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        SubmitButtonSingleChoiceTaskScript.selectedWord = wordText; // Store globally
    }
}
