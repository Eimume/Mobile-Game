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
    

    [Header("Dependencies")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer _spriteRenderer;

    void Start()
    {
        pathfinding = GetComponent<AstarPathfinding>();
        enemyShooter = GetComponent<EnemyShooter>(); // Get the shooter component
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
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
    
    void RetreatFromPlayer()
    {
        Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + directionAwayFromPlayer, retreatSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
