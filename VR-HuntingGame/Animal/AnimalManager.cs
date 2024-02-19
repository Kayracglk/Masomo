using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalManager : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public AnimalMovement animalMovement;
    [HideInInspector] public HealthManager healthManager;
    [HideInInspector] public AudioSource audioSource;
    
    public AnimalType type;
    public AnimalState animalState = new AnimalState();
    public int navMeshArea = 1;
    public bool isPerformingAction = false;
    public bool damaged = false;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animalMovement = GetComponent<AnimalMovement>();
        healthManager = GetComponent<HealthManager>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {
        HandleAllMovement();
    }

    public virtual void HandleAllMovement()
    {
        if(!damaged)
        {
            animalMovement.HandleRandomMovement();
        }
    }
}
