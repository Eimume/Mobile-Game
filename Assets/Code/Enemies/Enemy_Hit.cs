using System.Collections;
using UnityEngine;

public class Enemy_Hit : MonoBehaviour
{
    public float lineOfSite;
    public Transform Player;
    public float TimeHitCoolDown;
    private bool canHit = true;
    public float shootcooldown = 5f;
    public GameObject bulletPrefab;
    public LayerMask mask;

    private Vector2 velocity;
    private bool noWall;

    private float timer = 0f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("player").transform;
        noWall = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Ray2D ray = new Ray2D(transform.position, Player.transform.position);
        //Debug.DrawRay(ray.origin, ray.direction * lineOfSite, Color.blue);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position, lineOfSite, mask);
        float distanceFromPlayer = Vector2.Distance(Player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && canHit)
        {
            HitAction();
        }
    }
    void HitAction()
    {
        timer += Time.deltaTime;

        if (timer > shootcooldown)
        {

        }

        Debug.Log("Hit Player");

        StartCoroutine(CoolDown());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }
    IEnumerator CoolDown()
    {
        canHit = false;
        yield return new WaitForSeconds(TimeHitCoolDown);

        canHit = true;
    }

    void stopWall()
    {
        Ray2D ray = new Ray2D(transform.position, Player.transform.position);
        Debug.DrawRay(ray.origin, ray.direction * lineOfSite, Color.blue);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position, lineOfSite, mask);

        if (hit.collider != null && hit.collider.gameObject == Player)
        {
            Debug.DrawRay(ray.origin, ray.direction * lineOfSite, Color.red);
            noWall = false;
        }
    }
}
