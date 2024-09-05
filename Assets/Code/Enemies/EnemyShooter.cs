using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint; // The position where bullets will be fired from
    public float shootingInterval = 2f; // Time between shots
    public float bulletSpeed = 5f; // Speed of the bullet

    private bool canShoot = false;
    private GameObject player; // Reference to the player object
    private float shootingTimer = 0f; // Timer for tracking shooting intervals

    void Start()
    {
        // Find the player by tag (assuming the player is tagged as "Player")
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Count down the timer for shooting
        shootingTimer -= Time.deltaTime;

        // If the timer reaches zero, shoot a bullet
        if (shootingTimer <= 0f)
        {
            Shoot();
            shootingTimer = shootingInterval; // Reset the timer
        }
    }

    void Shoot()
    {
             GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            if (player != null)
            {
            // Calculate the direction from the enemy to the player
            Vector2 direction = (player.transform.position - firePoint.position).normalized;

            // Rotate the bullet to face the player
            bullet.transform.right = direction;

            // Set the bullet's velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bulletSpeed;
            }
        }
        
    }
    public void EnableShooting(bool value)
    {
        canShoot = value;
    }
}
