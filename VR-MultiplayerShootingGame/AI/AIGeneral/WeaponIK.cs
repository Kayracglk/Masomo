using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float boneWeight;
}

public class WeaponIK : MonoBehaviour
{
    AIManager agent;

    public Transform targetTransform;
    public Transform aimTransform;
    public Vector3 targetOffset;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    [SerializeField] private int iteration = 10;
    [Range(0f, 1f)]
    [SerializeField] private float weight = 1.0f;

    private void Awake()
    {
        agent = GetComponent<AIManager>();
    }

    private void Start()
    {
        boneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = agent.anim.GetBoneTransform(humanBones[i].bone);
        }
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + targetOffset) - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);

        if(targetAngle > agent.config.anglelimit)
        {
            blendOut += (targetAngle - agent.config.anglelimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < agent.config.distancelimit)
        {
            blendOut += agent.config.distancelimit - targetDistance;
        }
        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        
        return aimTransform.position + direction;
    }

    private void LateUpdate()
    {
        if(aimTransform == null || targetTransform == null) { return; }
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < iteration; i++)
        {
            for (int j = 0; j < boneTransforms.Length; j++)
            {
                Transform bone = boneTransforms[j];
                float boneWeigh = humanBones[j].boneWeight * weight;
                AimAtTarget(bone, targetPosition, boneWeigh);
            }
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = aimTowards * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform _aimTransform)
    {
        aimTransform = _aimTransform;
    }
}
