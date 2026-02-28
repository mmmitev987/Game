using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScriptCertificate : MonoBehaviour
{
    public List<GameObject> pieces;
    public GameObject nameObject;
    public GameObject dateObject;

    [Header("Auto-close settings")]
    public float autoCloseDelay = 3f; // Time in seconds before auto-closing

    // Track which pieces have already been activated (persistent across certificate views)
    private static List<int> activatedPieceIndices = new List<int>();

    void Start()
    {
        // If this is the first time viewing certificate, initialize
        if (activatedPieceIndices.Count == 0 && GameManagerScript.instance.tasksCompleted > 0)
        {
            // For existing progress, activate random pieces
            InitializeRandomPieces();
        }
        else
        {
            // Update with current progress
            UpdateCertificatePieces();
        }

        // Set name and date
        nameObject.GetComponent<TextMeshProUGUI>().text = GameManagerScript.instance.getPlayerName();
        dateObject.GetComponent<TextMeshProUGUI>().text = DateTime.Now.ToString("MMMM d, yyyy", new System.Globalization.CultureInfo("mk-MK"));

        // Start auto-close timer
        Invoke("AutoCloseCertificate", autoCloseDelay);
    }

    private void InitializeRandomPieces()
    {
        activatedPieceIndices.Clear();

        int totalPiecesToActivate = Mathf.Clamp(GameManagerScript.instance.tasksCompleted, 0, pieces.Count);

        // Create a list of all possible indices
        List<int> allIndices = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
        {
            allIndices.Add(i);
        }

        // Shuffle the indices
        ShuffleList(allIndices);

        // Take the first 'totalPiecesToActivate' indices
        for (int i = 0; i < totalPiecesToActivate; i++)
        {
            if (i < allIndices.Count)
            {
                activatedPieceIndices.Add(allIndices[i]);
            }
        }

        // Update visual pieces
        UpdateVisualPieces();
    }

    private void UpdateCertificatePieces()
    {
        int currentProgress = GameManagerScript.instance.tasksCompleted;

        // If we have more progress than activated pieces, add new random ones
        if (currentProgress > activatedPieceIndices.Count)
        {
            int piecesToAdd = currentProgress - activatedPieceIndices.Count;
            AddNewRandomPieces(piecesToAdd);
        }
        // If progress somehow decreased (shouldn't happen), remove excess pieces
        else if (currentProgress < activatedPieceIndices.Count)
        {
            activatedPieceIndices.RemoveRange(currentProgress, activatedPieceIndices.Count - currentProgress);
        }

        // Update visual pieces
        UpdateVisualPieces();
    }

    private void AddNewRandomPieces(int count)
    {
        // Create a list of indices that are NOT yet activated
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < pieces.Count; i++)
        {
            if (!activatedPieceIndices.Contains(i))
            {
                availableIndices.Add(i);
            }
        }

        // Shuffle available indices
        ShuffleList(availableIndices);

        // Add new random pieces (up to available count)
        int piecesToActuallyAdd = Mathf.Min(count, availableIndices.Count);
        for (int i = 0; i < piecesToActuallyAdd; i++)
        {
            activatedPieceIndices.Add(availableIndices[i]);
        }
    }

    private void UpdateVisualPieces()
    {
        // First, deactivate ALL pieces
        foreach (GameObject piece in pieces)
        {
            if (piece != null)
                piece.SetActive(false);
        }

        // Then activate only the ones in our activated list
        foreach (int index in activatedPieceIndices)
        {
            if (index >= 0 && index < pieces.Count && pieces[index] != null)
            {
                pieces[index].SetActive(true);
            }
        }
    }

    // Fisher-Yates shuffle algorithm
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    // Static method to reset when starting new game
    public static void ResetCertificate()
    {
        activatedPieceIndices.Clear();
    }

    private void AutoCloseCertificate()
    {
        closeScene();
    }

    public void closeScene()
    {
        SceneManager.UnloadSceneAsync("Certificate");
        GameManagerScript.instance.BlockKeyboard = false;
    }
}