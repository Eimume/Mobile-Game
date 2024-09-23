using UnityEngine;

public class Player_Weapon : MonoBehaviour
{
    public Weapon currentWeapon;      // อาวุธปัจจุบันที่ถืออยู่
    public Transform weaponHolder;    // ตำแหน่งที่จะติดตั้งอาวุธ
    public Transform gunTransform;    // อ้างถึง Transform ของปืน
    public Transform enemy;           // อ้างถึง Transform ของศัตรู

    private GameObject equippedWeaponInstance;


    public Weapon hand;
    /*public Weapon gun;   // อ้างถึงปืน (ScriptableObject)
    public Weapon sword; // อ้างถึงดาบ (ScriptableObject)
    */
    private void Start()
    {
        EquipWeapon(hand); // เริ่มต้นถือดาบ
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

    }

    // ฟังก์ชันในการติดตั้งอาวุธใหม่
    public void EquipWeapon(Weapon newWeapon)
    {
        // ลบอาวุธเก่าออก
        if (equippedWeaponInstance != null)
        {
            Destroy(equippedWeaponInstance);
        }

        // เปลี่ยนอาวุธใหม่
        currentWeapon = newWeapon;

        // สร้าง Prefab ของอาวุธใหม่บนตัวละคร
        if (newWeapon.weaponPrefab != null)
        {
            equippedWeaponInstance = Instantiate(newWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation);
            equippedWeaponInstance.transform.SetParent(weaponHolder);
        }

        Debug.Log("Equipped: " + newWeapon.weaponName);
    }

    // ฟังก์ชันโจมตี
    public void Attack()
    {
        if (currentWeapon != null)
        {
            currentWeapon.Attack();  // ใช้ฟังก์ชันโจมตีของอาวุธปัจจุบัน
        }
    }
     /*private void AimAtEnemy()
    {
         if (enemy != null && gunTransform != null)
        {
            // คำนวณทิศทางในแกน 2D จากปืนไปยังศัตรู
            Vector2 directionToEnemy = enemy.position - gunTransform.position;

            // คำนวณมุมการหมุนโดยใช้ Atan2 ซึ่งใช้คำนวณมุมระหว่างสองตำแหน่งในแกน X, Y
            float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

            // หมุนปืนให้ชี้ไปตามมุมที่คำนวณได้
            gunTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }*/

    // ฟังก์ชันสลับอาวุธระหว่างปืนและดาบ
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q)) // กด Q เพื่อสลับอาวุธ
        {
            if (currentWeapon == gun)
            {
                EquipWeapon(sword);  // เปลี่ยนเป็นดาบ
            }
            else
            {
                EquipWeapon(gun);    // เปลี่ยนเป็นปืน
            }
        }*/

        // กดปุ่มซ้ายเมาส์เพื่อโจมตี
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        /*if (currentWeapon == gun)
        {
            AimAtEnemy();
        }*/
    }
}
