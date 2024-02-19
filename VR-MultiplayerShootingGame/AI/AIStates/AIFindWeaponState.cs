using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIState
{
    public AIStateID GetID()
    {
        return AIStateID.FindWeaponState;
    }

    public void Enter(AIManager agent)
    {
        WeaponPickUp pickUp = FindClosestWeapon(agent);
        if (pickUp != null)
        {
            agent.agent.destination = pickUp.transform.position;
            agent.agent.speed = 5;
        }
    }

    public void Update(AIManager agent)
    {
        if(agent.weapons.Count() == 2)
        {
            agent.stateMachine.ChangeState(AIStateID.AttackPlayerState);
        }
        else
        {
            if(!agent.agent.hasPath)
            {
                WeaponPickUp pickUp = FindClosestWeapon(agent);
                if (pickUp != null)
                {
                    agent.agent.destination = pickUp.transform.position;
                }
            }
        }
    }

    public void Exit(AIManager agent)
    {
        
    }

    private WeaponPickUp FindClosestWeapon(AIManager agent)
    {
        WeaponPickUp[] weapons = Object.FindObjectsOfType<WeaponPickUp>();
        WeaponPickUp closestWeapon = null;
        if (weapons.Length > 0)
        {
            closestWeapon = weapons[0];
            float minDistance = Vector3.Distance(agent.transform.position, closestWeapon.transform.position);
            for(int i = 1; i < weapons.Length; i++)
            {
                float distance = Vector3.Distance(agent.transform.position, weapons[i].transform.position);
                if (minDistance > distance)
                {
                    closestWeapon = weapons[i];
                    minDistance = distance;
                }
            }
        }
        return closestWeapon;
    }
}
