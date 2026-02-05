using JetBrains.Annotations;
using System;
using System.Collections.Generic;

[Serializable]
public class Question
{
    public string id;
    public int age_group;
}
[Serializable]
public class QuestionWrapper
{
    public List<Question> questions;
}


// SINGLE CHOICE TASK
[Serializable]
public class SingleChoiceQuestion
{
    public string id;
    public int age_group;
    public string prompt; // prasanjeto
    public List<string> options;  // ["...", "...", ...]
    public int correct_index;     // indeks na tocno prasanje
    public string help;
    
}

[Serializable]
public class SingleChoiceWrapper
{
    public List<SingleChoiceQuestion> questions;
}







// DRAG AND DROP TASK
[Serializable]
public class DragAndDropItem
{
    public string left;   // e.g. "Име"
    public string right;  // e.g. "Безбедно е да го споделиш со пријателите"
    public string hint;   // e.g. "Општа информација..."
}

[Serializable]
public class DragAndDropQuestion
{
    public string id;
    public int age_group;
    public string prompt;
    public List<DragAndDropItem> pairs;
}

[Serializable]
public class DragAndDropWrapper
{
    public List<DragAndDropQuestion> questions;
}











//IMAGE TASK

[Serializable]
public class ImageQuestion
{
    public string id;
    public int age_group;
    public string prompt;
    public string text;            // the message content shown in the scenario
    public List<string> options;   // answer choices
    public int correct_index;      // correct option index
    public string help;            // explanation / hint
}

[Serializable]
public class ImageWrapper
{
    public List<ImageQuestion> questions;
}




[Serializable]
public class DoodleJumpQuestion
{
    public string id;
    public int age_group;
    public string prompt;
    public int required_score;
    public string help;
}

[Serializable]
public class DoodleJumpWrapper
{
    public List<DoodleJumpQuestion> questions;
}