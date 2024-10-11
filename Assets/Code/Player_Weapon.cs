using Unity.VisualScripting;
using UnityEngine;

public class Player_Weapon : MonoBehaviour
{
    public Transform weaponHolder;    // ตำแหน่งที่จะติดตั้งอาวุธ
    public Weapon currentWeapon;      // อาวุธปัจจุบันที่ถืออยู่

    private GameObject equippedWeaponInstance;
     private WeaponPickup nearbyWeaponPickup; // Store the nearby weapon pickup


    public Transform weaponTransform; // Transform of the weapon (used for aiming)
    private Transform nearestEnemy;

    public Weapon hand;
    public Player_HP playerHP;
    private GameObject lastDroppedWeaponInstance;

    public Animator anime;

    private bool isAttacking = false; // To track whether the weapon is in use

    

    private void Start()
    {
        
        EquipWeapon(hand);

        if (playerHP == null)
        {
            playerHP = GetComponent<Player_HP>();
        }
        
    }
    private void Update()
    {

        nearestEnemy = FindNearestEnemy();
        
        // กดปุ่มซ้ายเมาส์เพื่อโจมตี
        if (nearbyWeaponPickup != null && nearbyWeaponPickup.IsPlayerNear() && Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();

        }

        if (currentWeapon is Sword sword)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Attack();
                anime.SetTrigger ("attack");
                //anime.SetBool("isAttack", true);
            }
            else
            {
                // Rotate sword to 90 degrees when not attacking
                weaponTransform.localRotation = Quaternion.Euler(0, 0, 90);
                //anime.SetBool("isAttack", false);
            }
        }
        else
        {
            // Handle other weapons (e.g., gun)
            if (Input.GetKey(KeyCode.E))
            {
                Attack();
            }
        }
    }
    
    public void SwitchWeapon()
    {
        if (currentWeapon != null && nearbyWeaponPickup != null)
        {
            Weapon newWeapon = nearbyWeaponPickup.weaponToEquip; // Get the weapon on the ground
            GameObject weaponObject = nearbyWeaponPickup.gameObject;

            ApplyWeaponDamageToPlayer(newWeapon);

            // Drop the current weapon before picking up the new one
            DropCurrentWeapon();
            PickupWeapon(newWeapon, weaponObject);
        }
    }

    void PickupWeapon(Weapon newWeapon, GameObject weaponObject)
    {

        EquipWeapon(newWeapon);
        weaponObject.SetActive(false);  // Disable the weapon from the scene, as it is now equipped
        Debug.Log("Picked up: " + newWeapon.name);
    }

    void ApplyWeaponDamageToPlayer(Weapon weapon)
    {
        if (playerHP != null)
        {
            int damageToPlayer = weapon.playerDamageOnEquip;  // จำนวนความเสียหายจากอาวุธ
            playerHP.TakeDamage(damageToPlayer);  // เรียกฟังก์ชัน TakeDamage จาก Player_HP
            Debug.Log("Player took " + damageToPlayer + " damage from equipping " + weapon.weaponName);
        }
    }
    void DropCurrentWeapon()
    {
        // Create a dropped weapon object in the current position
         if (currentWeapon != null && equippedWeaponInstance != null)
        {
            // Disable the equipped weapon instance and place it on the ground at the player's position
            equippedWeaponInstance.transform.position = transform.position;
            equippedWeaponInstance.transform.SetParent(null);  // Detach it from the player
            equippedWeaponInstance.SetActive(true);  // Ensure it's visible in the scene

            // Add the WeaponPickup component to allow it to be picked up again if not already there
            if (!equippedWeaponInstance.GetComponent<WeaponPickup>())
            {
                WeaponPickup droppedWeaponPickup = equippedWeaponInstance.AddComponent<WeaponPickup>();
                droppedWeaponPickup.weaponToEquip = currentWeapon;
            }

            lastDroppedWeaponInstance = equippedWeaponInstance;
            Debug.Log("Dropped: " + currentWeapon.name);
        }
    }

    // ฟังก์ชันในการติดตั้งอาวุธใหม่
    public void EquipWeapon(Weapon newWeapon)
    {

        // If the weapon is already in the scene (was dropped), reuse that instance
        if (lastDroppedWeaponInstance != null && newWeapon == lastDroppedWeaponInstance.GetComponent<WeaponPickup>().weaponToEquip)
        {
            equippedWeaponInstance = lastDroppedWeaponInstance;
            equippedWeaponInstance.SetActive(true); // Re-enable the weapon if it was disabled
            equippedWeaponInstance.transform.SetParent(weaponHolder);  // Attach it back to the player
            lastDroppedWeaponInstance = null; // Clear this since we are re-equipping
        }
        else
        {
            // Otherwise, instantiate a new weapon if needed
            if (newWeapon.weaponPrefab != null)
            {
                equippedWeaponInstance = Instantiate(newWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation);
                equippedWeaponInstance.transform.SetParent(weaponHolder);
            }
        }
        

        // เปลี่ยนอาวุธใหม่
        currentWeapon = newWeapon;

        if (currentWeapon is Gun gun)
        {
            gun.Initialize();  // Initialize the gun's ammo and reload state
        }

        Debug.Log("Equipped: " + newWeapon.weaponName);
    }

    public void Attack()
    {
        if (currentWeapon != null)
        {
            bool isEnemyInRange = currentWeapon.AimAtEnemy(weaponTransform, nearestEnemy);
            if (isEnemyInRange)
            {
                // If enemy is within range, execute the attack
                if (currentWeapon is Sword sword)
                {
                    anime = equippedWeaponInstance.GetComponent<Animator>();
                    //anime = currentWeapon.anim;
                    //anime.SetTrigger ("attack");
                    

                    isAttacking = true;

                    sword.Attack();
                    Debug.Log("Attacking with sword!");

                    Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(weaponTransform.position, sword.attackRadius);  // Sword swing range
                    sword.DealDamageToEnemies(enemiesHit, weaponTransform);

                }
                
                if (currentWeapon is Gun gun)
                {
                    gun.AimAtEnemy(weaponTransform, nearestEnemy);
                    gun.ShootAtEnemy(weaponTransform, nearestEnemy);
                }
            }
            else
            {
                Debug.Log("Enemy out of range!");
            }    
        }
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }
        return nearest;
    
    }
    private void OnDrawGizmos()
    {
        if (currentWeapon != null)
        {
            Gizmos.color = Color.green;
            // Draw a wire sphere representing the weapon's aim range
            Gizmos.DrawWireSphere(transform.position, currentWeapon.aimRange);
        }
        if (currentWeapon is Sword sword)
        {
           // Draw a wire sphere representing the sword's attack radius
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, sword.attackRadius);

                // Draw sword attack angle
            Vector3 startDirection = Quaternion.Euler(0, 0, -sword.attackAngle / 2) * weaponTransform.right;
            Vector3 endDirection = Quaternion.Euler(0, 0, sword.attackAngle / 2) * weaponTransform.right;
            Gizmos.DrawLine(weaponTransform.position, weaponTransform.position + startDirection * sword.attackRadius);
            Gizmos.DrawLine(weaponTransform.position, weaponTransform.position + endDirection * sword.attackRadius);

            // Draw arc to visualize the attack area
            int segments = 20;
            Vector3 previousPoint = weaponTransform.position + startDirection * sword.attackRadius;
            for (int i = 1; i <= segments; i++)
            {
                float angleStep = sword.attackAngle / segments;
                float currentAngle = -sword.attackAngle / 2 + angleStep * i;
                Vector3 arcPoint = Quaternion.Euler(0, 0, currentAngle) * weaponTransform.right * sword.attackRadius;
                Gizmos.DrawLine(previousPoint, weaponTransform.position + arcPoint);
                previousPoint = weaponTransform.position + arcPoint;
            }
        }
        else
        {
            // Draw default aim range for other weapons (e.g., guns)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, currentWeapon.aimRange);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WeaponPickup weaponPickup = collision.GetComponent<WeaponPickup>();
        if (weaponPickup != null)
        {
            nearbyWeaponPickup = weaponPickup;
        }
    }

    // Clear the nearby weapon when the player leaves the range
    private void OnTriggerExit2D(Collider2D collision)
    {
        WeaponPickup weaponPickup = collision.GetComponent<WeaponPickup>();
        if (weaponPickup != null)
        {
            nearbyWeaponPickup = null;
        }

    }

}
