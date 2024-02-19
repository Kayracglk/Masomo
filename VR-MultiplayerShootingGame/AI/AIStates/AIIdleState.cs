using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private float elapsedTime = 0f;
    private bool hasWaited = false;

    public AIStateID GetID()
    {
        return AIStateID.IdleState; 
    }

    public void Enter(AIManager agent)
    {
        agent.weapons.DeactivateWeapon();
        agent.agent.ResetPath();
        elapsedTime = 0f;
        hasWaited = false;
    }
    public void Update(AIManager agent)
    {
        if (!hasWaited)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= agent.config.waitTime)
            {
                agent.stateMachine.ChangeState(AIStateID.LookAroundState);
                hasWaited = true;
            }
        }
    }

    public void Exit(AIManager agent)
    {
        
    }
}
