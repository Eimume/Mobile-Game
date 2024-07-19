using UnityEngine;

public class Enemy_Follow_stop : MonoBehaviour
{
   public float speed;
    public float lineOfSite;
    public float stopLine;
    private Transform player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;      
    }
    
    void Update()  
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < lineOfSite && distanceFromPlayer > stopLine)
        {
               transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopLine);
        
    }
}
