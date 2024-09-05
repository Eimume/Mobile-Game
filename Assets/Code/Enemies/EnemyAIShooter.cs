using Pathfinding;
using UnityEngine;

public class EnemyAIShooter : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    public float detectionRadius = 10f; // Distance for enemy to start following
    public float stoppingDistance = 5f; // Distance from player where enemy will stop moving
    public float retreatSpeed = 2f; // Speed at which the enemy retreats when player is too close

    private AstarPathfinding pathfinding; // Reference to your A* pathfinding script
    private bool isFollowingPlayer = false; // Is the enemy following the player?
    private EnemyShooter enemyShooter; // Reference to the EnemyShooter component

    void Start()
    {
        pathfinding = GetComponent<AstarPathfinding>();
        enemyShooter = GetComponent<EnemyShooter>(); // Get the shooter component
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            enemyShooter.EnableShooting(true); // Enable shooting if the player is inside detection radius
        }
        else
        {
            enemyShooter.EnableShooting(false); // Disable shooting if the player is outside detection radius
        }

        // If player is within detection range, start following
         if (distanceToPlayer < detectionRadius && distanceToPlayer > stoppingDistance)
        {
            isFollowingPlayer = true;
            pathfinding.FollowPlayerPath();
            
        }
        else if (distanceToPlayer <= stoppingDistance) 
        {
            isFollowingPlayer = false;
            RetreatFromPlayer(); // Start retreating when too close
            
        }
        else
        {
            isFollowingPlayer = false;
            pathfinding.StopMoving(); // Stop moving when out of range
            
        }
        
    }
    
    void RetreatFromPlayer()
    {
        // Calculate the direction away from the player
        Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;

        // Move the enemy in the opposite direction from the player at a slower speed
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + directionAwayFromPlayer, retreatSpeed * Time.deltaTime);
    }

    // Optional: Draw detection range and stopping distance in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}
