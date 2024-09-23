using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [Header("Dependencies")]
    public EnemyShooter enemyShooter;


      public void RelayFireBullet()
    {
        // Call the FireBullet function on the EnemyShooter script
        if (enemyShooter != null)
        {
            Debug.Log("Shooting");
            enemyShooter.Shoot();
        }
        else
        {
            Debug.LogError("EnemyShooter reference is not set in AnimationEventRelay!");
        }
    }
}
