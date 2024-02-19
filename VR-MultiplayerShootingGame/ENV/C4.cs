using UnityEngine;
using Photon.Pun;

public class C4 : MonoBehaviourPunCallbacks
{
    public Animator animator; // Animator component'ını referans almak için
    public GameObject RedLazerDeactive; // RedLazerDeactive objesini referans almak için

    [PunRPC]
    void DeactivateRedLazer()
    {
        RedLazerDeactive.SetActive(false);
    }

    [PunRPC]
    void TriggerAnimation()
    {
        animator.SetTrigger("kablo1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pense"))
        {
            Debug.Log("C4");

            // Photon RPC ile tüm oyunculara DeactivateRedLazer fonksiyonunu çağır
            photonView.RPC("DeactivateRedLazer", RpcTarget.AllBuffered);

            // Photon RPC ile tüm oyunculara TriggerAnimation fonksiyonunu çağır
            photonView.RPC("TriggerAnimation", RpcTarget.AllBuffered);

            Debug.Log("Kablo1");
        }
    }
}
