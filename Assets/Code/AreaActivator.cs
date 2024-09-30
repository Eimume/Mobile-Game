using UnityEngine;

public class AreaActivator : MonoBehaviour
{
    //public string playerTag = "Player";
    public GameObject door;
    private bool playerInside = false;
    public GameObject enemy;

    void Start ()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.SetActive(false);
        door.SetActive(false);
    }
    // This will be called when the player enters the trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            // Activate all enemies in the area
            //ActivateEnemies(true);

            if (door != null)
            {
                door.SetActive(true); // Make the door appear
            }

            if (enemy != null)
            {
                enemy.SetActive(true); // Make the door appear
            }
        }
    }

    // When the player exits the trigger area (if needed)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }

    }

    // Helper method to activate/deactivate enemies
    /*private void ActivateEnemies(bool activate)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            // Check if the enemy is within the activation zone
            if (IsEnemyInArea(enemy))
            {
                enemy.SetActive(activate); // Activate or deactivate the enemy
            }
        }
    }*/

    // Helper method to check if the enemy is within the collider area
    private bool IsEnemyInArea(GameObject enemy)
    {
        Collider2D areaCollider = GetComponent<Collider2D>();
        return areaCollider.bounds.Contains(enemy.transform.position);
    }

    // Update method to check if all enemies are defeated
    private void Update()
    {
        // Check if all enemies in the area are defeated
        if (playerInside && AreAllEnemiesDefeated())
        {
            // Deactivate the door once all enemies are defeated
            if (door != null && door.activeSelf)
            {
                door.SetActive(false); // Make the door disappear
            }
        }
    }

    // Helper method to check if all enemies in the area are defeated
    private bool AreAllEnemiesDefeated()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            // Check if the enemy is in the area and still active
            if (IsEnemyInArea(enemy) && enemy.activeSelf)
            {
                return false; // There are still enemies alive
            }
        }
        return true; // All enemies in the area are defeated
    }
}
