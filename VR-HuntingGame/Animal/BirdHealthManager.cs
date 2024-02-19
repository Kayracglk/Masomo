using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHealthManager : MonoBehaviour
{
    BirdMovement bird;
    [SerializeField] private string deadAnimationName = string.Empty;
    private AnimalType type = AnimalType.Bird;
    public float health = 1;
    public float deadDelay = 3f;

    private void Awake()
    {
        bird = GetComponent<BirdMovement>();
    }
    public void GiveDamage()
    {
        print("DEYDI");
        health = 0;
        bird.canFly = false;
        bird.animator.SetTrigger(deadAnimationName);
        LevelManager.instance.CheckLevel(type);
        StartCoroutine(DeadAnimationWaitEvent());
    }

    IEnumerator DeadAnimationWaitEvent()
    {
        yield return new WaitForSeconds(deadDelay);
        Destroy(gameObject);
    }
}
