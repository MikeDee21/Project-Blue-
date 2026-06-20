using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveForce;
    private float maxSpeed;

    public float interactDistance = 3f;
    private TestFish lastScannedFish;

    private ColorMemoryGame colorMemoryGame;
    private TestFish currentFish;

    private Vector2 moveInput;

    public Camera playerCamera;
    public GameObject scanPromptUI;
    private Rigidbody2D rb;

    [SerializeField] private float holdTime = 1.5f;
    [SerializeField] private UnityEngine.UI.Slider holdBarUI;

    private float holdTimer;
    private bool isHolding;
    private float scanProgress;
    [SerializeField] private float scanSpeed = 1f;
    [SerializeField] private float scanDecay = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        colorMemoryGame = GetComponent<ColorMemoryGame>();

        moveForce = 5f;
        maxSpeed = 25f;
    }

    private void Update()
    {
        GetMoveInput();
        MouseTracker();
        HandleHoverUI();
        HandleScanHold();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.AddForce(moveInput * moveForce, ForceMode2D.Force);
    }

    private void GetMoveInput()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void MouseTracker()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 direction = mousePos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ScanFish()
    {
        if (currentFish == null)
            return;

        TestFish fish = currentFish;

        float distance = Vector2.Distance(transform.position, fish.transform.position);

        if (distance > interactDistance)
            return;

        if (fish.alreadyScanned)
        {
            Debug.Log("ALREADY SCANNED");
            return;
        }

        if (fish.isLocked)
        {
            Debug.Log("FISH ON COOLDOWN");
            return;
        }

        Debug.Log("STARTING MINIGAME: " + fish.name);

        lastScannedFish = fish;

        colorMemoryGame.StartScanMinigame();
    }

    private void HandleHoverUI()
    {
        if (ColorMemoryGame.IsActive)
        {
            if (currentFish != null)
                currentFish.SetHighlighted(false);

            currentFish = null;
            scanPromptUI.SetActive(false);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        TestFish newFish = null;

        if (hit.collider != null)
            newFish = hit.collider.GetComponentInParent<TestFish>();

        // ✔ if hovered fish changed, remove highlight from old one
        if (newFish != currentFish)
        {
            if (currentFish != null)
                currentFish.SetHighlighted(false);

            currentFish = newFish;
        }

        bool valid = false;

        if (currentFish != null)
        {
            float distance = Vector2.Distance(transform.position, currentFish.transform.position);
            valid =
                distance <= interactDistance &&
                !currentFish.isLocked &&
                !currentFish.alreadyScanned;

            currentFish.SetHighlighted(valid);
        }

        scanPromptUI.SetActive(valid);
    }

    private void ResetHold()
    {
        holdTimer = 0f;

        if (holdBarUI != null)
        {
            holdBarUI.value = 0;
            holdBarUI.gameObject.SetActive(false);
        }
    }

    private void HandleScanHold()
    {
        if (ColorMemoryGame.IsActive)
        {
            ResetScan();
            return;
        }

        if (currentFish == null)
        {
            ResetScan();
            return;
        }

        float distance = Vector2.Distance(transform.position, currentFish.transform.position);

        bool validTarget =
            distance <= interactDistance &&
            !currentFish.isLocked &&
            !currentFish.alreadyScanned;

        if (Input.GetKey(KeyCode.E) && validTarget)
        {
            scanProgress += Time.deltaTime * scanSpeed;
        }
        else
        {
            scanProgress -= Time.deltaTime * scanDecay;
        }

        scanProgress = Mathf.Clamp01(scanProgress);

        holdBarUI.gameObject.SetActive(scanProgress > 0f);
        holdBarUI.value = scanProgress;

        if (scanProgress >= 1f)
        {
            CompleteScan();
        }
    }

    private void CompleteScan()
    {
        if (currentFish == null)
        {
            ResetScan();
            return;
        }

        scanProgress = 0f;
        holdBarUI.value = 0f;
        holdBarUI.gameObject.SetActive(false);

        ScanFish();
    }

    private void ResetScan()
    {
        scanProgress = 0f;
        holdBarUI.value = 0f;
        holdBarUI.gameObject.SetActive(false);
    }

    public void HandleScanResult()
    {
        if (lastScannedFish == null)
            return;

        if (MiniGameState.ScanMinigameWon)
        {
            StartCoroutine(DespawnFish(lastScannedFish));
        }
        else if (MiniGameState.ScanMinigameLost)
        {
            lastScannedFish.isLocked = true;
            lastScannedFish.scanCooldown = 15f;
        }

        MiniGameState.ScanMinigameWon = false;
        MiniGameState.ScanMinigameLost = false;

        lastScannedFish = null;
    }

    private System.Collections.IEnumerator DespawnFish(TestFish fish)
    {
        float t = 0f;

        Vector3 startScale = fish.transform.localScale;

        while (t < 1f)
        {
            t += Time.deltaTime;

            fish.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);

            yield return null;
        }

        Destroy(fish.gameObject);
    }
}