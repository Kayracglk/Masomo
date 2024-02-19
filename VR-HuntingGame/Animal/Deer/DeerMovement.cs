using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class DeerMovement : AnimalMovement
{
    public override void HandleSleeping()
    {
        base.HandleSleeping();
        SoundManager.instance.PlaySoundEffects(animal.audioSource, AudioTypes.Deer2);
    }
}
