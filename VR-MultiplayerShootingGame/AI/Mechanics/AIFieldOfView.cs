using System;
using System.Collections;
using UnityEngine;

public class AIFieldOfView : MonoBehaviour
{
    AIManager agent;
    public bool canSeePlayer;
    private bool canChangeLookState = true;
    private bool canChangeState = true;
    private void Awake()
    {
        agent = GetComponent<AIManager>();
    }
    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, agent.config.radius, agent.config.targetMask);

        if (rangeChecks.Length != 0)
        {
            for (int i = 0; i < rangeChecks.Length; i++)
            {
                Transform target = rangeChecks[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < agent.config.angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, agent.config.obstructionMask))
                    {
                        canSeePlayer = true;
                        canChangeLookState = true;
                        canChangeState = false;
                        agent.playerTransform = target;
                        EnemyManager.instance.AttackAllPlayer(agent);
                        enabled = false;
                        return;
                    }
                }
            }
        }
        canSeePlayer = false;
    }
}
