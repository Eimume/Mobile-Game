using UnityEngine;

public class bullet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemies = other.GetComponent<Enemy>();
        enemies.TakeDamage();
    }
}
