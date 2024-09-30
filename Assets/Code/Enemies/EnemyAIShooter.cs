using Pathfinding;
using UnityEngine;

public class EnemyAIShooter : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    //public float Enspeed = 10f;
    public float detectionRadius = 10f; // Distance for enemy to start following
    public float stoppingDistance = 5f; // Distance from player where enemy will stop moving
    public float retreatSpeed = 2f; // Speed at which the enemy retreats when player is too close

    private AstarPathfinding pathfinding; // Reference to your A* pathfinding script
    private bool isFollowingPlayer = false; // Is the enemy following the player?
    private EnemyShooter enemyShooter; // Reference to the EnemyShooter component
        private bool isAlive = true;  // To track if the enemy is alive
    

    [Header("Dependencies")]
    Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] EnemyHealth enemyHealth;  // Reference to the EnemyHealth script


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfinding = GetComponent<AstarPathfinding>();
        enemyShooter = GetComponent<EnemyShooter>(); // Get the shooter component
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();

    }

    void Update()
    {
        if (enemyHealth.GetCurrentHealth() <= 0)
        {
            isAlive = false;  // Set alive status to false
            enemyShooter.EnableShooting(false);  // Disable shooting
            StopMoving();  // Stop movement
            return;  // Exit the update if the enemy is dead
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            enemyShooter.EnableShooting(true); // Enable shooting if the player is inside detection radius
            //anim.SetTrigger("Shoot");
        }
        else
        {
            enemyShooter.EnableShooting(false); // Disable shooting if the player is outside detection radius
        }

        // If player is within detection range, start following
         if (distanceToPlayer < detectionRadius && distanceToPlayer > stoppingDistance)
        {
            isFollowingPlayer = true;
            //pathfinding.speed = Enspeed;
            anim.SetBool("isWalk", true);
            anim.SetBool("isWalkBack", false);
            pathfinding.FollowPlayerPath();
            CalculateFacingDirection();
            
        }
        else if (distanceToPlayer < stoppingDistance) 
        {
            isFollowingPlayer = false;
            anim.SetBool("isWalk", true);
            anim.SetBool("isWalkBack", true);
            RetreatFromPlayer(); // Start retreating when too close
            
            
        }
        else
        {
            isFollowingPlayer = false;
            pathfinding.StopMoving(); // Stop moving when out of range
            anim.SetBool("isWalk", false);
            anim.SetBool("isWalkBack", false);
            
        }
        
        
    }
    private void StopMoving()
    {
        pathfinding.StopMoving();  // Stop movement
        anim.SetBool("isWalk", false);  // Stop walk animation
        anim.SetBool("isWalkBack", false);  // Stop walk back animation
        //rb.velocity = Vector2.zero;  // Stop Rigidbody movement
    }

    
    void RetreatFromPlayer()
    {
        Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + directionAwayFromPlayer, retreatSpeed * Time.deltaTime);

        CalculateFacingDirection();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }

    private void CalculateFacingDirection()
    {
        if (player != null)
        {
            // If player is on the left of the enemy, flip the sprite (scale X to -1), otherwise keep it (scale X to 1)
            if (player.position.x < transform.position.x)
            {
                _spriteRenderer.flipX = true;  // Flip sprite
            }
            else
            {
                _spriteRenderer.flipX = false;  // Don't flip sprite
            }
        }

    }
}
