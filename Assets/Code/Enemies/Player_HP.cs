using UnityEngine;

public class Player_HP : MonoBehaviour
{
    public int maxHp = 100;

    private int currentHp;
    void Start()
    {
        currentHp = maxHp;   
    }

    public void TakeDamage(int damageAmount)
    {  
        currentHp -= damageAmount;

        if (currentHp <= 0)
        {
            currentHp = 0;
            //Die();
        }

        Debug.Log("Player health: " + currentHp);

    }

     /*public void Heal(int healAmount)
    {
        currentHp += healAmount;

        // Ensure health doesn't exceed maxHealth
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        
        Debug.Log("Player Health: " + currentHp);
    }*/
    private void Die()
    {
        Debug.Log("Player has died!");
        // Add additional code to handle player's death (e.g., respawn, game over screen)
    }

    public int GetCurrentHealth()
    {
        return currentHp;
    }
}
