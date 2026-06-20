using UnityEngine;
using UnityEngine.UI;

public class DepthLighting : MonoBehaviour
{
    [SerializeField] private Image darknessOverlay;
    [SerializeField] private float transitionSpeed = 2f;

    private void Update()
    {
        float targetAlpha = 0f;

        switch (DepthDetection.instance.CurrentZone)
        {
            case FishZone.Sunlight:
                targetAlpha = 0f;
                break;

            case FishZone.Twilight:
                targetAlpha = 0.15f;
                break;

            case FishZone.Midnight:
                targetAlpha = 0.35f;
                break;

            case FishZone.Abyss:
                targetAlpha = 0.55f;
                break;

            case FishZone.Hadal:
                targetAlpha = 0.75f;
                break;

            case FishZone.Blood:
                targetAlpha = 0.83f;
                break;

            case FishZone.Void:
                targetAlpha = 0.91f;
                break;
        }

        Color c = darknessOverlay.color;

        c.a = Mathf.Lerp(
            c.a,
            targetAlpha,
            Time.deltaTime * transitionSpeed
        );

        darknessOverlay.color = c;
    }
}