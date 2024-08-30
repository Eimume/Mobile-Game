using Unity.VisualScripting;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public GameObject[] weapons = new GameObject[2];
    public GameObject First_item;
    private int currentWeaponIndex = 0;

    public Vector3 dropOffset = new Vector3(0, 0, 1);

    void Start()
    {
        GameObject Hand_player = GameObject.Find("Handle_Item");
        GameObject item = Instantiate(First_item,Hand_player.transform);
        item.transform.SetParent(Hand_player.transform);
        item.transform.localPosition = Vector3.zero;
        weapons[currentWeaponIndex] = item;
        foreach (GameObject weapon in weapons)
        {
            if (weapon != null)
            {
                weapon.SetActive(false);
            }
        }
        if (weapons[0] != null)
        {
            weapons[0].SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        int nextWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;

        if (weapons[nextWeaponIndex] != null)
        {
            weapons[currentWeaponIndex]?.SetActive(false);

            currentWeaponIndex = nextWeaponIndex;
            weapons[currentWeaponIndex]?.SetActive(true);
        }
    }

    void DropWeapon()
    {
        DropItem drop = weapons[currentWeaponIndex].gameObject.GetComponent<DropItem>();
        if (weapons[currentWeaponIndex] != null)
        {
            GameObject currentWeapon = weapons[currentWeaponIndex];
            currentWeapon.transform.SetParent(null);
            currentWeapon.transform.position = transform.position + dropOffset;
            drop.enabled = true;
            weapons[currentWeaponIndex] = null;
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        weapons[currentWeaponIndex]?.SetActive(false);
        bool weaponAdded = false;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
            {
                weapons[i] = newWeapon;
                currentWeaponIndex = i;              
                weapons[currentWeaponIndex].SetActive(true);
                weaponAdded = true;
                break;
            }
        }
        if (!weaponAdded)
        {
            weapons[currentWeaponIndex].SetActive(true);
            DropWeapon();
            weapons[currentWeaponIndex] = newWeapon;
            weapons[currentWeaponIndex].SetActive(true);
        }
    }
}
