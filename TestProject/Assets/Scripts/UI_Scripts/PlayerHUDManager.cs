using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public static PlayerHUDManager instance;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform playerTransform;

    private float shakeTimer;
    private float shakeDuration = 0.25f;
    private float shakeMagnitude = 0.2f;

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

    public void ShakeScreen()
    {
        shakeTimer = shakeDuration;
    }

    private void LateUpdate()
    {
        Vector3 basePos = playerTransform.position;
        basePos.z = mainCamera.transform.position.z;

        Vector3 shakeOffset = Vector3.zero;

        if (shakeTimer > 0)
        {
            shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeMagnitude,
                Random.Range(-1f, 1f) * shakeMagnitude,
                0
            );

            shakeTimer -= Time.deltaTime;
        }

        mainCamera.transform.position = basePos + shakeOffset;
    }
}
