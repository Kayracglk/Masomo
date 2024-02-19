using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Pense : MonoBehaviourPun
{
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PenseAnimation()
    {
        photonView.RPC("RPC_PenseAnimation", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_PenseAnimation()
    {
        animator.SetTrigger("Pense");
    }
}
