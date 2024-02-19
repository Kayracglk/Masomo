using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearManager : AnimalManager
{
    public override void HandleAllMovement()
    {
        if (!damaged)
        {
            animalMovement.HandleRandomMovement();
        }
    }
}
