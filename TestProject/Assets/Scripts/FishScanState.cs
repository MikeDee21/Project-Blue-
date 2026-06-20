using UnityEngine;

public class TestFish : MonoBehaviour
{
    public enum FishType
    {
        SmallFish,
        MediumFish,
        LargeFish,
        HostileFish
    }

    [Header("Fish Data")]
    public FishType fishType;
    public float scanCooldown;
    public bool alreadyScanned;
    public bool isLocked;
    public float lockTimer;

    private SpriteRenderer sr;
    private Color originalColor;

    [Header("Hover Highlight")]
    [Range(0f, 1f)]
    public float highlightAlpha = 0.25f;

    public Color highlightTint = new Color(0f, 1f, 1f, 1f);

    private void Update()
    {
        if (isLocked)
        {
            scanCooldown -= Time.deltaTime;

            if (scanCooldown <= 0f)
            {
                isLocked = false;
                scanCooldown = 0f;
            }
        }
    }

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
            originalColor = sr.color;
    }

    public void SetHighlighted(bool state)
    {
        if (sr == null) return;

        if (state)
        {
            // subtle cyan overlay
            Color c = highlightTint;
            c.a = highlightAlpha;
            sr.color = c;
        }
        else
        {
            sr.color = originalColor;
        }
    }

    public void LockScan(float duration)
    {
        isLocked = true;
        lockTimer = duration;
    }
}