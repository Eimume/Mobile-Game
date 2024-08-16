using UnityEngine;

public class Enemy_status : MonoBehaviour
{
    public float EnemyDamage;
    public float EnemyMaxHp = 20f;
    public float EnemyCurrentHp;
    public bool getHit = false;
    //private Transform player;

    Player player = new Player();

    void Start()
    {
        getHit = false;
        EnemyCurrentHp = EnemyMaxHp;
        //player = GameObject.FindGameObjectWithTag("player").transform;
    }
    void Update()
    {

    }

    void Attack()
    {
        Debug.Log("Enemy: Attack player");
    }
    public void GetAttack()
    {
    //EnemyCurrentHp -=    

    }

}
