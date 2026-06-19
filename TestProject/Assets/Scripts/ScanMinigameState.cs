using UnityEngine;

public static class MiniGameState
{
    public static bool ScanMinigameWon;
    public static bool ScanMinigameLost;

    public static void Reset()
    {
        ScanMinigameWon = false;
        ScanMinigameLost = false;
    }
}
