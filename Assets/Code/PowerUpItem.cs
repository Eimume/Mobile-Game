using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public PlayerState powerUpState;  // The new PlayerState when this power-up is collected

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the power-up
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            // Trigger the power-up
            player.PowerUp(powerUpState);

            // Destroy the power-up object after collection
            Destroy(gameObject);
        }
    }
}
