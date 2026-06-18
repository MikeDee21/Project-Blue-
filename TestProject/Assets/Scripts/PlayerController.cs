using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveForce;
    private float maxSpeed;

    private Vector2 moveInput;

    [SerializeField]private Rigidbody2D rb; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }
    private void Start()
    {
        moveForce = 5f;
        maxSpeed = 25; 
    }
    private void Update()
    {
        GetMoveInput(); 
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
}
