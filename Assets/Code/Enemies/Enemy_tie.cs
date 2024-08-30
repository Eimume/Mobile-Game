using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Enemy_tie : MonoBehaviour
{
    public NavMeshAgent agent;//Add NavMeshModifier

    public GameObject player1;
    public GameObject player2;

    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject bullet;

    // Patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // State
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Patrol parameters
    public float moveSpeed = 3f;
    public float patrolRadius = 5f;
    public float waitTime = 1f;

    private Vector2 startPos;
    private Vector2 targetPos;
    private float waitTimer;
    private bool isWaiting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // Ensure the agent updates correctly for 2D
        agent.updateRotation = false;  // Disable automatic rotation updates
        agent.updateUpAxis = false;    // Disable automatic up axis updates
        // Initialize patrol parameters
        startPos = transform.position;
        SetRandomTargetPosition();
        waitTimer = waitTime;
        isWaiting = false;
    }

    void Update()
    {
        // Constrain the agent to the XY plane
        Vector3 position = agent.transform.position;
        position.z = 0;
        agent.transform.position = position;

        Transform nearestPlayer = GetNearestPlayer();

        if (nearestPlayer != null)
        {
            playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
                Patrolling();
            else if (playerInSightRange && !playerInAttackRange)
                ChasePlayer(nearestPlayer);/*
            else if (playerInAttackRange && playerInSightRange)
                Attacking(nearestPlayer);*/
        }
    }

    void Patrolling()
    {
        StopChasing();
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                SetRandomTargetPosition();
                isWaiting = false;
                waitTimer = waitTime;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == targetPos)
            {
                isWaiting = true;
            }
        }
    }

    void SetRandomTargetPosition()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomY = Random.Range(-patrolRadius, patrolRadius);
        targetPos = new Vector2(startPos.x + randomX, startPos.y + randomY);
    }

    void ChasePlayer(Transform targetPlayer)
    {
        agent.SetDestination(targetPlayer.position);
    }

    /*void Attacking(Transform targetPlayer)
    {
        agent.SetDestination(transform.position);

        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        transform.up = direction;

        if (!alreadyAttacked)
        {
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            Rigidbody2D rb = bulletInstance.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * 12f, ForceMode2D.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }*/
    public void StopChasing()
    {
        agent.isStopped = true; // Stops the agent from moving
        agent.ResetPath(); // Clears the current path
    }

   /* private void ResetAttack()
    {
        alreadyAttacked = false;
    }*/
    

    private Transform GetNearestPlayer()
    {
        float distanceToPlayer1 = Vector3.Distance(transform.position, player1.transform.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, player2.transform.position);

        return distanceToPlayer1 < distanceToPlayer2 ? player1.transform : player2.transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
