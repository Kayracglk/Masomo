using Photon.Pun;
using UnityEngine;

public class MultiElevator : MonoBehaviourPun
{
    public Animator animator;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("interaction"))
        {
            photonView.RPC("OpenElevator", RpcTarget.All);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            photonView.RPC("CloseElevator", RpcTarget.All);
        }
    }

    [PunRPC]
    void OpenElevator()
    {
        animator.SetTrigger("Open");
        Debug.Log("Elevator Open");
    }

    [PunRPC]
    void CloseElevator()
    {
        animator.SetTrigger("Close");
        Debug.Log("Elevator Close");
    }
}
