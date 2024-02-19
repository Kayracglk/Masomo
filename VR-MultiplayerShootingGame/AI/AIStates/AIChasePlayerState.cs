using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasePlayerState : AIState
{
    float timer = 0.0f;

    public AIStateID GetID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(AIManager agent)
    {
        
    }

    public void Update(AIManager agent)
    {
        if (!agent.enabled) { return; }

        timer -= Time.deltaTime;
        if (!agent.agent.hasPath)
        {
            agent.agent.destination = agent.playerTransform.position;
        }
        if (timer < 0.0f)
        {
            Vector3 direction = (agent.playerTransform.position - agent.agent.destination);
            direction.y = 0.0f;
            if (direction.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                if (agent.agent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
                {
                    agent.agent.destination = agent.playerTransform.position;

                }
            }
            timer = agent.config.maxTime;
        }
    }

    public void Exit(AIManager agent)
    {
        
    }
}
