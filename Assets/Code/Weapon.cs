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
    public int ammo;  // จำนวนกระสุนของปืน

    // ฟังก์ชันโจมตีเฉพาะสำหรับปืน
    public override void Attack()
    {
        if (ammo > 0)
        {
            Debug.Log("Shooting with " + weaponName + ". Ammo left: " + (ammo - 1));
            ammo--;
        }
        else
        {
            Debug.Log("Out of ammo!");
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
