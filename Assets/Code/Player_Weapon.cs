using UnityEngine;

public class Player_Weapon : MonoBehaviour
{
    public Transform weaponHolder;    // ตำแหน่งที่จะติดตั้งอาวุธ
    public Weapon currentWeapon;      // อาวุธปัจจุบันที่ถืออยู่
    //private GameObject droppedWeapon;       // Store the dropped weapon object

    private GameObject equippedWeaponInstance;
     private WeaponPickup nearbyWeaponPickup; // Store the nearby weapon pickup

    public Transform gunTransform;    // อ้างถึง Transform ของปืน
    private Transform nearestEnemy;
    //public GameObject enemy;

    public Weapon hand;

    private GameObject lastDroppedWeaponInstance;

    private void Start()
    {
        EquipWeapon(hand);
        //enemy = GameObject.FindGameObjectWithTag("Enemy");

    }
    private void Update()
    {

        nearestEnemy = FindNearestEnemy();
        
        // กดปุ่มซ้ายเมาส์เพื่อโจมตี
        if (nearbyWeaponPickup != null && nearbyWeaponPickup.IsPlayerNear() && Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
            //PickupWeapon(nearbyWeaponPickup.weaponToEquip, nearbyWeaponPickup.gameObject);
        }

         if (Input.GetKey(KeyCode.E))
        {
            if (currentWeapon is Gun gun)
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

            // Drop the current weapon before picking up the new one
            DropCurrentWeapon();

            // Pick up the new weapon
            PickupWeapon(newWeapon, weaponObject);
        }
    }

    void PickupWeapon(Weapon newWeapon, GameObject weaponObject)
    {

        EquipWeapon(newWeapon);

        weaponObject.SetActive(false);  // Disable the weapon from the scene, as it is now equipped
        Debug.Log("Picked up: " + newWeapon.name);
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
            bool isEnemyInRange = currentWeapon.AimAtEnemy(gunTransform, nearestEnemy);
            if (isEnemyInRange)
            {
                // If enemy is within range, execute the attack
                if (currentWeapon is Sword sword)
                {
                    sword.Attack();
                // Sword attack logic
                    Debug.Log("Attacking with sword!");
                    Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, 1.5f);  // Sword swing range
                foreach (Collider2D enemy in enemiesHit)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        sword.DealDamage(enemy);  // Deal damage to enemies in range
                    }
                }
                }
                else if (currentWeapon is Gun gun)
                {
                    gun.AimAtEnemy(gunTransform, nearestEnemy);
                    gun.ShootAtEnemy(gunTransform, nearestEnemy);
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
