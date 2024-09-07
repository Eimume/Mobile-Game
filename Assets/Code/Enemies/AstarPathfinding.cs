using UnityEngine;
using Pathfinding;

public class AstarPathfinding : MonoBehaviour
{
    public Transform target;

    public float speed = 400f;
    public float nextWaypointDistance = 3f;
    //public float lineMove;

    //public Transform enemyGPX;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool isMoving = true;

    Seeker seeker;
    Rigidbody2D rb;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }
    void Update()
    {
        if (isMoving)
        {
        FollowPlayerPath();
        }
                   
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
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

    public void FollowPlayerPath()
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

        /*if (force.x >= 0.01f)
        {
            enemyGPX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGPX.localScale = new Vector3(1f, 1f, 1f);
        }*/
    }

     public void StopMoving()
    {
        isMoving = false; // Set the flag to stop movement
        rb.velocity = Vector2.zero; // Stop any ongoing movement by resetting velocity
        path = null; // Clear the current path to avoid further movement
    }

    // Method to resume movement if needed
    public void ResumeMoving()
    {
        isMoving = true;
    }
}
