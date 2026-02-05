using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class JSONTranslator : MonoBehaviour
{
    [System.Serializable]
    public class TranslationEntry
    {
        public string macedonian;
        public string english;
    }

    [Header("Translation Files")]
    public TextAsset translationFile;

    [Header("JSON Files to Translate")]
    public List<TextAsset> jsonFilesToTranslate;

    private Dictionary<string, string> translationDictionary = new Dictionary<string, string>();
    private bool isEnglish = false;

    void Start()
    {
        // Load language setting
        int languageIndex = PlayerPrefs.GetInt("Lang", 1);
        isEnglish = (languageIndex == 1); // Assuming 1 is English

        // Load translations
        if (translationFile != null)
        {
            LoadTranslations();
        }

        // Apply translations if English is selected
        if (isEnglish)
        {
            TranslateAllJSONs();
        }
    }

    private void LoadTranslations()
    {
        try
        {
            string jsonContent = translationFile.text;
            var wrapper = JsonUtility.FromJson<TranslationWrapper>(jsonContent);

            if (wrapper != null && wrapper.translations != null)
            {
                foreach (var entry in wrapper.translations)
                {
                    if (!string.IsNullOrEmpty(entry.macedonian) && !string.IsNullOrEmpty(entry.english))
                    {
                        string key = entry.macedonian.Trim().ToLower();
                        if (!translationDictionary.ContainsKey(key))
                        {
                            translationDictionary.Add(key, entry.english);
                        }
                    }
                }
                Debug.Log($"Loaded {translationDictionary.Count} translations from file");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading translation file: {e.Message}");
        }
    }

    private void TranslateAllJSONs()
    {
        foreach (var jsonFile in jsonFilesToTranslate)
        {
            if (jsonFile != null)
            {
                string translatedJson = TranslateJSON(jsonFile.text);
                // You can save this to a temporary file or use it directly
                Debug.Log($"Translated {jsonFile.name}");
            }
        }
    }

    public string TranslateJSON(string jsonContent)
    {
        if (!isEnglish || string.IsNullOrEmpty(jsonContent))
            return jsonContent;

        // First, try to translate entire JSON with all text fields
        string translated = jsonContent;

        // Translate text in JSON structure (this is a simplified approach)
        translated = TranslateTextInJSON(translated);

        return translated;
    }

    private string TranslateTextInJSON(string json)
    {
        // This is a simplified approach. For a more robust solution,
        // you'd want to parse the JSON structure properly.

        // Replace Macedonian text with English translations
        foreach (var kvp in translationDictionary)
        {
            // Escape special characters for JSON
            string macedonianEscaped = JsonEscape(kvp.Key);
            string englishEscaped = JsonEscape(kvp.Value);

            // Simple replacement (this might replace partial words, so be careful)
            json = json.Replace(macedonianEscaped, englishEscaped);

            // Also try with quotes
            json = json.Replace($"\"{kvp.Key}\"", $"\"{kvp.Value}\"");
        }

        return json;
    }

    private string JsonEscape(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return text.Replace("\\", "\\\\")
                   .Replace("\"", "\\\"")
                   .Replace("\n", "\\n")
                   .Replace("\r", "\\r")
                   .Replace("\t", "\\t");
    }

    public string TranslateSentence(string macedonianText)
    {
        if (!isEnglish || string.IsNullOrEmpty(macedonianText))
            return macedonianText;

        string translated = macedonianText;

        // Split into words and translate each
        string[] words = macedonianText.Split(' ', ',', '.', '!', '?', ';', ':', '-', '\n', '\r', '\t');

        foreach (string word in words)
        {
            string cleanWord = word.Trim().ToLower();
            if (!string.IsNullOrEmpty(cleanWord) && translationDictionary.ContainsKey(cleanWord))
            {
                string englishWord = translationDictionary[cleanWord];
                // Replace the word (case-insensitive)
                translated = ReplaceWord(translated, word, englishWord, StringComparison.OrdinalIgnoreCase);
            }
        }

        return translated;
    }

    private string ReplaceWord(string original, string oldWord, string newWord, StringComparison comparison)
    {
        int index = original.IndexOf(oldWord, comparison);
        if (index >= 0)
        {
            return original.Remove(index, oldWord.Length).Insert(index, newWord);
        }
        return original;
    }

    // Method to get translated JSON content
    public string GetTranslatedJSON(TextAsset jsonFile)
    {
        if (jsonFile == null)
            return "";

        if (!isEnglish)
            return jsonFile.text;

        return TranslateJSON(jsonFile.text);
    }

    // Method to manually trigger translation
    public void ForceTranslateAll()
    {
        TranslateAllJSONs();
    }

    // Method to add custom translations at runtime
    public void AddTranslation(string macedonian, string english)
    {
        string key = macedonian.Trim().ToLower();
        if (!translationDictionary.ContainsKey(key))
        {
            translationDictionary.Add(key, english);
        }
        else
        {
            translationDictionary[key] = english;
        }
    }

    [System.Serializable]
    private class TranslationWrapper
    {
        public List<TranslationEntry> translations;
    }
}