using RootMotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : HealthManager
{
    private AIManager agent;


    protected override void OnAwake()
    {
        agent = GetComponent<AIManager>();
    }
    protected override void OnStart() { }
    protected override void OnDamaged() { }
    protected override void OnDeath() 
    {
        agent.stateMachine.ChangeState(AIStateID.DeathState);
    }
}
