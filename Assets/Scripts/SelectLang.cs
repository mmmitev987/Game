using UnityEngine;
using System.Globalization;
using System.Threading;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SelectLang : MonoBehaviour
{
    public GameObject langPanel;
    private bool isChanging = false;

    public void SelectLanguage(int index)
    {
        PlayerPrefs.SetInt("Lang", index);

        if (index == 0)
        {
            StartCoroutine(ChangeLanguage(1));
        }
        else
        {
            StartCoroutine(ChangeLanguage(0));
        }

        langPanel.SetActive(false);
    }

    private System.Collections.IEnumerator ChangeLanguage(int index)
    {
        isChanging = true;

        // Wait for Localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Get the locale
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = selectedLocale;

        isChanging = false;
    }
}