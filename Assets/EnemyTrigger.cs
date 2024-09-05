using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    
    private Ene_closeCom parentEnemy;
/*
    void Start()
    {
        // Find the parent Enemy component
        parentEnemy = GetComponentInParent<Ene_closeCom>();
    }

    // When the player enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentEnemy.PlayerEnteredTrigger(); // Notify parent enemy
        }
    }

    // When the player exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            parentEnemy.PlayerExitedTrigger(); // Notify parent enemy
        }
    }*/
}
