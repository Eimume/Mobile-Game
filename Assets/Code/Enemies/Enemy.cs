using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyType;
    public Transform player;
    public float enemySpeed;
    public float DamageEnemy;
    public bool InRed = false;
    public Transform trigger;
    private Vector2 enemyDirection;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        /*if (InRed)
        {}*/
            followPlayer();
        
    }

    public void followPlayer()
    {
        //if (Inactive == false)
        
        if (player != null)
        {
            enemyDirection = (player.position - transform.position).normalized;
            //transform.Translate(enemyDirection * enemySpeed );
            rb.velocity = enemyDirection * enemySpeed;
            //Debug.Log("Follw Player");
        }
        
    }
    void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag == "player")
        {
            InRed = true;
            Debug.Log("In");
        }
    }
    void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag == "player")
        {
            InRed = false;
            Debug.Log("Out");
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.tag == "player")
        {
            InRed = true;
            Debug.Log("In");
        }
    }
    private void OnTriggerExit2D(Collider2D Player)
    {
        if (Player.tag == "player")
        {
            InRed = false;
            Debug.Log("Out");
        }
    }
    */
    
}
