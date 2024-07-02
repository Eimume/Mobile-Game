using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = movementDirection * PlayerSpeed;
    }
}
