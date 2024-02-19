using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearMovement : AnimalMovement
{
    public override void HandleRandomMovement()
    {
        if (!animal.isPerformingAction && animal.navMeshAgent.remainingDistance <= stoppingDistance)
        {
            SoundManager.instance.PlaySoundEffects(animal.audioSource, AudioTypes.SingleBear);
            animal.animalState = AnimalState.walk;
            animal.navMeshAgent.speed = walkSpeed;
            animal.isPerformingAction = false;
            randomTarget = GetRandomPosition();
            animal.navMeshAgent.SetDestination(randomTarget);
        }
    }

    public override void RespondToSound(Sound sound)
    {
        
    }
}
