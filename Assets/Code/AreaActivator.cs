using System.Collections.Generic;
using UnityEngine;

public class AreaActivator : MonoBehaviour
{
    //public string playerTag = "Player";
    public GameObject door;
    //public string enemyTag = "Enemy";
    public List <GameObject> enemiesInRoom = new List<GameObject>();
    private bool playerInside = false;
    //public GameObject enemy;

    void Start ()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //enemy.SetActive(false);
        SetEnemiesActive(false);
        door.SetActive(false);
    }

    private void SetEnemiesActive(bool isActive)
    {
        foreach (GameObject enemy in enemiesInRoom)
        {
            enemy.SetActive(isActive);
        }
    }
    // This will be called when the player enters the trigger area
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            SetEnemiesActive(true);

            if (door != null)
            {
                door.SetActive(true); // Make the door appear
            }

            
        }

        
        /*if (other.CompareTag(enemyTag))
            {
                GameObject[] x = GameObject.FindGameObjectsWithTag("Enemy");
                foreach(GameObject Enemy in x)
                {
                    enemiesInTrigger.Add(Enemy);
                }
                
                /*if (enemy != null)
                {
                    enemy.SetActive(true); // Make the door appear
                }*/
            
    }


    // When the player exits the trigger area (if needed)
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SetEnemiesActive(false);
            playerInside = false;
        }

    }


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
