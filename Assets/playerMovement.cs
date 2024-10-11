using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    private enum Directions { UP , DOWN , LEFT , RIGHT }

    [Header("Movement Attributes")]
    [SerializeField] public float speed;
    //public float PlayerDamage;
    public InputAction playerControls;
    private Vector2 movement = Vector2.zero;
    private Directions _facingDirection = Directions.RIGHT;

    private readonly int _animMoveRight = Animator.StringToHash("walk");
    private readonly int _animIdle = Animator.StringToHash("idle");

    [Header("Dependencies")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        CalculateFacingDirection();
        UpdateAnimation();
    }
    void FixedUpdate()
    {
        rb.velocity = movement.normalized * speed;
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Move()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

     private void CalculateFacingDirection()
    {
        if (movement.x != 0)
        {
            // If player is on the left of the enemy, flip the sprite (scale X to -1), otherwise keep it (scale X to 1)
            if (movement.x > 0) // Moving Right
            {
                _facingDirection = Directions.RIGHT;
            }
            else if (movement.x < 0) // Moving Left
            {
                _facingDirection = Directions.LEFT;
            }
        }
    }

    private void UpdateAnimation()
    {
        if (_facingDirection == Directions.LEFT)
        {
            _spriteRenderer.flipX = true;
        }
        else if (_facingDirection == Directions.RIGHT)
        {
            _spriteRenderer.flipX = false;
        }

        if (movement.SqrMagnitude() > 0) //We're Moving
        {
            _animator.CrossFade(_animMoveRight, 0);
        }
        else
        {
            _animator.CrossFade(_animIdle, 0);
        }
    }
}
