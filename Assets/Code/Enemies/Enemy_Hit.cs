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

    private float timer = 0f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
}
