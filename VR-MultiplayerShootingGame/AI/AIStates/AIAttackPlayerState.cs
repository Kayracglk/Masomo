using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackPlayerState : AIState
{
    float timer = 0.0f;
    public AIStateID GetID()
    {
        return AIStateID.AttackPlayerState;
    }

    public void Enter(AIManager agent)
    {
        agent.weapons.SetTarget(agent.playerTransform);
        agent.weapons.ActivateWeapon();
        agent.agent.ResetPath();
        agent.agent.stoppingDistance = 5.0f;
        agent.weapons.SetFiring(true);
    }

    public void Update(AIManager agent)
    {
        ReloadWeapon(agent);
        SelectWeapon(agent);
        if (agent.playerTransform.GetComponent<HealthManager>().IsDead())
        {
            agent.stateMachine.ChangeState(AIStateID.IdleState);
        }
        ChasePlayer(agent);
    }

    public void Exit(AIManager agent)
    {
        agent.agent.stoppingDistance = 0.5f;
        agent.weapons.currentTarget = null;
        agent.weapons.SetTarget(null);
        agent.weapons.SetFiring(false);
    }

    void ReloadWeapon(AIManager agent)
    {
        var weapon = agent.weapons.currentWeapon;

        if (weapon && weapon.ammoCount <= 0)
        {
            agent.weapons.ReloadWeapon();
        }
    }
    void ChasePlayer(AIManager agent)
    {
        if (!agent.enabled) { return; }
        Vector3 direction = (agent.playerTransform.position - agent.transform.position);
        float distance = Vector3.Distance(agent.transform.position, agent.playerTransform.position);
        if (Physics.Raycast(agent.transform.position, direction, distance , agent.config.obstructionMask))
        {
            Debug.DrawLine(agent.transform.position, agent.playerTransform.position, Color.black);
            // agent.agent.SetDestination(agent.playerTransform.position);
            if(distance < 20f)
                agent.agent.SetDestination((agent.transform.position + direction)*0.1f);
        }
        else
        {
            agent.agent.ResetPath();
        }
    }
    void SelectWeapon(AIManager agent)
    {
        var bestWeapon = ChooseWeapon(agent);

        if (bestWeapon != agent.weapons.currentWeaponSlot)
        {
            agent.weapons.SwitchWeapon(bestWeapon);
        }
        agent.weapons.SetFiring(true);
    }

    private AIWeapons.WeaponSlot ChooseWeapon(AIManager agent)
    {
        float distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);

        if (distance < agent.config.closeRange)
        {
            return AIWeapons.WeaponSlot.Secondary;
        }
        else
        {
            return AIWeapons.WeaponSlot.Primary;
        }
    }
}
