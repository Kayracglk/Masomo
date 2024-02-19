using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
using Invector;
#endif
using Photon.Pun;
using Photon.Realtime;

namespace BNG {
    public class NetworkDamage : MonoBehaviourPun, IPunObservable {
        public float currentHealth = 100f;
        private bool destroyed = false;

        [Header("Events")]
        public UnityEvent<float> onDamaged;
        public UnityEvent onDestroyed;

        Rigidbody rigid;
        bool initialWasKinematic;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
            if (rigid) {
                initialWasKinematic = rigid.isKinematic;
            }
        }

        public void DealDamage(float damageAmount) {
            DealDamage(damageAmount, transform.position);
        }

        public void DealDamage(float damageAmount, Vector3? hitPosition = null, Vector3? hitNormal = null, bool reactToHit = true, GameObject sender = null, GameObject receiver = null) {
            if (destroyed) {
                return;
            }

            currentHealth -= damageAmount;

            // Invoke the onDamaged event
            onDamaged?.Invoke(damageAmount);

            // Invector Integration
#if INVECTOR_BASIC || INVECTOR_AI_TEMPLATE
            if (photonView.IsMine && SendDamageToInvector) {
                var d = new Invector.vDamage();
                d.hitReaction = reactToHit;
                d.hitPosition = (Vector3)hitPosition;
                d.receiver = receiver == null ? this.gameObject.transform : null;
                d.damageValue = (int)damageAmount;

                this.gameObject.ApplyDamage(new Invector.vDamage(d));
            }
#endif

            
            if (currentHealth <= 0) {
                photonView.RPC("RPC_DestroyThis", RpcTarget.All);
            }
        }

        [PunRPC]
        private void RPC_DestroyThis() {
            currentHealth = 0;
            destroyed = true;

            // Additional destruction logic...

            // Invoke the onDestroyed event
            onDestroyed?.Invoke();

            if (photonView.IsMine) {
                photonView.RPC("RPC_DestroyObject", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void RPC_DestroyObject() {
            // Destroy the object on all clients
            Destroy(gameObject);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                // Sending data to other clients
                stream.SendNext(currentHealth);
            } else {
                // Receiving data from owner client
                currentHealth = (float)stream.ReceiveNext();
            }
        }
    }
}