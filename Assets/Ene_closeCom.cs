using UnityEngine;
using System.Collections;

public class Ene_closeCom : MonoBehaviour
{
    public GameObject player;
    public int damage = 10;
    public float TimeHitCoolDown;
    private bool canHit = true;
    private float cooldownTimer = 0f;

void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        if (!canHit)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canHit = true; // Reset the ability to attack
            }
        }

        if (player != null && canHit)
        {
             Player_HP playerHealth = player.GetComponent<Player_HP>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Deal damage to the player
                canHit = false; // Start cooldown
                cooldownTimer = TimeHitCoolDown; // Reset cooldown timer
            }
        }

    }
     private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Player"))
        {
            player = other.gameObject; // Store the player reference
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Damage is handled in Update() with a cooldown
        }
    }
     private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null; // Clear the player reference
        }
    }
    
}
