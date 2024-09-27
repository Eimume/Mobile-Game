using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    //public int damage;
    public float attackSpeed;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public float aimRange = 5f; // Default aiming range for weapons

    // ฟังก์ชันโจมตีสำหรับดาบหรือปืน
    public virtual void Attack()
    {
        Debug.Log("Attacking with " + weaponName);
    }
    public virtual bool AimAtEnemy(Transform weaponTransform, Transform nearestEnemy)
    {
        if (nearestEnemy == null) return false;

        // Calculate the distance to the nearest enemy
        float distanceToEnemy = Vector2.Distance(weaponTransform.position, nearestEnemy.position);
        
        // Check if the enemy is within the weapon's aim range
        if (distanceToEnemy <= aimRange)
        {
            // Calculate direction and rotate weapon towards the enemy
            Vector2 direction = nearestEnemy.position - weaponTransform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weaponTransform.rotation = Quaternion.Euler(0, 0, angle);

            // Optionally, draw a Debug Ray to visualize the aim direction
            Debug.DrawRay(weaponTransform.position, direction.normalized * distanceToEnemy, Color.green);

            return true;
        }

        return false; // Enemy is out of range
    }
}

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun")]
public class Gun : Weapon
{
    public GameObject bulletPrefab;
    //public Transform firePoint;
    public float bulletSpeed = 10f;    // Speed of the bullet
    public float shootInterval = 0.5f; // Delay between shots
    public int maxAmmo = 10;           // Maximum ammo capacity
    public float reloadTime = 2f;      // Time to reload in seconds
    public int bulletDamage = 20;      // Damage that the bullets from this gun deal

    private int currentAmmo;             // Current ammo available
    private float shootTimer;            // Timer for controlling shooting interval
    private bool isReloading;            // Is the gun currently reloading?
    private float reloadTimer;           // Timer to track reload progress
    
   /* public override void Attack()
    {
        Debug.Log("Shooting with the gun: " + weaponName);
    }*/
    public void Initialize()
    {
        currentAmmo = maxAmmo;  // Start with a full ammo clip
        isReloading = false;    // Gun is not reloading initially
        shootTimer = 0f;        // Reset shoot timer
    }

     public override bool AimAtEnemy(Transform weaponTransform, Transform nearestEnemy)
    {
        return base.AimAtEnemy(weaponTransform, nearestEnemy);
    }
    public void ShootAtEnemy(Transform gunTransform, Transform nearestEnemy)
    {
        // If currently reloading, can't shoot
        if (isReloading)
        {
            Debug.Log("Gun is reloading");
            Reload();
            return;
        }

        // If no ammo left, start reloading
        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo, reloading...");
            StartReloading();
            return;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval && nearestEnemy != null)
        {
            Debug.Log("Shooting at enemy: " + nearestEnemy.name);
            // Instantiate bullet at gun's position
            GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);
            Vector2 direction = (nearestEnemy.position - gunTransform.position).normalized;

            bullet.transform.right = direction;

            Bullet_Weapon bulletComponent = bullet.GetComponent<Bullet_Weapon>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDamage(bulletDamage); // Set the bullet damage from the gun
            }
            // Apply velocity to the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {              
                rb.velocity = direction * bulletSpeed;
            }

            // Consume one ammo and reset timer after shooting
            currentAmmo--;
            Debug.Log("Ammo: " + currentAmmo);
            shootTimer = 0f;
        }
    }

     private void StartReloading()
    {
        isReloading = true;
        reloadTimer = 0f; // Reset the reload timer
        Debug.Log("Reloading...");
    }

    // Function to handle reloading process
    private void Reload()
    {
        reloadTimer += Time.deltaTime;

        if (reloadTimer >= reloadTime)
        {
            // Finished reloading, reset ammo and flags
            currentAmmo = maxAmmo;
            isReloading = false;
            Debug.Log("Reload complete!");
        }
    }
}

[CreateAssetMenu(fileName = "NewSword", menuName = "Weapons/Sword")]
public class Sword : Weapon
{
    public int damage = 25;
    public float attackAngle = 180f; // Attack angle range (180 degrees)

    public override bool AimAtEnemy(Transform weaponTransform, Transform nearestEnemy)
    {
        if (nearestEnemy == null) return false;

        // Calculate the distance to the nearest enemy
        float distanceToEnemy = Vector2.Distance(weaponTransform.position, nearestEnemy.position);

        // Calculate the direction to the nearest enemy
        Vector2 directionToEnemy = (nearestEnemy.position - weaponTransform.position).normalized;
        float angleToEnemy = Vector2.Angle(weaponTransform.right, directionToEnemy);

        // Check if the enemy is within the sword's aim range and attack angle
        if (distanceToEnemy <= aimRange && angleToEnemy <= attackAngle / 2)
        {
            // Rotate the player to face the enemy
            weaponTransform.right = directionToEnemy;

            // Draw a debug line for visualization
            Debug.DrawLine(weaponTransform.position, nearestEnemy.position, Color.red);

            return true;
        }

        return false; // Enemy is out of range or attack angle
    }

    public override void Attack()
    {
        Debug.Log("Swinging the sword: " + weaponName);
    }    public void DealDamage(Collider2D enemyCollider)
    {
        EnemyHealth enemy = enemyCollider.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}

[CreateAssetMenu(fileName = "hand", menuName = "Weapons/Hand")]
public class Hand : Weapon
{
    // ฟังก์ชันโจมตีเฉพาะสำหรับดาบ
    /*public override void Attack()
    {
        Debug.Log("punch" + weaponName);
    }*/
}


