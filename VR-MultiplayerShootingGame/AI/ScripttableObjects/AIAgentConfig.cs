using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "agentConfig", menuName = "AI/agent", order = 0)]
public class AIAgentConfig : ScriptableObject
{
    [Header("CHASE PLAYER STATE")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    [Space]

    [Header("ATTACK STATE")]
    public float closeRange = 9.0f;
    [Space]

    [Header("FIELD OF VIEW")]
    public float radius;
    [Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    [Space]

    [Header("MOVEMENT")]
    public float walkSpeed;
    public float runSpeed;
    [Space]

    [Header("LOOK AROUND STATE")]
    public float waitTime = 5.0f;
    [Space]

    [Header("WEAPON IK")]
    public float anglelimit = 120f;
    public float distancelimit = 1.5f;
    [Space]

    [Header("COVER STATE")]
    public LayerMask HidableLayers;
    public float MinObstacleHeight;
    [Range(-1f, 1f)] public float HideSensitivity;

}
