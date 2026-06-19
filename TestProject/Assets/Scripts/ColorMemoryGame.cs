using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMemoryGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject minigameUI;

    [Header("Buttons")]
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button yellowButton;

    [Header("Flash Settings")]
    public float flashDuration = 0.5f;
    public float flashDelay = 0.25f;

    [Header("Game Settings")]
    public int sequenceLength = 4;

    private List<int> sequence = new List<int>();

    private Image redImage;
    private Image blueImage;
    private Image greenImage;
    private Image yellowImage;

    private int currentInputIndex;
    private bool acceptingInput;

    void Start()
    {
        // Hide UI at game start
        if (minigameUI != null)
            minigameUI.SetActive(false);

        // Cache button images
        redImage = redButton.GetComponent<Image>();
        blueImage = blueButton.GetComponent<Image>();
        greenImage = greenButton.GetComponent<Image>();
        yellowImage = yellowButton.GetComponent<Image>();

        // Button listeners
        redButton.onClick.AddListener(() => PlayerInput(0));
        blueButton.onClick.AddListener(() => PlayerInput(1));
        greenButton.onClick.AddListener(() => PlayerInput(2));
        yellowButton.onClick.AddListener(() => PlayerInput(3));
    }

    public void StartScanMinigame()
    {
        MiniGameState.Reset();

        // Show UI when minigame starts
        if (minigameUI != null)
            minigameUI.SetActive(true);

        sequence.Clear();
        currentInputIndex = 0;
        acceptingInput = false;

        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(Random.Range(0, 4));
        }

        StartCoroutine(ShowSequence());
    }

    IEnumerator ShowSequence()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (int color in sequence)
        {
            yield return FlashColor(color);
            yield return new WaitForSeconds(flashDelay);
        }

        acceptingInput = true;
    }

    IEnumerator FlashColor(int colorIndex)
    {
        Image target = GetImage(colorIndex);

        Color originalColor = target.color;

        target.color = Color.white;
        yield return new WaitForSeconds(flashDuration);
        target.color = originalColor;
    }

    Image GetImage(int index)
    {
        switch (index)
        {
            case 0: return redImage;
            case 1: return blueImage;
            case 2: return greenImage;
            case 3: return yellowImage;
        }

        return null;
    }

    public void PlayerInput(int colorIndex)
    {
        if (!acceptingInput)
            return;

        if (colorIndex == sequence[currentInputIndex])
        {
            currentInputIndex++;

            if (currentInputIndex >= sequence.Count)
            {
                WinGame();
            }
        }
        else
        {
            LoseGame();
        }
    }

    void WinGame()
    {
        acceptingInput = false;

        MiniGameState.ScanMinigameWon = true;

        if (minigameUI != null)
            minigameUI.SetActive(false);

        Debug.Log("Memory Game Won");
    }

    void LoseGame()
    {
        acceptingInput = false;

        MiniGameState.ScanMinigameLost = true;

        if (minigameUI != null)
            minigameUI.SetActive(false);

        Debug.Log("Memory Game Lost");
    }
}
