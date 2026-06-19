using UnityEngine;

public class FishHostile : MonoBehaviour
{
    // fish movement
    public float detectionRange = 10f;
    public float chaseSpeed = 6f;
    public float patrolSpeed = 2f;

    // fish damage
    public int dmg = 1;
    public float attackCooldown = 3f;

    Transform player;
    bool canAttack = true;
    
    Vector3 patrolDirection;
    bool isStunned = false;
    bool isTouchingPlayer = false; // checker
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("FishHostile couldn't find a Player");
        }

        patrolDirection = Random.value > 0.5f ? Vector3.right : Vector3.left;
    }

    void Update()
    {
        if (isStunned || player == null)
            return;
        
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position += patrolDirection * patrolSpeed * Time.deltaTime;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (patrolDirection.x < 0);
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

            if(spriteRenderer != null)
            {
                spriteRenderer.flipX = (direction.x < 0);
            }

            if (isTouchingPlayer)
                return;

            transform.position += direction * chaseSpeed * Time.deltaTime;
    }
// Detect when the fish runs directly into the player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true;

            // Attack the player if the cooldown timer is ready
            if (canAttack)
            {
                Player playerScript = collision.GetComponent<Player>();
                if (playerScript != null)
                {
                    playerScript.DamagePlayer(dmg);
                    StartCoroutine(AttackCooldownCoroutine());
                }
            }
        }
    }

    // Reset movement lock when the player pulls away or moves out of bounds
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    System.Collections.IEnumerator StunCoroutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    System.Collections.IEnumerator AttackCooldownCoroutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
