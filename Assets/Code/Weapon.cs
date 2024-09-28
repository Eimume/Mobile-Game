using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    //public int damage;
    public float attackSpeed;
    public Sprite weaponIcon;
    public float aimRange = 5f;
    public GameObject weaponPrefab;


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

#region Gun
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
    // ฟังก์ชันโจมตีเฉพาะสำหรับปืน
    
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
#endregion

#region Sword
[CreateAssetMenu(fileName = "NewSword", menuName = "Weapons/Sword")]
public class Sword : Weapon
{
    public int damage = 25;
    public float attackRadius = 1.5f; // The range for dealing damage (close combat)
    public float attackAngle = 180f; // Attack angle range (180 degrees)
    public float attackCooldown = 1.0f;     // Time delay between sword attacks

    private float attackTimer;
    private bool canAttack = true;
    //private bool canDealDamage = true; 


    public override bool AimAtEnemy(Transform weaponTransform, Transform nearestEnemy)
    {
        if (nearestEnemy == null) return false;

        // Aim at the enemy if within the aim range (this uses aimRange, not attackRadius)
        return base.AimAtEnemy(weaponTransform, nearestEnemy);
    }

    public override void Attack()
    {
        if (canAttack)
        {
            Debug.Log("Sword swinging: " + weaponName);
            canAttack = false;  // Set the sword to not able to attack again until cooldown
            attackTimer = 0f;
            //canDealDamage = false;

            // Logic for dealing damage to enemies (handled in Player_Weapon)
        }
        else
        {
            Debug.Log("Sword attack is on cooldown.");
        }
    }

    public void UpdateCooldown(float deltaTime)
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;

            // When the timer exceeds the cooldown period, reset the ability to attack
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                //canDealDamage = true;
                //attackTimer = 0f;
            }
        }
    }
    public void DealDamage(Collider2D enemyCollider)
    {
        
            EnemyHealth enemy = enemyCollider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Dealt " + damage + " damage to " + enemy.name);
            }
        
    }   

}
#endregion

[CreateAssetMenu(fileName = "hand", menuName = "Weapons/Hand")]
public class Hand : Weapon
{
    // ฟังก์ชันโจมตีเฉพาะสำหรับดาบ
    public override void Attack()
    {
        Debug.Log("punch" + weaponName);
    }
}
