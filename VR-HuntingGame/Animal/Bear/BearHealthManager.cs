using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BearHealthManager : HealthManager
{
    public Image damageimage;

    private void Awake()
    {
        damageimage = GameObject.FindGameObjectWithTag("DamageImage").GetComponent<Image>();
    }

    public override void GiveDamage(float damage, Vector3 hitPosition)
    {
        if (!animal.damaged)
        {
            animal.navMeshAgent.speed = 0;
            animal.animator.SetTrigger("Roar");
            SoundManager.instance.PlaySoundEffects(animal.audioSource, AudioTypes.SingleBear);

            animal.damaged = true;
        }

        if (health - damage > 0)
        {
            health -= damage;
            // Start flashing damage image
            // animal.animator.SetTrigger(hitAnimationName);
            StartCoroutine(RunAndAttackToPlayer());
        }
        else if (!isDeath)
        {
            isDeath = true;
            health = 0;
            animal.navMeshAgent.speed = 0;
            animal.navMeshAgent.isStopped = true;
            animal.animator.SetTrigger(deadAnimationName);
            StartCoroutine(DeadAnimationWaitEvent());
            StopCoroutine(FlashDamageImage());
        }
    }
    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.5f);
        BearLevelManager.instance.SpawnController();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public override IEnumerator DeadAnimationWaitEvent()
    {
        StartCoroutine(Wait());
        yield return null;
    }
    private IEnumerator FlashDamageImage()
    {
        while (!isDeath)
        {
            damageimage.color = new Color(1f, 0f, 0f, 0.5f);
            yield return new WaitForSeconds(0.2f);
            damageimage.color = Color.clear;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator RunAndAttackToPlayer()
    {
        if (!isDeath)
        {
            yield return new WaitForSeconds(2f);
            animal.navMeshAgent.speed = 5;
            animal.animalState = AnimalState.run;
            animal.animator.SetBool("Run", true);
            animal.navMeshAgent.SetDestination(BearLevelManager.instance.player.transform.position);
            yield return null;
            while (!isDeath)
            {
                if (!animal.isPerformingAction && animal.navMeshAgent.remainingDistance <= animal.animalMovement.stoppingDistance)
                {
                    print(animal.navMeshAgent.remainingDistance);
                    animal.navMeshAgent.speed = 0;
                    animal.animator.SetTrigger("Attack");
                    StartCoroutine(FlashDamageImage());
                    break;
                }
                yield return null;
            }
        }
    }
}
