using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public AnimalManager animal;
    
    public string hitAnimationName = string.Empty;
    public string deadAnimationName = string.Empty;

    public float health = 100;
    public float deadDelay = 3f;

    public bool isDeath = false;

    private void Awake()
    {
        animal = GetComponent<AnimalManager>();
    }

    public virtual void GiveDamage(float damage, Vector3 hitPosition)
    {
        animal.damaged = true;
        if (health - damage > 0)
        {
            health -= damage;
            animal.animator.SetTrigger(hitAnimationName);
            print(hitPosition);
            StartCoroutine(RunAwayUpdate(hitPosition));
        }
        else if (!isDeath)
        {
            isDeath = true;
            health = 0;
            animal.navMeshAgent.speed = 0;
            animal.animator.SetTrigger(deadAnimationName);
            if (animal.type == AnimalType.Deer)
            {
                SoundManager.instance.PlaySoundEffects(animal.audioSource, AudioTypes.Deer1);
            }
            else if (animal.type == AnimalType.Boar)
            {
                SoundManager.instance.PlaySoundEffects(animal.audioSource, AudioTypes.BoarDead);
            }   
            LevelManager.instance.CheckLevel(animal.type);
            StartCoroutine(DeadAnimationWaitEvent());
        }
    }

    public virtual IEnumerator DeadAnimationWaitEvent()
    {
        print("DeadAnimationWaitEvent");
        yield return new WaitForSeconds(deadDelay);
        Destroy(gameObject);
    }

    public IEnumerator RunAwayUpdate(Vector3 hitPos)
    {
        if (!isDeath)
        {
            float startTime = Time.time;
            float duration = 20;
            animal.animalMovement.RunAwayTheBullet(hitPos);

            while (!isDeath && (Time.time - startTime) < duration)
            {
                if (!animal.isPerformingAction && animal.navMeshAgent.remainingDistance <= animal.animalMovement.stoppingDistance)
                {
                    animal.animalMovement.RunNextTarget();
                }

                yield return null;
            }
            animal.damaged = false;
        }
    }

}
