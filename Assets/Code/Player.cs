using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    public float PlayerDamage;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        playermovement();
        Attack();
    }

    void playermovement()
    {
         movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = movementDirection * PlayerSpeed;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Damage " + PlayerDamage );

        }
    }
}
