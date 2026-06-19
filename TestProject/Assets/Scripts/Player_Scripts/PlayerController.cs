using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveForce;
    private float maxSpeed;
    public float interactDistance = 3f;

    private ColorMemoryGame colorMemoryGame;

    private Vector2 moveInput;

    public Camera playerCamera;
    public GameObject scanPromptUI;

    [SerializeField]private Rigidbody2D rb; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }
    private void Start()
    {
        colorMemoryGame = GetComponent<ColorMemoryGame>();
        moveForce = 5f;
        maxSpeed = 25; 
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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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

        if (fish.alreadyScanned)
        {
            Debug.Log("ALREADY SCANNED");
            return;
        }

        Debug.Log("STARTING MINIGAME ON: " + fish.name);

        float distance = Vector2.Distance(transform.position, fish.transform.position);

        if (distance > interactDistance)
        {
            Debug.Log("Too far to scan fish");
            return;
        }

        fish.alreadyScanned = true;
        colorMemoryGame.StartScanMinigame();
    }

    void HandleHoverUI()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider == null)
        {
            scanPromptUI.SetActive(false);
            return;
        }

        TestFish fish = hit.collider.GetComponentInParent<TestFish>();

        if (fish == null || fish.alreadyScanned)
        {
            scanPromptUI.SetActive(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, fish.transform.position);

        if (distance <= interactDistance)
        {
            scanPromptUI.SetActive(true);
        }
        else
        {
            scanPromptUI.SetActive(false);
        }
    }


}
