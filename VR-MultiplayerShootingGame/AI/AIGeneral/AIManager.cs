using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public abstract class AIManager : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public AIWeapons weapons;
    [HideInInspector] public MeshSockets meshSockets;
    [HideInInspector] public WeaponIK weaponIK;
    [HideInInspector] public AIAnimationManager animationManager;
    [HideInInspector] public AIFieldOfView fieldOfView;
    [HideInInspector] public Bounds bounds;

    public Collider walkableArea;
    public AIAgentConfig config;
    public AIStateID initalState;
    
    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        weapons = GetComponent<AIWeapons>();
        meshSockets = GetComponent<MeshSockets>();
        weaponIK = GetComponent<WeaponIK>();
        animationManager = GetComponent<AIAnimationManager>();
        fieldOfView = GetComponent<AIFieldOfView>();
        bounds = walkableArea.bounds;

        stateMachine = new AIStateMachine(this);
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public virtual void Start()
    {
        stateMachine.RegisterState(new AIDeathState());
        stateMachine.RegisterState(new AIChasePlayerState());
        stateMachine.RegisterState(new AIIdleState());
        stateMachine.RegisterState(new AIFindWeaponState());
        stateMachine.RegisterState(new AIAttackPlayerState());
        stateMachine.RegisterState(new AILookAroundState());
        stateMachine.RegisterState(new AICoverState());
        stateMachine.ChangeState(initalState);
    }

    public virtual void Update()
    {
        stateMachine.Update();
    }
}
