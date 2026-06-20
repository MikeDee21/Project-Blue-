using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMemoryGame : MonoBehaviour
{
    [Header("UI ROOT")]
    [SerializeField] private GameObject scanMinigamePanelGO;
    [SerializeField] private RectTransform scanMinigamePanel;

    [SerializeField] private float slideDuration = 0.5f;

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;

    [Header("Buttons")]
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button yellowButton;

    private Image redImage;
    private Image blueImage;
    private Image greenImage;
    private Image yellowImage;

    [Header("Game Settings")]
    [SerializeField] private int sequenceLength = 4;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.4f;
    [SerializeField] private float flashDelay = 0.25f;

    private List<int> sequence = new List<int>();
    private int currentInputIndex;
    private bool acceptingInput;
    private bool isRunning;
    public static bool IsActive;

    // =========================
    // INIT
    // =========================
    private void Awake()
    {
        shownPosition = scanMinigamePanel.anchoredPosition;
        hiddenPosition = shownPosition + new Vector2(0, Screen.height);

        scanMinigamePanel.anchoredPosition = hiddenPosition;

        scanMinigamePanelGO.SetActive(false);
    }

    private void Start()
    {
        redImage = redButton.GetComponent<Image>();
        blueImage = blueButton.GetComponent<Image>();
        greenImage = greenButton.GetComponent<Image>();
        yellowImage = yellowButton.GetComponent<Image>();

        redButton.onClick.AddListener(() => PlayerInput(0));
        blueButton.onClick.AddListener(() => PlayerInput(1));
        greenButton.onClick.AddListener(() => PlayerInput(2));
        yellowButton.onClick.AddListener(() => PlayerInput(3));
    }

    // =========================
    // START MINIGAME
    // =========================
    public void StartScanMinigame()
    {
        IsActive = true;
        if (isRunning) return;

        isRunning = true;
        StopAllCoroutines();

        MiniGameState.Reset();

        sequence.Clear();
        currentInputIndex = 0;
        acceptingInput = false;

        for (int i = 0; i < sequenceLength; i++)
            sequence.Add(Random.Range(0, 4));

        StartCoroutine(RunMinigame());
    }

    // =========================
    // MAIN FLOW
    // =========================
    private IEnumerator RunMinigame()
    {
        scanMinigamePanelGO.SetActive(true);

        Time.timeScale = 0f;

        yield return SlidePanel(shownPosition);

        yield return ShowSequence();

        acceptingInput = true;
    }

    // =========================
    // SEQUENCE
    // =========================
    private IEnumerator ShowSequence()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        foreach (int i in sequence)
        {
            yield return FlashColor(i);
            yield return new WaitForSecondsRealtime(flashDelay);
        }
    }

    private IEnumerator FlashColor(int index)
    {
        Image target = GetImage(index);
        if (target == null) yield break;

        Color original = target.color;

        target.color = Color.white;
        yield return new WaitForSecondsRealtime(flashDuration);
        target.color = original;
    }

    private Image GetImage(int index)
    {
        return index switch
        {
            0 => redImage,
            1 => blueImage,
            2 => greenImage,
            3 => yellowImage,
            _ => null
        };
    }

    // =========================
    // INPUT
    // =========================
    public void PlayerInput(int colorIndex)
    {
        if (!acceptingInput) return;

        if (sequence[currentInputIndex] == colorIndex)
        {
            currentInputIndex++;

            if (currentInputIndex >= sequence.Count)
                WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    // =========================
    // END GAME
    // =========================
    private void WinGame()
    {
        IsActive = false;
        MiniGameState.ScanMinigameWon = true;
        EndMinigame();
    }

    private void LoseGame()
    {
        IsActive = false;
        MiniGameState.ScanMinigameLost = true;
        EndMinigame();
    }

    private void EndMinigame()
    {
        if (!isRunning) return;

        acceptingInput = false;
        StartCoroutine(CloseRoutine());
    }

    private IEnumerator CloseRoutine()
    {
        yield return SlidePanel(hiddenPosition);

        scanMinigamePanelGO.SetActive(false);

        Time.timeScale = 1f;

        isRunning = false;

        FindFirstObjectByType<PlayerController>()?.HandleScanResult();
    }

    // =========================
    // SLIDE SYSTEM
    // =========================
    private IEnumerator SlidePanel(Vector2 target)
    {
        Vector2 start = scanMinigamePanel.anchoredPosition;

        float t = 0f;

        while (t < slideDuration)
        {
            t += Time.unscaledDeltaTime;

            float p = Mathf.SmoothStep(0f, 1f, t / slideDuration);

            scanMinigamePanel.anchoredPosition =
                Vector2.Lerp(start, target, p);

            yield return null;
        }

        scanMinigamePanel.anchoredPosition = target;
    }

    public void CloseMinigame()
    {
        StartCoroutine(CloseRoutine());
    }
}