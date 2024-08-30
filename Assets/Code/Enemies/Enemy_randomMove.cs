using UnityEngine;
using System.Collections;

public class Enemy_randomMove : MonoBehaviour
{
    //public Animator anim;
    public Rigidbody2D Rigidbody;
    public Transform[] targets;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WALKAROUND());
    }


    IEnumerator Reachpoint(Transform target)
    {
        //anim.SetFloat("walk", 1f);

        while (Vector3.Distance(this.transform.position, target.position) > 0.1f)
        {
            Vector3 direction = (target.position - this.transform.position).normalized;
            Rigidbody.velocity = direction * 2;  // 'speed' is a float variable you need to define
            yield return null; // Wait until the next frame
        }

        Rigidbody.velocity = Vector3.zero; // Stop the object when it reaches the target
        //anim.SetFloat("walk", 0);
    }


    IEnumerator WALKAROUND()
    {
        while (true)
        {
            yield return StartCoroutine(Reachpoint(targets[Random.Range(0, targets.Length)]));
            yield return new WaitForSeconds(2);
        }
    }
}
