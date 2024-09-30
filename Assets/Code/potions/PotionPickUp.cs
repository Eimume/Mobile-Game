using UnityEngine;

public class PotionPickUp : MonoBehaviour
{
    public Potion potion;               // Assign the Potion ScriptableObject
    private bool isPlayerNear = false;  // Track if the player is in range to pick up

    private Player_HP playerHP;         // Reference to the player's health component

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect if the player is near
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player is near the potion.");

            // Get the player's health component
            playerHP = other.GetComponent<Player_HP>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Detect if the player leaves the potion's range
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the potion's range.");
        }
    }

    private void Update()
    {
        // Check if the player is near and presses the 'Q' key to use the potion
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Q))
        {
            if (potion != null && playerHP != null)
            {
                // Use the potion to heal the player
                potion.Use(playerHP);

                // Destroy the potion object after it has been used
                Destroy(gameObject);
                Debug.Log("Potion used and destroyed.");
            }
        }
    }

    // Optional: Check if the player is near for external use
    public bool IsPlayerNear()
    {
        return isPlayerNear;
    }
}
