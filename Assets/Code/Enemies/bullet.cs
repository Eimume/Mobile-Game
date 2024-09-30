using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    public int damage = 10; // Amount of damage the bullet deals
    public float lifetime = 5f; // How long the bullet lasts before being destroyed
    public LayerMask wallLayer; // LayerMask for the wall layer

    void Start()
    {
        // Destroy the bullet after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move the bullet forward based on its speed
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // Detect collision with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component and deal damage
            Player_HP playerHealth = other.GetComponent<Player_HP>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Apply damage to the player
            }

            // Destroy the bullet after it hits the player
            Destroy(gameObject);
        }

        else if (((1 << other.gameObject.layer) & wallLayer) != 0)
        {
            // Destroy the bullet if it hits a wall (on the Wall layer)
            Destroy(gameObject);
        }
    }
}
