using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveForce;
    private float maxSpeed;

    public float interactDistance = 3f;

    private ColorMemoryGame colorMemoryGame;
    private TestFish currentFish;

    private Vector2 moveInput;

    public Camera playerCamera;
    public GameObject scanPromptUI;

    [SerializeField] private Rigidbody2D rb;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            ScanFish();
        }
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
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider == null)
        {
            Debug.Log("NO HIT");
            return;
        }

        Debug.Log("HIT: " + hit.collider.name);

        TestFish fish = hit.collider.GetComponentInParent<TestFish>();

        if (fish == null)
        {
            Debug.Log("HIT OBJECT IS NOT FISH");
            return;
        }

        float distance = Vector2.Distance(transform.position, fish.transform.position);

        if (distance > interactDistance)
        {
            Debug.Log("TOO FAR");
            return;
        }

        if (fish.alreadyScanned)
        {
            Debug.Log("ALREADY SCANNED");
            return;
        }

        Debug.Log("STARTING MINIGAME: " + fish.name);

        fish.alreadyScanned = true;

        colorMemoryGame.StartScanMinigame();
    }

    private void HandleHoverUI()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        TestFish newFish = null;

        if (hit.collider != null)
        {
            newFish = hit.collider.GetComponentInParent<TestFish>();
        }

        // handle hover switching ONLY
        if (newFish != currentFish)
        {
            if (currentFish != null)
                currentFish.SetHighlighted(false);

            currentFish = newFish;
        }

        // continuously evaluate current hovered fish
        if (currentFish != null)
        {
            float distance = Vector2.Distance(transform.position, currentFish.transform.position);

            bool valid = distance <= interactDistance && !currentFish.alreadyScanned;

            // highlight updates EVERY FRAME
            currentFish.SetHighlighted(valid);

            // UI logic
            scanPromptUI.SetActive(valid);
        }
        else
        {
            scanPromptUI.SetActive(false);
        }
    }
}