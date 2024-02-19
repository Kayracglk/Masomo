using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour, IHear
{
    [SerializeField] Vector2 radiusMinMax;
    [SerializeField] Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFromTo, changeTargetEveryFromTo;
    [SerializeField] Vector2 yMinMax;
    [SerializeField] Transform homeTarget, flyingTarget;
    [SerializeField] private bool returnToBase = false;
    [SerializeField] float idleSpeed, turnSpeed, switchSeconds, idleRatio;
    [SerializeField] private float randomBaseOffset = 5, delayStart = 0f;

    private AudioSource audioSource;
    public Animator animator;
    public Rigidbody rb;
    private float changeTarget = 0f;
    private float changeAnim = 0f;
    private float timeSinceAnim = 0f;
    private float currentAnim = 0f;
    private float prevSpeed;
    private float speed;
    private float zturn;
    private float prevz;
    private float turnSpeedBackup;
    private Vector3 rotateTarget, position, direction, randomizedBase;
    private Quaternion lookRotation;
    private float distanceFromBase, distanceFromTarget;
    public bool canFly = false;
    private BirdHealthManager healthManager;
    public float dusmeHizi = 100f;
    public float torque = 0.5f;
    private GameObject player;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        healthManager = GetComponent<BirdHealthManager>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        turnSpeedBackup = turnSpeed;
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);
        if (delayStart < 0f) rb.velocity = idleSpeed * direction;
        homeTarget = player.transform;
        flyingTarget = player.transform;
    }

    void FixedUpdate()
    {
        if (!canFly) 
        {
            Vector3 oldVelocity = rb.velocity;
            // rb.velocity = Vector3.zero;
            //rb.Sleep(); rb.WakeUp();
            // rb.velocity += new Vector3(oldVelocity.x, 0, oldVelocity.z) * Time.deltaTime;
            rb.velocity +=  Vector3.up * Physics.gravity.y * (dusmeHizi -1 ) * Time.deltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(oldVelocity * torque));
            return;
        }
        
        animator.SetBool("CanFly", true);
        if (delayStart > 0f)
        {
            delayStart -= Time.fixedDeltaTime;
            return;
        }

        distanceFromBase = Vector3.Magnitude(randomizedBase - rb.position);
        distanceFromTarget = Vector3.Magnitude(flyingTarget.position - rb.position);

        if (returnToBase && distanceFromBase < 10f)
        {
            if (turnSpeed != 300f && rb.velocity.magnitude != 0f)
            {
                turnSpeedBackup = turnSpeed;
                turnSpeed = 300f;
            }
            else if (distanceFromBase <= 2f)
            {
                rb.velocity = Vector3.zero;
                turnSpeed = turnSpeedBackup;
                return;
            }
        }
        
        if (changeAnim < 0f)
        {
            currentAnim = ChangeAnim(currentAnim);
            changeAnim = Random.Range(changeAnimEveryFromTo.x, changeAnimEveryFromTo.y);
            timeSinceAnim = 0f;
            prevSpeed = speed;
            if (currentAnim == 0) speed = idleSpeed;
            else speed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, (currentAnim - animSpeedMinMax.x) / (animSpeedMinMax.y - animSpeedMinMax.x));
        }
        // Time for a new target position
        if (changeTarget < 0f)
        {   
            // Invoke("randomSesCal",delay);
            
            rotateTarget = ChangeDirection(rb.transform.position);
            if (returnToBase) changeTarget = 0.2f; else changeTarget = Random.Range(changeTargetEveryFromTo.x, changeTargetEveryFromTo.y);
        }

        if (rb.transform.position.y < yMinMax.x + 10f ||
            rb.transform.position.y > yMinMax.y - 10f)
        {
            if (rb.transform.position.y < yMinMax.x + 10f) rotateTarget.y = 1f; else rotateTarget.y = -1f;
        }
        
        zturn = Mathf.Clamp(Vector3.SignedAngle(rotateTarget, direction, Vector3.up), -45f, 45f);

        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;
        timeSinceAnim += Time.fixedDeltaTime;

        if (rotateTarget != Vector3.zero) lookRotation = Quaternion.LookRotation(rotateTarget, Vector3.up);
        Vector3 rotation = Quaternion.RotateTowards(rb.transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime).eulerAngles;
        rb.transform.eulerAngles = rotation;

        float temp = prevz;
        if (prevz < zturn) prevz += Mathf.Min(turnSpeed * Time.fixedDeltaTime, zturn - prevz);
        else if (prevz >= zturn) prevz -= Mathf.Min(turnSpeed * Time.fixedDeltaTime, prevz - zturn);

        prevz = Mathf.Clamp(prevz, -45f, 45f);

        rb.transform.Rotate(0f, 0f, prevz - temp, Space.Self);

        direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        if (returnToBase && distanceFromBase < idleSpeed)
        {
            rb.velocity = Mathf.Min(idleSpeed, distanceFromBase) * direction;
        }
        else rb.velocity = Mathf.Lerp(prevSpeed, speed, Mathf.Clamp(timeSinceAnim / switchSeconds, 0f, 1f)) * direction;

        if (rb.transform.position.y < yMinMax.x || rb.transform.position.y > yMinMax.y)
        {
            position = rb.transform.position;
            position.y = Mathf.Clamp(position.y, yMinMax.x, yMinMax.y);
            rb.transform.position = position;
        }
    }

    private float ChangeAnim(float currentAnim)
    {
        float newState;
        if (Random.Range(0f, 1f) < idleRatio) newState = 0f;
        else
        {
                SoundManager.instance.PlaySoundEffects(audioSource, AudioTypes.BirdWings);
            newState = Random.Range(animSpeedMinMax.x, animSpeedMinMax.y);
        }
        if (newState != currentAnim)
        {
            
            animator.SetFloat("flySpeed", newState);
            if (newState == 0){
                 animator.speed = 1f;
                 } 
            else animator.speed = newState;
        }
        return newState;
    }

    // Select a new direction to fly in randomly
    private Vector3 ChangeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;
        if (returnToBase)
        {
            randomizedBase = homeTarget.position;
            randomizedBase.y += Random.Range(-randomBaseOffset, randomBaseOffset);
            newDir = randomizedBase - currentPosition;
        }
        else if (distanceFromTarget > radiusMinMax.y)
        {
            newDir = flyingTarget.position - currentPosition;
        }
        else if (distanceFromTarget < radiusMinMax.x)
        {
            newDir = currentPosition - flyingTarget.position;
        }
        else
        {
            // 360-degree freedom of choice on the horizontal plane
            float angleXZ = Random.Range(-Mathf.PI, Mathf.PI);
            // Limited max steepness of ascent/descent in the vertical direction
            float angleY = Random.Range(-Mathf.PI / 48f, Mathf.PI / 48f);
            // Calculate direction
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + Mathf.Sin(angleY) * Vector3.up;
        }
        return newDir.normalized;
    }

    public void RespondToSound(Sound sound)
    {
        StartCoroutine(Wait(1));
        if (!canFly && healthManager.health > 0)
            canFly = true;
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
