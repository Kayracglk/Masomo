using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;


public class AnimalMovement : MonoBehaviour, IHear
{
    [HideInInspector] public Vector3 randomTarget = Vector3.zero;
    [HideInInspector] public NavMeshHit hit;
    [HideInInspector] public AnimalManager animal;

    [Header("Movement")]
    public float stoppingDistance = 0.5f;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI CHANCES")]
    public float feedChance = 10; // Yuzdesel olarak beslenme sansi
    public float runChance = 20;
    public float sleepChance = 30;


    public virtual void Awake()
    {
        animal = GetComponent<AnimalManager>();
    }
    protected virtual void Start()
    {
        randomTarget = GetRandomPosition();
    }

    public virtual void HandleRandomMovement()
    {
        if (!animal.isPerformingAction && animal.navMeshAgent.remainingDistance <= stoppingDistance)
        {   
            int randomMovement = UnityEngine.Random.Range(0, 100);
            if (randomMovement < feedChance)
            {
                animal.animalState = AnimalState.feed;
                animal.animator.SetTrigger("Feed");
                animal.isPerformingAction = true;
            }
            else if (randomMovement < feedChance)
            {
                RunNextTarget();
            }
            else if (randomMovement < sleepChance)
            {
                HandleSleeping();
            }
            else
            {
                WalkNextTarget();
            }
        }
    }

    public virtual void HandleSleeping()
    {
        animal.animalState = AnimalState.sleep;
        animal.animator.SetBool("Rest", true);
        animal.animator.SetTrigger("GoToRest");
        animal.isPerformingAction = true;
        StartCoroutine(SleepTimer());
    }
    public IEnumerator SleepTimer()
    {
        yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        animal.animator.SetBool("Rest", false);
        animal.isPerformingAction = false;
    }
    public void RunAwayTheBullet(Vector3 bulletPos)
    {
        animal.animalState = AnimalState.run;
        HandleAnimatorMovementParams(0, 2);
        Vector3 targetVector = new Vector3(-bulletPos.x, 0, -bulletPos.z);
        animal.navMeshAgent.speed = runSpeed;
        animal.navMeshAgent.SetDestination(transform.position - targetVector);
        
    }
    public void HandleAnimatorMovementParams(float horizontal, float vertical)
    {
        animal.animator.SetFloat("Horizontal", horizontal);
        animal.animator.SetFloat("Vertical", vertical);
    }
    public void RunNextTarget()
    {
        animal.animalState = AnimalState.run;
        animal.isPerformingAction = false;
        animal.navMeshAgent.speed = runSpeed;
        HandleAnimatorMovementParams(0, 2);
        randomTarget = GetRandomPosition();
        animal.navMeshAgent.SetDestination(randomTarget);
    }
    public void WalkNextTarget()
    {
        animal.animalState = AnimalState.walk;
        animal.navMeshAgent.speed = walkSpeed;
        animal.isPerformingAction = false;
        randomTarget = GetRandomPosition();
        HandleAnimatorMovementParams(0, 1);
        animal.navMeshAgent.SetDestination(randomTarget);
    }

    public float ReverseClamp(float value, float min, float max)
    {
        if (min < value && value < max)
            return value - min > max - value ? max : min;

        return value;
    }

    public Vector3 GetRandomPosition()
    {
        float randomX = RandomRangeTest(-30f, 30f, -10, 10); // X koordinatını rastgele seç
        float randomZ = RandomRangeTest(-30f, 30f, -10, 10); // Z koordinatını rastgele seç

        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMesh.SamplePosition(randomPoint, out hit, 1.0f, animal.navMeshArea); // Geçerli bir hedef noktası mı kontrol et

        return hit.position;
    }

    public float RandomRangeTest(float firstRange, float secondRange, float min, float max)
    {
        return ReverseClamp(Random.Range(firstRange, secondRange), min, max);
    }
    public virtual void RespondToSound(Sound sound)
    {
        animal.damaged = true;
        StartCoroutine(animal.healthManager.RunAwayUpdate(sound.pos));
    }
}
