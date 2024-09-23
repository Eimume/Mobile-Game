using UnityEngine;
using System.Collections;

public class Enemy_Dash : MonoBehaviour
{
     public float dashSpeed = 10f;         // Speed of the dash
    public float dashDuration = 0.2f;     // How long the dash lasts
    public float dashCooldown = 2f;       // Time between dashes
    public Transform player;              // Reference to the player position

    private Rigidbody2D rb;
    private Vector2 dashDirection;
    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // If not dashing and can dash, try to dash toward player
        if (!isDashing && canDash)
        {
            DashTowardsPlayer();
        }
    }

    void DashTowardsPlayer()
    {
        if (player == null) return;

        // Calculate the direction towards the player
        dashDirection = (player.position - transform.position).normalized;

        // Start dashing
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        // Set the dashing state to true
        isDashing = true;
        canDash = false;

        // Apply a burst of force in the direction of the player
        rb.velocity = dashDirection * dashSpeed;

        // Wait for the dash duration
        yield return new WaitForSeconds(dashDuration);

        // Stop the dash
        rb.velocity = Vector2.zero;

        // Set dashing state to false
        isDashing = false;

        // Wait for the cooldown before the enemy can dash again
        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
    private void OnDrawGizmosSelected()
    {
        // Optional: Visualize the dash direction in the Scene view
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
