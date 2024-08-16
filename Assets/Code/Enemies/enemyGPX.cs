using UnityEngine;
using Pathfinding;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class enemyGPX : MonoBehaviour
{
    public AIPath aiPath;


    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
