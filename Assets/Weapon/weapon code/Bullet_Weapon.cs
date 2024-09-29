using UnityEngine;

public class Bullet_Weapon : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    private int bulletDamage;
    public float lifeTime = 3f; // How long the bullet exists before being destroyed

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the bullet after a set time
    }
    void Update()
    {
        // Move the bullet forward based on its speed
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    public void SetDamage(int damage)
    {
        bulletDamage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with an enemy
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage); // Deal damage to the enemy
            }

            Destroy(gameObject);  // Destroy bullet when it hits the enemy
            // You could also apply damage to the enemy here
        }
    }
}
