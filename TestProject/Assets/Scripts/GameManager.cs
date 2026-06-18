using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// 
///  Make sure to attach PauseUICanvas prefab when dragging the GameManager prefab onto the scene otherwise Pause Menu will not work properly
///  
/// </summary>
public class GameManager : MonoBehaviour
{
   public static GameManager instance;

    public GameObject pauseUIPrefab; 

    private bool isGameOver;
    private bool isGamePaused;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }

        else
        {
            instance = this; 
        }

    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver == true)
                return; 

            if (isGamePaused == false)
                PauseGame();

            else 
                ResumeGame();
;       }

        TrackGameOver();

    }

    private void ResumeGame()
    {
        isGamePaused = false;

        SetNormalTime();

        pauseUIPrefab.SetActive(false);
    }

    private void PauseGame()
    {
        isGamePaused = true;

        StopTime();

        pauseUIPrefab.SetActive(true);
    }

    private void StopTime()
    {
        Time.timeScale = 0;
    }

    private void SlowTime(float t)
    {
        if (isGamePaused == true)
            return;

        Time.timeScale = t;
    }

    private void SetNormalTime()
    {
        Time.timeScale = 1;
    }

    private void TrackGameOver()
    {
        if (isGameOver == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void BTN_SceneSwapper(string sceneName)
    {
        SceneManager.LoadScene(sceneName); 
    }

    public void BTN_ResumeGame()
    {
        if (isGameOver == true)
            return; 

        if (isGamePaused == true)
        {
            ResumeGame();
        }
    }


    public void CallIsGameOver()
    {
        StartCoroutine(DoGameOver()); 
    }

    private IEnumerator DoGameOver()
    {
        //Display Death Text before reloading scene
        SlowTime(0.7f);

        yield return new WaitForSecondsRealtime(5.0f);

        isGameOver = true;
    }
}


   


