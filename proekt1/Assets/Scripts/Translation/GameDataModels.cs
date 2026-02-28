// DataModels.cs
using System.Collections.Generic;
using UnityEngine;

namespace GameDataModels
{
    [System.Serializable]
    public class Meta
    {
        public int version;
        public string locale;
        public string type;
    }

    [System.Serializable]
    public class SingleChoiceQuestion
    {
        public string id;
        public int age_group;
        public string prompt;
        public List<string> options;
        public int correct_index;
        public string help;
    }

    [System.Serializable]
    public class SingleChoiceWrapper
    {
        public Meta meta;
        public List<SingleChoiceQuestion> questions;
    }

    [System.Serializable]
    public class DragAndDropItem
    {
        public string left;
        public string right;
        public string hint;
    }

    [System.Serializable]
    public class DragAndDropQuestion
    {
        public string id;
        public int age_group;
        public string prompt;
        public List<DragAndDropItem> pairs;
    }

    [System.Serializable]
    public class DragAndDropWrapper
    {
        public Meta meta;
        public List<DragAndDropQuestion> questions;
    }

    [System.Serializable]
    public class ImageQuestion
    {
        public string id;
        public int age_group;
        public string prompt;
        public string text;
        public List<string> options;
        public int correct_index;
        public string help;
    }

    [System.Serializable]
    public class ImageWrapper
    {
        public Meta meta;
        public List<ImageQuestion> questions;
    }

    [System.Serializable]
    public class TranslationEntry
    {
        public string macedonian;
        public string english;
    }

    [System.Serializable]
    public class TranslationWrapper
    {
        public List<TranslationEntry> translations;
    }
}