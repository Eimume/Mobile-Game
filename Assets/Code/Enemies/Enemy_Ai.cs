using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;
//using static UnityEngine.RuleTile.TilingRuleOutput;


public class Enemy_Ai : MonoBehaviour
{
    public Transform target;

    public float speed = 10f;
    public float nextWaypointDistance = 3f;
    public float lineMove;
    public float viewRadius;
    public float viewAngle;
    public LayerMask targetMask;
    public LayerMask mask;

    //private Vector2 velocity;
    private bool noWall;
    private bool playerInSight;

    public Transform enemyGPX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("player").transform;
        viewRadius = lineMove;
        noWall = true;
        playerInSight = false;
        if (seeker == null || rb == null || target == null)
        {
            Debug.LogError("Missing essential components! Ensure Seeker, Rigidbody2D, and target are set.");
        }
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }
    void UpdatePath()
    {
        if (playerInSight && seeker.IsDone())
            Debug.Log("Updating path to player...");
            seeker.StartPath(rb.position, target.position, OnPathComplete);

    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distancePlayer = Vector2.Distance(target.position, transform.position);
        if(distancePlayer < lineMove)
        {
            CheckLineOfSight();
            if (playerInSight)
            {
                FollowPlayerPath();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineMove);

        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);*/

        Vector3 fovLine1 = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right * viewRadius;
        Vector3 fovLine2 = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right * viewRadius;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
    void FollowPlayerPath()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            enemyGPX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGPX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void CheckLineOfSight()
    {
        Vector3 directionToPlayer = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.right, directionToPlayer) < viewAngle / 2)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            if (Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, mask))
            {
                noWall = true;
                playerInSight = true;
                Debug.Log("Player detected!");
                // Implement your logic here (e.g., chase player, attack, etc.)
            } else
            {
                noWall = false;
                playerInSight = false;
                Debug.Log("Player blocked by an obstacle.");
            }
        }
        else
        {
            playerInSight = false ;
            // Player is outside the view angle
            Debug.Log("Player outside field of view.");
        }
    }

}

/*
Ray2D ray = new Ray2D(transform.position, target.transform.position);
Debug.DrawRay(ray.origin, ray.direction * lineMove, Color.blue);
RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position, lineMove, mask);

if (hit != null && hit == target)
{
    Debug.DrawRay(ray.origin, ray.direction * lineMove, Color.red);
    noWall = false;
}
else
{
}*/