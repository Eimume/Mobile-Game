using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Movement")]
    public Transform player;
    public float detectionRadius = 10f; // Distance for enemy to start following
    public float stoppingDistance = 5f; // Distance from player where enemy will stop moving

    [Header("Status")]
    public int attack = 10;
    public float cooldown = 2f;
    public float speed;


    private AstarPathfinding pathfinding;
    private bool isFollowingPlayer = false; // Is the enemy following the player?

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Ene_closeCom enattack;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer _spriteRenderer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pathfinding = GetComponent<AstarPathfinding>();
        player = GameObject.FindGameObjectWithTag("Player").transform;       
    }

    // Update is called once per frame
    void Update()
    {
        if (enattack != null)
        {
            enattack.damage = attack;
            enattack.TimeHitCoolDown = cooldown;
        }

        if (pathfinding != null)
        {
            pathfinding.speed = speed;
        }

        float distancePlayer = Vector2.Distance(player.position, transform.position);
        if(distancePlayer < detectionRadius && distancePlayer > stoppingDistance)
        {
            anim.SetBool("isWalk", true);
            pathfinding.FollowPlayerPath();
            CalculateFacingDirection();
        }
        else
        {
            anim.SetBool("isWalk", false);
            pathfinding.StopMoving();
        }

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
