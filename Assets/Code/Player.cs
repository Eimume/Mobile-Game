using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    public float PlayerDamage;
    public InputAction playerControls;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playermovement();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        
    }


    void playermovement()
    {
         movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = movementDirection * PlayerSpeed;
    }

    void Attack()
    {
 
        Debug.Log("Damage " + PlayerDamage);
        
    }

    void Damage()
    {
        Debug.Log(PlayerDamage);
    }
}
