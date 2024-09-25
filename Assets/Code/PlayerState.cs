using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "Player/PlayerState")]
public class PlayerState : ScriptableObject
{
    public string stateName;
    public int health;
    public float speed;
    public int strength;

     public virtual void OnStateEnter(Player player)
    {
        // Custom behavior when entering the state
        Debug.Log($"Entering {stateName} state.");
    }

    public virtual void OnStateExit(Player player)
    {
        // Custom behavior when exiting the state
        Debug.Log($"Exiting {stateName} state.");
    }
}

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player/NewStateTest")]
public class Upgrate : PlayerState
{

}
