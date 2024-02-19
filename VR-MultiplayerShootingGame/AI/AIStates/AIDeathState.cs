using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{

    public AIStateID GetID()
    {
        return AIStateID.DeathState;
    }
    public void Enter(AIManager agent)
    {
        agent.agent.isStopped = true;
        agent.ragdoll.ActiveteRagdoll();
        agent.weapons.DropWeapon();
    }

    public void Update(AIManager agent)
    {
        
    }

    public void Exit(AIManager agent)
    {

    }
}
