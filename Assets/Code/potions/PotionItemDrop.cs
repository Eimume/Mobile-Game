using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GachaPotionItem
{
    public Potion potion;  // The weapon associated with this drop
    [Range(0, 100)] public float dropRate; // The percentage drop rate
}
public class PotionItemDrop : MonoBehaviour
{
    public List<GachaPotionItem> gachaItems; // A list of gacha items with their respective drop rates
    public Transform spawnPoint; 

    private bool isPlayerInGachaZone = false; // A flag to check if the player is in the collider zone
    private bool hasUsedGacha = false;

    [SerializeField] Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // Check if the player presses the 'Q' key
        if (isPlayerInGachaZone && Input.GetKeyDown(KeyCode.Q) && !hasUsedGacha)
        {
            anim.SetTrigger("open");
            // Call the GachaDrop method to randomly select a weapon
            Potion droppedPotion = GachaDrop();
            if (droppedPotion != null)
            {
                Debug.Log("Dropped Weapon: " + droppedPotion.potionName);

                if (droppedPotion.potionPrefab != null && spawnPoint != null)
                {
                    Instantiate(droppedPotion.potionPrefab, spawnPoint.position, spawnPoint.rotation);
                }
                else
                {
                    Debug.LogWarning("Potion prefab or spawn point is missing!");
                }

                hasUsedGacha = true;
                Debug.Log("Gacha used. Cannot use again.");
            }
            else
            {
                Debug.Log("No potion dropped.");
            }
        }
    }


    public Potion GachaDrop()
    {
        float randomValue = Random.Range(0f, 100f); // Generate a random number between 0 and 100
        float cumulativeProbability = 0f;

        // Loop through all gacha items
        foreach (GachaPotionItem item in gachaItems)
        {
            cumulativeProbability += item.dropRate;

            // If the random value is less than the cumulative probability, this item is dropped
            if (randomValue <= cumulativeProbability)
            {
                return item.potion;
            }
        }

        return null; // If no item was selected (this shouldn't happen if the total drop rate is 100)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInGachaZone = true; // Set the flag to true when the player enters the gacha zone
            Debug.Log("Player entered the gacha zone.");
        }
    }

    // This function is called when something exits the trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInGachaZone = false; // Set the flag to false when the player leaves the gacha zone
            Debug.Log("Player left the gacha zone.");
        }
    }
}
