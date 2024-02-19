using UnityEngine;
using UnityEngine.AI;

public class AICoverState : AIState
{
    private Collider[] Colliders = new Collider[10];
    private AIManager _agent;

    public AIStateID GetID()
    {
        return AIStateID.CoverState;
    }

    public void Enter(AIManager agent)
    {
        _agent = agent;
        agent.agent.ResetPath();
        Hide(agent);
        
    }
    public void Update(AIManager agent)
    {
        if(agent.agent.remainingDistance < 1.0f)
        {
            agent.stateMachine.ChangeState(AIStateID.AttackPlayerState);
        }
    }

    public void Exit(AIManager agent)
    {
        agent.transform.LookAt(agent.playerTransform.position);
        agent.agent.ResetPath();
    }

    private void Hide(AIManager agent)
    {
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i] = null;
        }

        int hits = Physics.OverlapSphereNonAlloc(agent.transform.position, agent.config.radius, Colliders, agent.config.HidableLayers);

        int hitReduction = 0;
        for (int i = 0; i < hits; i++)
        {
            if (Vector3.Distance(Colliders[i].transform.position, agent.playerTransform.position) < agent.config.closeRange || Colliders[i].bounds.size.y < agent.config.MinObstacleHeight)
            {
                Colliders[i] = null;
                hitReduction++;
            }
        }
        hits -= hitReduction;

        System.Array.Sort(Colliders, ColliderArraySortComparer);

        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(Colliders[i].transform.position, out NavMeshHit hit, 2f, agent.agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.agent.areaMask))
                {
                    Debug.LogError($"Unable to find edge close to {hit.position}");
                }

                if (Vector3.Dot(hit.normal, (agent.playerTransform.position - hit.position).normalized) < agent.config.HideSensitivity)
                {
                    agent.agent.speed = agent.config.runSpeed;
                    agent.agent.SetDestination(hit.position);
                    break;
                }
                else
                {
                    if (NavMesh.SamplePosition(Colliders[i].transform.position - (agent.playerTransform.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.agent.areaMask))
                    {
                        if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.agent.areaMask))
                        {
                            Debug.LogError($"Unable to find edge close to {hit2.position} (second attempt)");
                        }

                        if (Vector3.Dot(hit2.normal, (agent.playerTransform.position - hit2.position).normalized) < agent.config.HideSensitivity)
                        {
                            agent.agent.speed = agent.config.runSpeed;
                            agent.agent.SetDestination(hit2.position);
                            break;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError($"Unable to find NavMesh near object {Colliders[i].name} at {Colliders[i].transform.position}");
            }
        }
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null)
        {
            return 1;
        }
        else if (A != null && B == null)
        {
            return -1;
        }
        else if (A == null && B == null)
        {
            return 0;
        }
        else
        {
            return Vector3.Distance(_agent.agent.transform.position, A.transform.position).CompareTo(Vector3.Distance(_agent.agent.transform.position, B.transform.position));
        }
    }

}
