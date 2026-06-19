using UnityEngine;

public class ScanMinigameResult : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MiniGameState.ScanMinigameWon)
        {
            Debug.Log("Give reward");

            MiniGameState.Reset();
        }

        if (MiniGameState.ScanMinigameLost)
        {
            Debug.Log("Restart puzzle");

            MiniGameState.Reset();
        }
    }
}

