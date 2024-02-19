using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BNG
{

    /// <summary>
    /// An object than do damage and play hit FX
    /// </summary>
    public class ProjectilePompali : MonoBehaviour
    {

        public GameObject[] ImpactFXPrefabs;
        public GameObject HitFXPrefab;
        private bool _checkRaycast;
        public float Damage = 25;
        public float MaxsimumRadius = 5f; // Maksimum collider radius artışı
        public float RadiusRate = 0.1f; // Her saniyede artış miktarı

        private float currentRadiusIncrease = 0f;

        /// <summary>
        /// Add force to rigidbody on impact
        /// </summary>
        public float AddRigidForce = 5;
        public LayerMask ValidLayers;
        public bool StickToObject = false;

        /// <summary>
        /// Minimum Z velocity required to register as an impact
        /// </summary>
        public float MinForceHit = 0.02f;

        [Tooltip("Unity Event called when the projectile damages something")]
        public UnityEvent onDealtDamageEvent;

        private void Update()
        {
            if (currentRadiusIncrease < MaxsimumRadius)
            {
                // Artış miktarını zamana göre hesapla
                float increaseAmount = RadiusRate * Time.deltaTime;

                // Mevcut artış miktarını güncelle
                currentRadiusIncrease += increaseAmount;

                // Collider'ı güncelle
                UpdateColliderRadius();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEvent(collision);
        }
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEvent(other);
        }
        public virtual void OnTriggerEvent(Collider other)
        {
            if (other.CompareTag("Animal"))
            {
                IDamagable d = other.gameObject.GetComponent<IDamagable>();
                if (d != null)
                {
                    d.GiveDamage(Vector3.zero, Damage);
                }
            }
        }
        public virtual void OnCollisionEvent(Collision collision)
        {
            // Ignore Triggers
            if (collision.collider.isTrigger)
            {
                return;
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb && MinForceHit != 0)
            {
                float zVel = System.Math.Abs(transform.InverseTransformDirection(rb.velocity).z);

                // Minimum Force not achieved
                if (zVel < MinForceHit)
                {
                    return;
                }
            }

            Vector3 hitPosition = collision.contacts[0].point;
            Vector3 normal = collision.contacts[0].normal;
            Quaternion hitNormal = Quaternion.FromToRotation(Vector3.forward, normal);

            // FX - Particles, Decals, etc.
            DoHitFX(hitPosition, hitNormal, collision.collider);
            ApplyParticleFX(hitPosition, hitNormal, collision.collider);

            // Damage if possible
            IDamagable d = collision.gameObject.GetComponent<IDamagable>();
            if (d != null)
            {
                print(collision.gameObject);
                d.GiveDamage(hitPosition, Damage);
                if (onDealtDamageEvent != null)
                {
                    onDealtDamageEvent.Invoke();
                }
            }

            if (StickToObject)
            {
                // tryStickToObject
            }
            else
            {
                // Done with this projectile
                Destroy(this.gameObject);
            }
        }

        public virtual void DoHitFX(Vector3 pos, Quaternion rot, Collider col)
        {

            // Create FX at impact point / rotation
            if (HitFXPrefab)
            {
                GameObject impact = Instantiate(HitFXPrefab, pos, rot) as GameObject;
                BulletHole hole = impact.GetComponent<BulletHole>();
                if (hole)
                {
                    hole.TryAttachTo(col);
                }
            }

            // push object if rigidbody
            Rigidbody hitRigid = col.attachedRigidbody;
            if (hitRigid != null)
            {
                hitRigid.AddForceAtPosition(transform.forward * AddRigidForce, pos, ForceMode.VelocityChange);
            }
        }

        /// <summary>
        /// A projectile can be converted into a raycast if time reverts to full speed (or more)
        /// </summary>
        public virtual void MarkAsRaycastBullet()
        {
            _checkRaycast = true;
            StartCoroutine(CheckForRaycast());
        }

        public virtual void DoRayCastProjectile()
        {

            // Raycast to hit
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 25f, ValidLayers, QueryTriggerInteraction.Ignore))
            {
                Quaternion decalRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                DoHitFX(hit.point, decalRotation, hit.collider);
            }

            _checkRaycast = false;

            // Done with this projectile
            Destroy(this.gameObject);
        }

        IEnumerator CheckForRaycast()
        {
            while (this.gameObject.activeSelf && _checkRaycast)
            {
                // Switch to raycast
                if (Time.timeScale >= 1)
                {
                    DoRayCastProjectile();
                }

                yield return new WaitForEndOfFrame();
            }
        }
        public virtual void ApplyParticleFX(Vector3 position, Quaternion rotation, Collider attachTo)
        {
            GameObject impactPrefab = null;

            // Check the type of the object hit
            if (attachTo.CompareTag("Su"))
            {
                // Use the first impact effect prefab for walls
                impactPrefab = ImpactFXPrefabs.Length > 0 ? ImpactFXPrefabs[0] : null;


            }
            if (attachTo.CompareTag("Agac"))
            {
                // Use the second impact effect prefab for metal objects
                impactPrefab = ImpactFXPrefabs.Length > 1 ? ImpactFXPrefabs[1] : null;


            }
            if (attachTo.CompareTag("Animal"))
            {
                // Use the third impact effect prefab for target objects
                impactPrefab = ImpactFXPrefabs.Length > 2 ? ImpactFXPrefabs[2] : null;

            }

            if (attachTo.CompareTag("CaliFX"))
            {
                // Use the third impact effect prefab for target objects
                impactPrefab = ImpactFXPrefabs.Length > 3 ? ImpactFXPrefabs[3] : null;



            }


            if (impactPrefab != null)
            {
                // Spawn the impact effect
                GameObject impact = Instantiate(impactPrefab, position, rotation) as GameObject;
                Sound sound = new Sound(position, 20);
                Sounds.MakeSound(sound);

                // Parent the impact effect to the attached object
                // impact.transform.parent = attachTo.transform;

                // Check for input to destroy the object
                if (InputBridge.Instance.XButton)
                {
                    // Destroy the impact effect
                    Destroy(impact);
                }

            }
        }
        private void UpdateColliderRadius()
        {
            // Collider bileşenini al
            SphereCollider sphereCollider = GetComponent<SphereCollider>();

            if (sphereCollider != null)
            {
                // Radius'u güncelle
                sphereCollider.radius += currentRadiusIncrease;
            }
        }
    }
}

