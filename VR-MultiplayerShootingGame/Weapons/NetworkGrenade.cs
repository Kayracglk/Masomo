using System.Collections;
using UnityEngine;
using BNG;
using Photon.Pun;

[RequireComponent(typeof(AudioSource))]
public class NetworkGrenade : MonoBehaviourPun, IPunObservable
{
    public float delay = 3f;
    public float destroyDelay = 3f;
    public float damage = 25f;
    public int explosionRadius = 4;
    public int explosionForce = 400;
    public GameObject explosionEffect;

    private AudioSource boomClip;
    private MeshRenderer[] meshRenderers; // Birden fazla MeshRenderer'ı saklamak için dizi

    void Start()
    {
        boomClip = GetComponent<AudioSource>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Disable the script for remote players
        if (!photonView.IsMine)
        {
            enabled = false;
        }
    }

    public void TriggerBoom()
    {
        StartCoroutine(WaitForIt());
    }

    private IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(delay);

        photonView.RPC("Explode", RpcTarget.AllBuffered);

        foreach (MeshRenderer mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

        yield return new WaitForSeconds(destroyDelay);

        photonView.RPC("DestroyGrenade", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void Explode()
    {
        boomClip.Play();

        PhotonNetwork.Instantiate(explosionEffect.name, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Check if the object has a PhotonView before applying explosion force
            PhotonView pv = nearbyObject.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }

                Damageable d = nearbyObject.GetComponent<Damageable>();
                if (d != null)
                {
                    d.DealDamage(damage);
                }
            }
        }
    }

    [PunRPC]
    private void DestroyGrenade()
    {
        Destroy(gameObject);
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        PhotonNetwork.Destroy(gameObject);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Implement synchronization of relevant data across the network if needed.
    }
}
