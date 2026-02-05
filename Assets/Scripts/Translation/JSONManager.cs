// JSONManager.cs
using UnityEngine;
using System.Collections.Generic;

public class JSONManager : MonoBehaviour
{
    public static JSONManager Instance { get; private set; }

    [Header("Macedonian JSON Files")]
    public TextAsset multipleChoiceJSON_MK;
    public TextAsset dragAndDropJSON_MK;
    public TextAsset imageMCQJSON_MK;
    public TextAsset doodleJumpJSON_MK;

    [Header("English JSON Files")]
    public TextAsset multipleChoiceJSON_EN;
    public TextAsset dragAndDropJSON_EN;
    public TextAsset imageMCQJSON_EN;
    public TextAsset doodleJumpJSON_EN;

    private bool isEnglish = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        CheckLanguage();
    }

    void CheckLanguage()
    {
        int languageIndex = PlayerPrefs.GetInt("Lang", 1);
        isEnglish = (languageIndex == 1); // Assuming 1 = English
    }

    public TextAsset GetMultipleChoiceJSON()
    {
        return isEnglish && multipleChoiceJSON_EN != null ? multipleChoiceJSON_EN : multipleChoiceJSON_MK;
    }

    public TextAsset GetDragAndDropJSON()
    {
        return isEnglish && dragAndDropJSON_EN != null ? dragAndDropJSON_EN : dragAndDropJSON_MK;
    }

    public TextAsset GetImageMCQJSON()
    {
        return isEnglish && imageMCQJSON_EN != null ? imageMCQJSON_EN : imageMCQJSON_MK;
    }

    public TextAsset GetDoodleJumpJSON()
    {
        return isEnglish && doodleJumpJSON_EN != null ? doodleJumpJSON_EN : doodleJumpJSON_MK;
    }

    public bool IsEnglish()
    {
        return isEnglish;
    }

    public void ReloadLanguage()
    {
        CheckLanguage();
        // Notify all scripts to reload
        BroadcastMessage("OnLanguageChanged", SendMessageOptions.DontRequireReceiver);
    }
}