using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager instance;

    [SerializeField] Player player; 

    [SerializeField] private List<Image> hullStrengthUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log($"Hull Boxes Count: {hullStrengthUI.Count}");
    }

    private void Update()
    {
        UpdateHull();
    }

    private void UpdateHull()
    {
        for (int i = 0; i < hullStrengthUI.Count; i++)
        {
            hullStrengthUI[i].enabled = i < player.GetCurrHP; 
        }
    }
}
