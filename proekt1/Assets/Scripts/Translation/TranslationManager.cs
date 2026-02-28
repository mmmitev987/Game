// TranslationManager.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class TranslationManager : MonoBehaviour
{
    public static TranslationManager Instance { get; private set; }

    [Header("Translation Files")]
    public TextAsset translationJSON;

    [Header("Game JSON Files")]
    public TextAsset multipleChoiceJSON;
    public TextAsset dragAndDropJSON;
    public TextAsset imageMCQJSON;
    public TextAsset doodleJumpJSON;

    // Complete Macedonian to English dictionary
    private Dictionary<string, string> macedonianToEnglish = new Dictionary<string, string>();
    private bool isEnglishSelected = false;

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
            return;
        }

        InitializeTranslations();
    }

    void Start()
    {
        CheckLanguageSetting();
    }

    private void InitializeTranslations()
    {
        // First, load basic translations
        LoadBasicTranslations();

        // Then load from JSON file if available
        if (translationJSON != null)
        {
            LoadTranslationsFromJSON();
        }

        Debug.Log($"Translation Manager initialized with {macedonianToEnglish.Count} translations");
    }

    private void LoadBasicTranslations()
    {
        // Basic words and common phrases
        AddTranslation("здраво", "hello");
        AddTranslation("да", "yes");
        AddTranslation("не", "no");
        AddTranslation("благодарам", "thank you");
        AddTranslation("ве молам", "please");
        AddTranslation("извинете", "excuse me");
        AddTranslation("како си", "how are you");
        AddTranslation("добро сум", "I'm fine");
        AddTranslation("се викам", "my name is");
        AddTranslation("од каде си", "where are you from");
        AddTranslation("сум од", "I'm from");

        // Common game terms from your JSONs
        AddTranslation("лозинка", "password");
        AddTranslation("име", "name");
        AddTranslation("адреса", "address");
        AddTranslation("телефон", "phone");
        AddTranslation("интернет", "internet");
        AddTranslation("безбедно", "safe");
        AddTranslation("небезбедно", "unsafe");
        AddTranslation("споделиш", "share");
        AddTranslation("пријатели", "friends");
        AddTranslation("непознат", "stranger");
        AddTranslation("возрасен", "adult");
        AddTranslation("наставник", "teacher");
        AddTranslation("родител", "parent");
        AddTranslation("профил", "profile");
        AddTranslation("игра", "game");
        AddTranslation("видео", "video");
        AddTranslation("слика", "picture");
        AddTranslation("порака", "message");
        AddTranslation("чат", "chat");
        AddTranslation("група", "group");
        AddTranslation("приватно", "private");
        AddTranslation("јавно", "public");
        AddTranslation("вирус", "virus");
        AddTranslation("антивирус", "antivirus");
        AddTranslation("хакирање", "hacking");
        AddTranslation("измама", "scam");
        AddTranslation("булинг", "bullying");
        AddTranslation("навреда", "insult");
        AddTranslation("пријави", "report");
        AddTranslation("блокирај", "block");
        AddTranslation("докази", "evidence");
        AddTranslation("скриншот", "screenshot");
        AddTranslation("безбедност", "security");
        AddTranslation("приватност", "privacy");
        AddTranslation("податоци", "data");
        AddTranslation("личен", "personal");
        AddTranslation("информации", "information");
        AddTranslation("веб страница", "website");
        AddTranslation("веб-страница", "website");
        AddTranslation("линк", "link");
        AddTranslation("https", "https");
        AddTranslation("wi-fi", "wi-fi");
        AddTranslation("е-пошта", "email");
        AddTranslation("фишинг", "phishing");
        AddTranslation("малвер", "malware");
        AddTranslation("кетфишинг", "catfishing");
        AddTranslation("дипфејк", "deepfake");
        AddTranslation("шифрирање", "encryption");

        // Specific phrases from your JSON files
        AddTranslation("Ја добив оваа порака на телефонот, што да направам?", "I received this message on my phone, what should I do?");
        AddTranslation("Честитки! По случаен избор добитник сте на најновиот ајфон 17!", "Congratulations! You have been randomly selected as the winner of the latest iPhone 17!");
        AddTranslation("Ве молиме потврдете го вашиот идентитет со одговор на оваа порака.", "Please confirm your identity by responding to this message.");
        AddTranslation("Имате само 24 часа да ја подигнете наградата инаку ќе биде предадена на друг.", "You only have 24 hours to claim the prize, otherwise it will be given to someone else.");
        AddTranslation("да одговорам веднаш со моите податоци", "to respond immediately with my information");
        AddTranslation("да не одговорам бидејќи не знам кој ме контактира", "not to respond because I don't know who contacted me");
        AddTranslation("да му кажам на возрасен", "to tell an adult");
        AddTranslation("Некои пораки на интернет или телефон велат дека си добил награда", "Some messages on the internet or phone say you've won a prize");
        AddTranslation("тие се лажни", "they are fake");
        AddTranslation("Целта им е да ти земат лични податоци или лозинки", "Their goal is to take your personal information or passwords");
        AddTranslation("Најбезбедно е да не одговараш и да му кажеш на родител или наставник", "It's safest not to respond and to tell a parent or teacher");
    }

    private void LoadTranslationsFromJSON()
    {
        try
        {
            string json = translationJSON.text;
            var wrapper = JsonUtility.FromJson<GameDataModels.TranslationWrapper>(json);

            if (wrapper != null && wrapper.translations != null)
            {
                foreach (var entry in wrapper.translations)
                {
                    AddTranslation(entry.macedonian, entry.english);
                }
                Debug.Log($"Loaded {wrapper.translations.Count} translations from JSON");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading translation JSON: {e.Message}");
        }
    }

    public void CheckLanguageSetting()
    {
        int languageIndex = PlayerPrefs.GetInt("Lang", 1);
        isEnglishSelected = (languageIndex == 1); // Assuming 1 = English
    }

    public bool IsEnglishSelected()
    {
        return isEnglishSelected;
    }

    public string TranslateText(string macedonianText)
    {
        if (!isEnglishSelected || string.IsNullOrEmpty(macedonianText))
            return macedonianText;

        string result = macedonianText;

        // First, try to match whole phrases (longest first)
        var sortedKeys = macedonianToEnglish.Keys.OrderByDescending(k => k.Length);
        foreach (var phrase in sortedKeys)
        {
            if (result.Contains(phrase))
            {
                result = result.Replace(phrase, macedonianToEnglish[phrase]);
            }
        }

        // If text still contains Macedonian characters, try word-by-word
        if (ContainsMacedonianCharacters(result))
        {
            result = TranslateWordByWord(result);
        }

        return result;
    }

    private bool ContainsMacedonianCharacters(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;

        // Macedonian Cyrillic range
        foreach (char c in text)
        {
            // Check if character is in Macedonian Cyrillic range
            if ((c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') ||
                c == 'Ѕ' || c == 'ѕ' || c == 'Ќ' || c == 'ќ' ||
                c == 'Ѓ' || c == 'ѓ' || c == 'Љ' || c == 'љ' ||
                c == 'Њ' || c == 'њ' || c == 'Џ' || c == 'џ')
            {
                return true;
            }
        }
        return false;
    }

    private string TranslateWordByWord(string text)
    {
        // Split by words and punctuation
        string[] words = Regex.Split(text, @"(\s+|[.,!?;:""'()\[\]{}])");

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i].Trim();
            if (string.IsNullOrEmpty(word)) continue;

            string cleanWord = CleanWord(word).ToLower();

            if (macedonianToEnglish.ContainsKey(cleanWord))
            {
                string translation = macedonianToEnglish[cleanWord];

                // Preserve original capitalization
                if (char.IsUpper(words[i][0]))
                {
                    translation = CapitalizeFirstLetter(translation);
                }

                // Preserve if the whole word was uppercase
                if (words[i].All(c => char.IsUpper(c)))
                {
                    translation = translation.ToUpper();
                }

                words[i] = words[i].Replace(CleanWord(words[i]), translation);
            }
        }

        return string.Join("", words);
    }

    private string CleanWord(string word)
    {
        // Remove common punctuation
        char[] punctuation = { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}' };
        return word.Trim(punctuation);
    }

    private string CapitalizeFirstLetter(string word)
    {
        if (string.IsNullOrEmpty(word)) return word;
        return char.ToUpper(word[0]) + word.Substring(1);
    }

    public void AddTranslation(string macedonian, string english)
    {
        if (string.IsNullOrEmpty(macedonian) || string.IsNullOrEmpty(english))
            return;

        string key = macedonian.Trim().ToLower();
        if (!macedonianToEnglish.ContainsKey(key))
        {
            macedonianToEnglish.Add(key, english);
        }
    }

    public void ReloadTranslations()
    {
        CheckLanguageSetting();

        // Notify all translation-dependent scripts to refresh
        BroadcastMessage("OnLanguageChanged", SendMessageOptions.DontRequireReceiver);
    }

    // Get translated JSON content
    public string GetTranslatedJSON(TextAsset jsonFile)
    {
        if (!isEnglishSelected || jsonFile == null)
            return jsonFile?.text ?? "";

        return TranslateJSONContent(jsonFile.text);
    }

    private string TranslateJSONContent(string jsonContent)
    {
        // This is a simple implementation - for production, you'd want to properly parse the JSON
        string translated = jsonContent;

        // Translate text fields (between quotes)
        var matches = Regex.Matches(jsonContent, "\"([^\"]+)\"");
        foreach (Match match in matches)
        {
            string original = match.Groups[1].Value;
            if (ContainsMacedonianCharacters(original))
            {
                string translatedText = TranslateText(original);
                translated = translated.Replace($"\"{original}\"", $"\"{translatedText}\"");
            }
        }

        return translated;
    }

    // Get translated question data
    public GameDataModels.SingleChoiceQuestion GetTranslatedQuestion(string questionId)
    {
        if (multipleChoiceJSON == null) return null;

        var wrapper = JsonUtility.FromJson<GameDataModels.SingleChoiceWrapper>(multipleChoiceJSON.text);
        var question = wrapper.questions.Find(q => q.id == questionId);

        if (question != null && isEnglishSelected)
        {
            // Create a translated copy
            var translated = new GameDataModels.SingleChoiceQuestion
            {
                id = question.id,
                age_group = question.age_group,
                prompt = TranslateText(question.prompt),
                options = question.options.Select(TranslateText).ToList(),
                correct_index = question.correct_index,
                help = TranslateText(question.help)
            };
            return translated;
        }

        return question;
    }
}