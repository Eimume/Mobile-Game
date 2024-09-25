using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
     public PlayerState currentState;  // Reference to the ScriptableObject
    private playerMovement playerMovement; // Reference to the movement script

    private int health;
    private float speed;
    private int strength;

    void Start()
    {
        // Initialize PlayerMovement and apply the state
        playerMovement = GetComponent<playerMovement>();
        ApplyState(currentState);  // Apply the initial state when the game starts
    }

    public void ApplyState(PlayerState newState)
    {
        currentState = newState;
        health = currentState.health;
        speed = currentState.speed;
        strength = currentState.strength;

        // Optionally apply to movement script if used
        if (playerMovement != null)
        {
            playerMovement.SetSpeed(currentState.speed);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Player is dead!");
        }
    }
    public void ChangeState(PlayerState newState)
    {
        currentState.OnStateExit(this);
        ApplyState(newState);
        currentState.OnStateEnter(this);
    }
}
