using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float attackSpeed;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;

    // ฟังก์ชันโจมตีสำหรับดาบหรือปืน
    public virtual void Attack()
    {
        Debug.Log("Attacking with " + weaponName);
    }
}

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun")]
public class Gun : Weapon
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;   // Speed of the bullet
    public float shootInterval = 0.5f; // Delay between shots
    public int maxAmmo = 10;             // Maximum ammo capacity
    public float reloadTime = 2f;        // Time to reload in seconds

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

     public void AimAtEnemy(Transform gunTransform, Transform nearestEnemy)
    {
        if (nearestEnemy == null) return;

        // Calculate the direction to the nearest enemy
        Vector2 direction = nearestEnemy.position - gunTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the gun to face the enemy
        gunTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Function to shoot bullets at the nearest enemy
    public void ShootAtEnemy(Transform gunTransform, Transform nearestEnemy)
    {
        // If currently reloading, can't shoot
        if (isReloading)
        {
            Reload();
            return;
        }

        // If no ammo left, start reloading
        if (currentAmmo <= 0)
        {
            StartReloading();
            return;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval && nearestEnemy != null)
        {
            // Instantiate bullet at gun's position
            GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, gunTransform.rotation);

            // Apply velocity to the bullet
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (nearestEnemy.position - gunTransform.position).normalized;
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
    // ฟังก์ชันโจมตีเฉพาะสำหรับดาบ
    public override void Attack()
    {
        Debug.Log("Swinging the sword: " + weaponName);
    }
}

[CreateAssetMenu(fileName = "hand", menuName = "Weapons/Hand")]
public class Hand : Weapon
{
    // ฟังก์ชันโจมตีเฉพาะสำหรับดาบ
    public override void Attack()
    {
        Debug.Log("punch" + weaponName);
    }
}
