using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SubmitScriptDragDropTask : MonoBehaviour
{
    // Reference to all drop zones (slots)
    public List<DroppableSlot> dropSlots;

    // Dictionary of correct matches: word → sentence text
    private Dictionary<string, string> correctMatches;
    public TextMeshProUGUI bottomText;

    public GameObject closeButtonForSceneUnloading;

    public bool isEnglish = false;

    // Call this from the Submit button OnClick
    public void CheckAnswers()
    {
        correctMatches = TextLoaderScriptForTaskCanvas.correctMatches;
        int howManyGreen = 0;
        foreach (var slot in dropSlots)
        {
            string correctSentence = "";
            string droppedWord = null;

            // Get the sentence text (assumed to be a child of parent of slot)
            TMP_Text sentenceText = slot.transform.parent.GetComponentInChildren<TMP_Text>();

            if (sentenceText == null)
                continue;

            correctSentence = sentenceText.text;

            Image sentenceBackground = slot.transform.parent.GetComponentInChildren<Image>();

            // Get the dropped word
            if (slot.transform.childCount > 1)
            {
                
                var child = slot.transform.GetChild(1);
                //if (child.GetComponent<DraggableItem>())
                //{
                TMP_Text wordText = child.GetComponentInChildren<TMP_Text>();
                if (wordText != null)
                    droppedWord = wordText.text;
                    
                //}
                
            }else {
                //NEMA STAVENO vo slotot odgovor
                sentenceBackground.color = new Color(0.917f, 0.627f, 0.627f);
                //bottomText.text = "The fields that are red are not correct.";
            }

                // Check correctness
                

            //if (sentenceBackground == null)
                //continue;

            if (droppedWord != null && correctMatches.ContainsKey(droppedWord))
            {
                if (correctMatches[droppedWord] == correctSentence)
                {
                    // ✅ Correct
                    sentenceBackground.color = new Color(0.625f, 0.917f, 0.607f);
                    howManyGreen++;
                    //bottomText.text = "Well done!!!";
                }
                else
                {
                    // ❌ Incorrect
                    sentenceBackground.color = new Color(0.917f, 0.627f, 0.627f);
                    //bottomText.text = "The fields that are red are not correct.";
                }
            }else{
                // ❌ No word dropped or unknown word
                sentenceBackground.color = new Color(0.917f, 0.627f, 0.627f);
                //bottomText.text = "The fields that are red are not correct.";
            }
        
        }
        GameManagerScript gameManager = GameManagerScript.instance;

        int languageIndex = PlayerPrefs.GetInt("Lang", 1);
        isEnglish = (languageIndex == 1);

        if (howManyGreen == 4)
        {
            if (isEnglish)
            {
                bottomText.text = "Well done!!!";
            }
            else
            {
                bottomText.text = "Браво!!!";
            }
            AudioManagerScript instance = AudioManagerScript.instance;
            instance.playSFX(instance.taskCorrect);
            gameManager.unloadTaskSceneAfterCorrectlyFInishedTask("DragAndDropTask");
        }
        else {
            if (isEnglish)
            {
                bottomText.text = howManyGreen + " out of 4 are correct.";
            }
            else
            {
                bottomText.text = "Точни се " + howManyGreen + " од 4.";
            }

            closeButtonForSceneUnloading.SetActive(false);
            StartCoroutine(putDelay(2f));
            gameManager.unloadTaskSceneAfterWronglyAnsweredTask("DragAndDropTask");

}
    }

    private IEnumerator putDelay(float delay) { 
        yield return new WaitForSeconds(delay); 
    }
}
