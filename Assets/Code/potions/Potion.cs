using UnityEngine;

public enum PotionType
{
    Healing,
    Mana,
    Stamina,
    StrengthBoost,
    SpeedBoost
    // Add more types as needed
}

[CreateAssetMenu(fileName = "New Potion", menuName = "Items/Potion")]
public class Potion : ScriptableObject
{
    public string potionName;         // The name of the potion
    public PotionType potionType;     // Type of potion (healing, etc.)
    public Sprite potionIcon;
    public int effectAmount;          // Amount of healing or other effect
    public GameObject potionPrefab;

    public virtual void Use(Player_HP playerHP)
    {
        switch (potionType)
        {
            case PotionType.Healing:
                playerHP.Heal(effectAmount);
                Debug.Log($"Used {potionName} and healed {effectAmount} HP.");
                break;
            case PotionType.Mana:
                // Logic to restore mana if you implement it
                break;
            case PotionType.Stamina:
                // Logic to restore stamina if you implement it
                break;
            case PotionType.StrengthBoost:
                // Logic to increase strength temporarily
                break;
            case PotionType.SpeedBoost:
                // Logic to increase speed temporarily
                break;
            // Add more cases as needed
        }
    }
}


[CreateAssetMenu(fileName = "New Healing Potion", menuName = "Items/Heal")]
public class HealPotion : Potion
{
    public override void Use(Player_HP playerHP)
    {
        if (potionType == PotionType.Healing)
        {
            playerHP.Heal(effectAmount);
            Debug.Log($"Used {potionName} and healed {effectAmount} HP.");
        }
    }

    
}
