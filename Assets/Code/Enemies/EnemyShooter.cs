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

    

    [Header("Dependencies")]
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer _spriteRenderer;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {
       shootingTimer += Time.deltaTime;

        if (canShoot)
        {       
        // If the timer reaches zero, shoot a bullet
            if (shootingTimer >= shootingInterval)
            {
                shootingTimer = 0f;
                Debug.Log("Shoot");
                anim.SetTrigger("Shoot");
                Shoot();                
                //shootingTimer = shootingInterval; // Reset the timer
                
            }
        }
        
    }

    public void Shoot()
    {            
            if (bulletPrefab != null && firePoint != null && player != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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
        canShoot = value;  // Set whether shooting is allowed
        /*if (!value)
        {
            anim.ResetTrigger("Shoot");  // Reset the shoot trigger if shooting is disabled
        }*/
    }
}
