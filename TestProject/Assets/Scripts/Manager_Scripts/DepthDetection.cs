using System.Collections;
using TMPro;
using UnityEngine;

public class DepthDetection : MonoBehaviour
{
    public static DepthDetection instance;

    [SerializeField] private TextMeshProUGUI depthText;
    [SerializeField] private TextMeshProUGUI currZoneText;

    [SerializeField] private Player player;

    private Coroutine zoneCoroutine;

    private float depthMeter;

    private FishZone currentZone;

    // Initialize to an invalid enum value so the first zone is displayed.
    private FishZone previousZone = (FishZone)(-1);

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        currZoneText.text = "";
    }

    private void Update()
    {
        TrackCurrentDepth();
        UpdateDepthUI();
    }

    private void TrackCurrentDepth()
    {
        depthMeter = -player.GetPlayerTransform.position.y;

        if (depthMeter >= 7500)
        {
            currentZone = FishZone.Void;
        }
        else if (depthMeter >= 6000)
        {
            currentZone = FishZone.Blood;
        }
        else if (depthMeter >= 4000)
        {
            currentZone = FishZone.Hadal;
        }
        else if (depthMeter >= 1000)
        {
            currentZone = FishZone.Abyss;
        }
        else if (depthMeter >= 250)
        {
            currentZone = FishZone.Twilight;
        }
        else if (depthMeter >= 5)
        {
            currentZone = FishZone.Sunlight;
        }
        else
        {
            return; // Above the start threshold
        }

        // Only show the zone text when the zone changes.
        if (currentZone != previousZone)
        {
            previousZone = currentZone;
            DisplayCurrentLayerUI();
        }
    }

    private void DisplayCurrentLayerUI()
    {
        if (zoneCoroutine != null)
        {
            StopCoroutine(zoneCoroutine);
        }

        zoneCoroutine = StartCoroutine(ZoneTextDisplay());
    }

    private IEnumerator ZoneTextDisplay()
    {
        currZoneText.text = $"{currentZone} Zone";

        yield return new WaitForSecondsRealtime(3.5f);

        currZoneText.text = "";
    }

    private void UpdateDepthUI()
    {
        depthText.text = $"Current Depth: {depthMeter:F0} Meters";
    }

    public FishZone CurrentZone
    {
        get => currentZone;
    }

    public float CurrentDepth
    {
        get => depthMeter;
    }
}