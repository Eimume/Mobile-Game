using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int MaxHp = 100;
    public float time = 5f;
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
        //Destroy(gameObject);
        StartCoroutine(DieAndDestroy());
        // Add additional code to handle player's death (e.g., respawn, game over screen)
    }

    private IEnumerator DieAndDestroy()
    {
        // Optionally, you can disable the enemy's collider or any other components here
        // GetComponent<Collider2D>().enabled = false;

        // Wait for 5 seconds
        yield return new WaitForSeconds(time);

        // Destroy the enemy object after 5 seconds
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHp;
    }
}
