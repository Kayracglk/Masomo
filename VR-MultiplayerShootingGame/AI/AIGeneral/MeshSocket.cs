using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    AIManager agent;
    public SocketId socketID;
    public HumanBodyBones bone;
    private Transform attachPoint;

    public Vector3 offset;
    public Vector3 rotation;


    private void Awake()
    {
        agent = GetComponentInParent<AIManager>();
    }

    private void Start()
    {
        attachPoint = new GameObject("socket" + socketID).transform;
        attachPoint.SetParent(agent.anim.GetBoneTransform(bone));
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);

    }
    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}
