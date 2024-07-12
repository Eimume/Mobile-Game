using UnityEditor.Rendering;
using UnityEngine;


public class water : MonoBehaviour
{

    public void  OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "water")
        {
            col.gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
    }
    public void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "water")
        {
            col.gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
    }
}
