using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float attackRange = 5f;

    private EnemyPathfinding enemyPathfinding;
    private bool chasingPlayer = false;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();   
    }

    public void Attack()
    {
        if (!chasingPlayer)
        {
            chasingPlayer = true;
            StartCoroutine(ChasingPlayerRoutine());
        }
    }

    private IEnumerator ChasingPlayerRoutine()
    {
        while (true)
        {
            float dist = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

            if (dist > attackRange)
            {
                chasingPlayer = false;
                yield break;
            }

            enemyPathfinding.MoveTowardsPlayer(PlayerController.Instance.transform.position);

            yield return null;
        }
    }
}
