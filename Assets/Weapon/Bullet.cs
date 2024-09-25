using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f; // How long the bullet exists before being destroyed

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the bullet after a set time
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with an enemy
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);  // Destroy bullet when it hits the enemy
            // You could also apply damage to the enemy here
        }
    }
}
