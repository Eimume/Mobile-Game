using UnityEngine;

public class Player_HP : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;
    private playerMovement playerMovementScript;

    void Start()
    {
        currentHp = maxHp;
        playerMovementScript = GetComponent<playerMovement>(); 
    }

    public void SetHp(int newHp)
    {
        maxHp = newHp;
    }
    public void TakeDamage(int damageAmount)
    {  
        currentHp -= damageAmount;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        Debug.Log("Player health: " + currentHp);

    }

     public void Heal(int healAmount)
    {
        Debug.Log("Healing amount: " + healAmount);
        Debug.Log("Player current HP before healing: " + currentHp);

        currentHp += healAmount;

        // Ensure health doesn't exceed maxHealth
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        
        Debug.Log("Player Health: " + currentHp);
    }
    private void Die()
    {
        Debug.Log("Player has died!");
        playerMovementScript.enabled = false;
        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHp;
    }
}
