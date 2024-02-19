using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIMovement : MonoBehaviourPunCallbacks
{
    public AIManager ai;

    public virtual void Awake()
    {
        ai = GetComponent<AIManager>();
    }

    public virtual void Start()
    {
        StartCoroutine(WaitAwakeFrame());
    }

    public virtual void Update()
    {

    }

    private IEnumerator WaitAwakeFrame()
    {
        yield return new WaitForEndOfFrame();
    }
}
