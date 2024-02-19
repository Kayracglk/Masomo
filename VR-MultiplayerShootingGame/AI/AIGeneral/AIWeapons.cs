using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    public enum WeaponState
    {
        Holstering,
        Holstered,
        Activating,
        Active,
        Reloading
    }

    public enum WeaponSlot
    {
        Primary,
        Secondary,
    }

    public WeaponSlot currentWeaponSlot { get { return (WeaponSlot)current; } }
    public AIRaycastWeapon currentWeapon { get { return weapons[current]; } }
    AIRaycastWeapon[] weapons = new AIRaycastWeapon[2];
    private int current = 0;
    private AIManager agent;
    public Transform currentTarget;
    public float inaccuracy = 0f;

    public List<AIRaycastWeapon> initialWeapon = new List<AIRaycastWeapon>();

    public WeaponState weaponState = WeaponState.Holstered;

    private void Awake()
    {
        agent = GetComponent<AIManager>();
    }

    private void Start()
    {
        for (int i = 0; i < initialWeapon.Count; i++)
        {
            AIRaycastWeapon _weapon = Instantiate(initialWeapon[i]);
            Equip(_weapon);
        }
    }

    private void Update()
    {

        if (currentWeapon && currentTarget != null && IsActive())
        {
            Vector3 target = currentTarget.position + agent.weaponIK.targetOffset;
            target += Random.insideUnitSphere * inaccuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SwitchWeapon(WeaponSlot slot)
    {
        if (IsHolstered())
        {
            current = (int)slot;
            ActivateWeapon();
            return;
        }
        int equipIndex = (int)slot;
        if (IsActive() && current != equipIndex)
        {
            StartCoroutine(SwitchWeaponWithAnimation(equipIndex));
        }
    }
    public bool IsActive()
    {
        return weaponState == WeaponState.Active;
    }

    public bool IsReloading()
    {
        return weaponState == WeaponState.Reloading;
    }

    public bool IsHolstered()
    {
        return weaponState == WeaponState.Holstered;
    }

    public void SetFiring(bool enabled)
    {
        if (enabled)
        {
            currentWeapon.StartFiring();
        }
        else
        {
            currentWeapon.StopFiring();
        }
    }
    public void Equip(AIRaycastWeapon weapon)
    {
        if (weapon.weaponName == "Pistol")
        {
            weapons[1] = weapon;
            agent.meshSockets.Attach(weapon.transform, SocketId.ThighTwist);

        }
        else
        {
            weapons[0] = weapon;
            agent.meshSockets.Attach(weapon.transform, SocketId.Spine);

        }
    }

    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.transform.SetParent(null);
            currentWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            currentWeapon.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            weapons[current] = null;
            this.enabled = false;
        }
    }
    public int Count()
    {
        int count = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i])
                count++;
        }
        return count;
    }
    public bool HasWeapon()
    {
        return currentWeapon != null;
    }

    public void ActivateWeapon()
    {
        StartCoroutine(EquipWeaponWithAnimation());
    }

    public void ReloadWeapon()
    {
        if (IsActive())
        {
            StartCoroutine(ReloadWeaponWithAnimation());
        }
    }

    private IEnumerator SwitchWeaponWithAnimation(int index)
    {
        yield return StartCoroutine(HolsterWeaponWithAnimation());
        current = index;
        yield return StartCoroutine(EquipWeaponWithAnimation());
    }

    private IEnumerator ReloadWeaponWithAnimation()
    {
        weaponState = WeaponState.Reloading;
        agent.anim.SetTrigger("Reload");
        agent.weaponIK.enabled = false;
        yield return new WaitForSeconds(0.5f);
        while (agent.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        agent.weaponIK.SetAimTransform(currentWeapon.GetComponentInChildren<DebugDrawline>().transform);
        agent.weaponIK.enabled = true;
        weaponState = WeaponState.Active;
    }

    private IEnumerator EquipWeaponWithAnimation()
    {
        weaponState = WeaponState.Activating;
        agent.anim.runtimeAnimatorController = currentWeapon.animatorController;
        agent.anim.SetBool("Equip", true);
        yield return new WaitForSeconds(0.5f);
        while (agent.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        agent.weaponIK.enabled = true;
        agent.weaponIK.SetAimTransform(currentWeapon.GetComponentInChildren<DebugDrawline>().transform);
        weaponState = WeaponState.Active;
    }

    public void DeactivateWeapon()
    {
        SetTarget(null);
        SetFiring(false);
        StartCoroutine(HolsterWeaponWithAnimation());
    }

    private IEnumerator HolsterWeaponWithAnimation()
    {
        weaponState = WeaponState.Holstering;
        agent.anim.SetBool("Equip", false);
        agent.weaponIK.enabled = false;
        yield return new WaitForSeconds(0.5f);
        while (agent.anim.GetCurrentAnimatorStateInfo(1).normalizedTime < 1.0f)
        {
            yield return null;
        }
        // agent.weaponIK.SetAimTransform(currentWeapon.GetComponentInChildren<DebugDrawline>().transform);
        weaponState = WeaponState.Holstered;
    }

    public void SetTarget(Transform target)
    {
        agent.weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }
}
