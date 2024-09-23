using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    public GameObject nearestEnemy;
    public GameObject attackPos;
    public LayerMask mask;
    public float attackRadius;
    public GameObject Bullet;
    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        FindNearest();
    }
    public void FindNearest()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        //Debug.Log("Nearest enemy is: " + nearestEnemy.name + " at a distance of " + shortestDistance);

        if(nearestEnemy != null && Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    public void Attack()
    {
        GameObject bullet = Instantiate(Bullet,transform.position,Quaternion.identity);
        Rigidbody2D bullets = bullet.GetComponent<Rigidbody2D>();
        Vector3 angle = (nearestEnemy.transform.position - transform.position ).normalized;
        bullets.AddForce(angle * speed, ForceMode2D.Impulse);
    }
    public void AttackMelee()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(attackPos.transform.position, attackRadius, mask);
        if(hit.Length > 0)
        {
            foreach(Collider2D c in hit)
            {
                Debug.Log(c.name);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.transform.position, attackRadius);
    }
}