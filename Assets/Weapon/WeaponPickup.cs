using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponToEquip;  // อาวุธที่ผู้เล่นจะได้รับเมื่อเก็บ

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่า Collider ที่เข้ามาคือผู้เล่นหรือไม่
        if (other.CompareTag("Player"))
        {
            Player_Weapon player = other.GetComponent<Player_Weapon>();
            if (player != null)
            {
                // ให้ผู้เล่นเปลี่ยนอาวุธเป็นอาวุธนี้
                player.EquipWeapon(weaponToEquip);

                // ลบ GameObject ของอาวุธออกจากฉากเมื่อเก็บ
                Destroy(gameObject);
            }
        }
    }
}
