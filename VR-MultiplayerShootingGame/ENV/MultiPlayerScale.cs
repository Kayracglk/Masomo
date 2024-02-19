
using Photon.Pun;
using Photon.Realtime;
using RootMotion.FinalIK;
using UnityEngine;

namespace BNG {
    public class MultiPlayerScale : MonoBehaviourPunCallbacks, IPunObservable {

        public VRIK ik;
        public float scaleMlp = 1f;
        public ControllerBinding ScalePlayerInput = ControllerBinding.AButtonDown;

        void Update() {
            if (photonView.IsMine) {
                // Sadece yerel oyuncu bu kodu çalıştırabilir ve ölçeklendirmeyi başlatabilir.
                if (InputBridge.Instance.GetControllerBindingValue(ScalePlayerInput)) {
                    ScalePlayerLocally();
                }
            }
        }

        void ScalePlayerLocally() {
            // Sadece yerel oyuncu tarafından çağrılacak olan ölçekleme kodu
            float sizeF = (ik.solver.spine.headTarget.position.y - ik.references.root.position.y) / (ik.references.head.position.y - ik.references.root.position.y);
            ik.references.root.localScale *= sizeF * scaleMlp;

            // Ölçekleme bilgisini ağ üzerinde diğer oyunculara gönder
            photonView.RPC("SyncScale", RpcTarget.OthersBuffered, sizeF * scaleMlp);
        }

        [PunRPC]
        void SyncScale(float syncedScale) {
            // Diğer oyuncular bu RPC çağrısı ile ölçeklemeyi takip eder
            ik.references.root.localScale *= syncedScale;
        }

        // IPunObservable arayüzünü uygulayarak senkronizasyonu sağlayabilirsiniz.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            if (stream.IsWriting) {
                // Senkronize edilecek verileri yazın.
                // Örneğin: stream.SendNext(ik.references.root.localScale);
            } else {
                // Senkronize edilen verileri okuyun.
                // Örneğin: ik.references.root.localScale = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
