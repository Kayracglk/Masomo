using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimationManager : MonoBehaviour
{
    AIManager agent;
    public GameObject magazineHand;
    public float dropForce = 1.5f;

    private void Awake()
    {
        agent = GetComponent<AIManager>();
    }
    public void AnimationEvent_EquipWeapon()
    {
        if(agent.weapons.currentWeapon.weaponName == "Pistol")
            agent.meshSockets.Attach(agent.weapons.currentWeapon.transform, SocketId.PistolRightHand);
        else
            agent.meshSockets.Attach(agent.weapons.currentWeapon.transform, SocketId.RightHand);
    }

    public void AnimationEvent_UnequipWeapon()
    {
        if (agent.weapons.currentWeapon.weaponName == "Pistol")
            agent.meshSockets.Attach(agent.weapons.currentWeapon.transform, SocketId.ThighTwist);
        else
            agent.meshSockets.Attach(agent.weapons.currentWeapon.transform, SocketId.Spine);
    }

    public void AnimationEvent_DetachMagazine()
    {
        var leftHand = agent.anim.GetBoneTransform(HumanBodyBones.LeftHand);
        AIRaycastWeapon weapon = agent.weapons.currentWeapon;
        magazineHand = Instantiate(weapon.magazine, leftHand, true);
        weapon.magazine.SetActive(false);
    }
    public void AnimationEvent_DropMagazine()
    {
        GameObject droppedMagazine = Instantiate(magazineHand, magazineHand.transform.position, magazineHand.transform.rotation);
        droppedMagazine.SetActive(true);
        Rigidbody body = droppedMagazine.AddComponent<Rigidbody>();

        Vector3 dropDirection = -gameObject.transform.right;
        dropDirection += Vector3.down;

        body.AddForce(dropDirection * dropForce, ForceMode.Impulse);
        droppedMagazine.AddComponent<BoxCollider>();
        magazineHand.SetActive(false);
    }
    
    public void AnimationEvent_RefillMagazine()
    {
        magazineHand.SetActive(true);
    }
    public void AnimationEvent_AttachMagazine()
    {
        AIRaycastWeapon weapon = agent.weapons.currentWeapon;
        weapon.magazine.SetActive(true);
        Destroy(magazineHand);

        weapon.ammoCount = weapon.clipSize;
        agent.anim.ResetTrigger("Reload");
    }
}
