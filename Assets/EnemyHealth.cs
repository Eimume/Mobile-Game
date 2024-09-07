using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHp = 100;
    private int currentHp;
    void Start()
    {
        currentHp = MaxHp;
    }

    public void TakeDamage(int damageAmount)
    {  
        currentHp -= damageAmount;

        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        Debug.Log("Enemy health: " + currentHp);

    }

    private void Die()
    {
        Debug.Log("Enemy has died!");
        // Add additional code to handle player's death (e.g., respawn, game over screen)
    }

    public int GetCurrentHealth()
    {
        return currentHp;
    }
}
