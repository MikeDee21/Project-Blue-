using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int currHullStrength;
    [SerializeField] private int maxHullStrength;

    private Transform playerTransform; 
    void Start()
    {
        currHullStrength = 3;
        maxHullStrength = currHullStrength;

        gameObject.SetActive(true);
    }

 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DamagePlayer(1); 
        }

        TrackPlayerStatus();
    }

    private void TrackPlayerStatus()
    {
        if (currHullStrength <= 0)
        {
            GameManager.instance.CallIsGameOver(); 
            gameObject.SetActive(false); 
        }
    }

    public Transform GetPlayerTransform
    {
        get => transform; 
    }
    public int GetCurrHP 
    {
        get => currHullStrength; 

    }
    public void DamagePlayer(int dmg)
    {
        currHullStrength -= dmg;
        if (PlayerHUDManager.instance != null)
        {
            PlayerHUDManager.instance.ShakeScreen();
        }
    }
}
