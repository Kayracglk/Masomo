using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    SoldierManager soldierManager;

    private void Awake()
    {
        soldierManager = GetComponent<SoldierManager>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        DeactiveteRagdoll();
    }

    public void DeactiveteRagdoll()
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = true;
        }
        soldierManager.anim.enabled = true;
    }
    public void ActiveteRagdoll()
    {
        soldierManager.agent.isStopped = true;
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = false;
        }
        soldierManager.anim.enabled = false;
    }
}
