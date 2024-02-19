#if PUN_2_OR_NEWER
using Photon.Pun;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PUN_2_OR_NEWER
namespace BNG {
    public class NetworkedRaycastWeapon : RaycastWeapon, IPunObservable {

        PhotonView photonView;

        void Start() {
            photonView = GetComponent<PhotonView>();
        }

        public override void Shoot() {
            if (photonView) {
                var playEmptySound = !BulletInChamber && MustChamberRounds && !playedEmptySound || ws != null && ws.LockedBack;
                photonView.RPC("ShootRPC", RpcTarget.Others, playEmptySound);
            }

            base.Shoot();
        }
        public override void OnRaycastHit(RaycastHit hit) {
            photonView.RPC("ApplyParticleFXRPC", RpcTarget.All, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal), hit.collider.tag);

            base.OnRaycastHit(hit);
        }
        
        [PunRPC]
        private void ApplyParticleFXRPC(Vector3 position, Quaternion rotation, string tag) {
            // efecklerin taglerini ayarla  duvar, plastik, cam, hedef
            GameObject impactPrefab = null;

            switch (tag) {
                case "DuvarFX":
                    impactPrefab = ImpactFXPrefabs.Length > 0 ? ImpactFXPrefabs[0] : null;
                    break;
                case "PlastikFX":
                    impactPrefab = ImpactFXPrefabs.Length > 1 ? ImpactFXPrefabs[1] : null;
                    break;
                case "CamFX":
                    impactPrefab = ImpactFXPrefabs.Length > 2 ? ImpactFXPrefabs[2] : null;
                    break;
                case "HedefFX":
                    impactPrefab = ImpactFXPrefabs.Length > 3 ? ImpactFXPrefabs[3] : null;
                    break;
            }

            if (impactPrefab != null) {
                // Spawn the impact effect
                GameObject impact = PhotonNetwork.Instantiate(impactPrefab.name, position, rotation);
            }
        }
        [PunRPC]
        private void ShootRPC(bool playEmptySound) {
            if (playEmptySound) {
                VRUtils.Instance.PlaySpatialClipAt(EmptySound, transform.position, EmptySoundVolume, 0.5f);
                return;
            }

            VRUtils.Instance.PlaySpatialClipAt(GunShotSound, transform.position, GunShotVolume);

            shotRoutine = AutoChamberRounds ? animateSlideAndEject() : doMuzzleFlash();
            StartCoroutine(shotRoutine);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
            // This is our object, send our positions to the other players
            if (stream.IsWriting && photonView.IsMine) {
                // Send Weapon state such as firing, ammo, etc.
                //stream.SendNext(transform.position);
               
            }
            // Receive updates
            else {
               
            }
        }

    }
}
#endif