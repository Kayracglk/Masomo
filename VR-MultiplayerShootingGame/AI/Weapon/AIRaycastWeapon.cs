using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initalPosition;
        public Vector3 initalVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }

    // public ActiveWeapon.WeaponSlot weaponSlot;
    public SocketId holsterSocket;
    public bool isFiring = false;
    public int fireRate;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0.0f;
    public int maxBounces = 0;
    public bool debug = false;
    public GameObject[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public string weaponName;

    public RuntimeAnimatorController animatorController;

    public int ammoCount;
    public int clipSize;
    public float damage = 10;

    public Transform raycastOrigin;
    public WeaponRecoil recoil;
    public GameObject magazine;

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3.0f;


    private void Awake()
    {
        recoil = GetComponent<WeaponRecoil>();
    }

    private Vector3 GetPosition(Bullet bullet)
    {
        // p + v*t + 0.5 *g * t*t -> SERBEST DUSME
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initalPosition) + (bullet.initalVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time); 
    }

    private Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initalPosition = position;
        bullet.initalVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounces;
        // print("FIRE");
        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        if(accumulatedTime > 0.0f)
        {
            accumulatedTime = 0.0f;
        }
        recoil.Reset();
    }

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        if(isFiring)
        {
            UpdateFiring(deltaTime, target);
        }
        accumulatedTime += deltaTime;
        UpdateBullets(deltaTime);
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }
    
    private void UpdateFiring(float deltaTime, Vector3 target)
    {
        float fireInterval =  1.0f / fireRate;
        while(accumulatedTime >= 0)
        {
            FireBullet(target);

            accumulatedTime -= fireInterval;
        }
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time > maxLifeTime);
    }
    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);

        });
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        Color debugColor = Color.green;

        if(Physics.Raycast(ray, out hitInfo, distance))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(0);
            bullet.time = maxLifeTime;
            end = hitInfo.point;
            debugColor = Color.red;

            if(bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initalPosition = hitInfo.point;
                bullet.initalVelocity = Vector3.Reflect(bullet.initalVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if(rb2d != null )
            {
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }

            //var hitBox = hitInfo.collider.GetComponent<HitBox>();
            //if(hitBox != null )
            //{
            //    hitBox.OnRaycastHit(this, ray.direction);
            //}
        }

        bullet.tracer.transform.position = end;

        if(debug)
        {
            Debug.DrawLine(start, end, debugColor, 1.0f);
        }
    }

    private void FireBullet(Vector3 target)
    {
        if(ammoCount <= 0)
        {
            return;
        }
        ammoCount--;
        Instantiate(muzzleFlash[0], raycastOrigin);

        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        recoil.GenerateRecoil(weaponName);
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
