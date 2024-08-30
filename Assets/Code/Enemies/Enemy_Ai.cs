using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;


public class Enemy_Ai : MonoBehaviour
{
    public Transform target;

    public float speed = 10f;
    public float nextWaypointDistance = 3f;
    public float lineMove;

    //public LayerMask targetMask;
    //public LayerMask mask;

    //private Vector2 velocity;

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
        target = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("UpdatePath", 0f, .5f);
        
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

    // Update is called once per frame
    void Update()
    {
        float distancePlayer = Vector2.Distance(target.position, transform.position);
        if(distancePlayer < lineMove)
        {
                FollowPlayerPath();
            
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineMove);

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