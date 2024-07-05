using UnityEngine;

public class Enemy_follow : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;      
    }
    
    void Update()  
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineOfSite)
        {
               transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        
    }
}
