using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weaponToEquip;  // อาวุธที่ผู้เล่นจะได้รับเมื่อเก็บ
    private bool isPlayerNear = false;  // Track if the player is in range to pick up

    private void OnTriggerEnter2D(Collider2D other)
    {   
         if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log("Player is near the weapon.");
        }
    }

    // Detect if the player exits the pickup range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            Debug.Log("Player left the weapon's range.");
        }
    }

    public bool IsPlayerNear()
    {
        return isPlayerNear;
    }
    
}
