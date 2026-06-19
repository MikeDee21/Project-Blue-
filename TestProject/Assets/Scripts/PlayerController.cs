using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveForce;
    private float maxSpeed;

    private ColorMemoryGame colorMemoryGame;

    private Vector2 moveInput;

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
        ScanFish();
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            colorMemoryGame.StartScanMinigame();
        }
    }

}
