using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Hit : MonoBehaviour
{
    public float lineOfSite;
    public Transform Player;
    public float TimeHitCoolDown;
    private bool canHit = true;
    public float shootcooldown = 5f;
    public GameObject bulletPrefab;
    public float viewAngle = 90f; 
    public float viewDistance;
    private bool playerInSight = false;
    public LayerMask mask;

    private Vector2 velocity;

    private float timer = 0f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromPlayer = Vector2.Distance(Player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && canHit)
        {
            HitAction();
        }
    }
    void HitAction()
    {
        timer += Time.deltaTime;

        if (timer > shootcooldown)
        {
            Debug.Log("shoot");
        }

       

        StartCoroutine(CoolDown());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);

        if (Player != null)
        {
            Gizmos.color = playerInSight ? Color.green : Color.red;
            Gizmos.DrawLine(transform.position, Player.position);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 fovLine1 = Quaternion.Euler(0, 0, viewAngle / 2) * transform.right * viewDistance;
        Vector3 fovLine2 = Quaternion.Euler(0, 0, -viewAngle / 2) * transform.right * viewDistance;

        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);
    }
    IEnumerator CoolDown()
    {
        canHit = false;
        yield return new WaitForSeconds(TimeHitCoolDown);

        canHit = true;
    }

    void stopWall()
    {
        Ray2D ray = new Ray2D(transform.position, Player.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * lineOfSite, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position, lineOfSite, mask);

        if (hit.collider != null && hit.collider.gameObject == Player)
        {
            Debug.DrawRay(ray.origin, ray.direction * lineOfSite, Color.red);
        }
    }
    void shooter()
    {
        //Vector3 directionToPlayer = (Player.position - transform.position).normalized;

    }
    void CheckLineOfSight()
    {
        Vector3 directionToPlayer = (Player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);

        if (Vector3.Angle(transform.right, directionToPlayer) < viewAngle / 2)
        {
            if (distanceToPlayer < viewDistance)
            {
                // Perform the raycast
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, mask);

                if (hit.collider == null)
                {
                    // No obstruction, player is in sight
                    playerInSight = true;
                    Debug.Log("Player is in sight!");
                    // Implement logic when player is detected, e.g., chasing
                }
                else
                {
                    // Obstruction is in the way
                    playerInSight = false;
                    Debug.Log("Player is blocked by an obstacle.");
                }
            }
            else
            {
                playerInSight = false;
                Debug.Log("Player is out of range.");
            }
        }
        else
        {
            playerInSight = false;
            Debug.Log("Player is outside the field of view.");
        }
    }

}
