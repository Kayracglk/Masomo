using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AILookAroundState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.LookAroundState;
    }

    public void Enter(AIManager agent)
    {
        LookAroundWithInMovement(agent);
    }

    public void Update(AIManager agent)
    {
        if(agent.agent.remainingDistance <= 1.0f)
        {
            LookAroundWithOutMovement(agent);
        }
    }

    public void Exit(AIManager agent)
    {
        agent.agent.ResetPath();
    }

    public void LookAroundWithOutMovement(AIManager agent)
    {
        agent.stateMachine.ChangeState(AIStateID.IdleState);
    }

    public void LookAroundWithInMovement(AIManager agent)
    {
        GetRandomPositionInArea(agent);
    }

    private void GetRandomPositionInArea(AIManager agent)
    {
        Vector3 randomPoint = RandomPointInBounds(agent);
        agent.agent.speed = agent.config.walkSpeed;
        agent.agent.SetDestination(randomPoint);
    }

    private Vector3 RandomPointInBounds(AIManager agent)
    {
        return new Vector3(Random.Range(agent.bounds.min.x, agent.bounds.max.x), 0f, Random.Range(agent.bounds.min.z, agent.bounds.max.z));
    }

    
}
