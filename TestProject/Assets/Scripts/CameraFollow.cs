using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [Header("Camera follow")]
    public Transform target;
    public float smoothSpeed = 1.5f;

    [Header("Cursor camera")]
    public bool enableCursorPush = true;
    public float edgeSize = 0.2f;
    public float pushAmount = 2f;

    private Vector3 followOffset;
    private Vector3 cursorOffset;
    private Vector3 shakeOffset;

    private Camera cam;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cam = Camera.main;
    }

    void LateUpdate()
    {
        UpdateCursorOffset();

        Vector3 finalPos =
            GetFollowPosition()
            + followOffset
            + cursorOffset
            + shakeOffset;

        transform.position =
            Vector3.Lerp(
                transform.position,
                finalPos,
                smoothSpeed * Time.deltaTime
            );
    }

    Vector3 GetFollowPosition()
    {
        if (target == null)
            return transform.position;

        return new Vector3(
            target.position.x,
            target.position.y,
            -10
        );
    }

    void UpdateCursorOffset()
    {
        if (!enableCursorPush)
        {
            cursorOffset = Vector3.zero;
            return;
        }

        Vector3 mouse =
            cam.ScreenToViewportPoint(
                Input.mousePosition
            );

        cursorOffset = Vector3.zero;

        if (mouse.x > 1 - edgeSize)
            cursorOffset.x = pushAmount;

        if (mouse.x < edgeSize)
            cursorOffset.x = -pushAmount;

        if (mouse.y > 1 - edgeSize)
            cursorOffset.y = pushAmount;

        if (mouse.y < edgeSize)
            cursorOffset.y = -pushAmount;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void EnableCursorPush(bool value)
    {
        enableCursorPush = value;
    }

    public void Shake(float duration, float intensity)
    {
        StartCoroutine(
            ShakeRoutine(
                duration,
                intensity
            )
        );
    }

    IEnumerator ShakeRoutine(
        float duration,
        float intensity
    )
    {
        float timer = 0;

        while (timer < duration)
        {
            Vector2 random =
                Random.insideUnitCircle
                * intensity;

            shakeOffset =
                new Vector3(
                    random.x,
                    random.y,
                    0
                );

            timer += Time.deltaTime;

            yield return null;
        }

        shakeOffset = Vector3.zero;
    }
}