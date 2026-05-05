using UnityEngine;
using UnityEngine.Rendering;



public class DroneFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform droneHead;

    
    

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;

    [Header("Movement")]
    private float originalSpeed;
    [SerializeField] private float moveSpeed = 5f;
    [Tooltip("(heightOffset <= explodeDistance <= attackRange)")]
    [SerializeField] private float heightOffset = 5f;
    [SerializeField] private float speedIncreseRatio = 0.2f;

    [Header("Explosion")] [Tooltip("(explodeDistance <= attackRange)")]
    [SerializeField] private float explodeDistance = 1.5f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private GameObject explosionEffect;

    [Header("Attacking")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackSpeedMultiplier = 3f;

    [Header("Obstacle avoid")]
    [SerializeField] private float obstacleCheckDistance = 3f;
    [SerializeField] private LayerMask obstacleMask;

    [Header("Sound FXs")]
    [SerializeField] private AudioClip explosionFX;
    [SerializeField] private AudioClip flyingFX;


    private enum DroneState
    {
        Idle,
        Chasing,
        Avoiding,
        Returning,
        Attacking
    }

    private Vector3 seekingPosition;
    private bool isAttacking = false;
    private DroneState currentState = DroneState.Idle;
    private Vector3 attackPosition;
    private float originalRange;
    private Vector3 startPosition;
    private Vector3 avoidTarget;

    private float avoidOffset = 1.0f;
    private AudioSource flyingSound;
    float obstacleTopY;

    void Start()
    {
        originalSpeed = moveSpeed;
        originalRange = detectionRange;
        startPosition = transform.position;
        seekingPosition = startPosition;
        currentState = DroneState.Idle;

        if (flyingSound == null)
        {
            flyingSound = SoundFXManager.Instance.PlaySoundFXClipInLoop(flyingFX, transform, 0.5f, false);
            flyingSound.transform.SetParent(transform);
            flyingSound.transform.localPosition = Vector3.zero;

        }
    }

    void updateMovingSound()
    {
        if (flyingSound == null) return;
        bool shouldPlay = currentState != DroneState.Idle;


        if (shouldPlay && !flyingSound.isPlaying)
        {
            flyingSound.Play();
        }
        else if (!shouldPlay && flyingSound.isPlaying)
        {
            flyingSound.Stop();
        }
    }

    void Update()
    {
        

        Debug.Log("Current: " + currentState.ToString());

        // Deside what to do
        switch (currentState)
        {
            case DroneState.Avoiding:
                AvoidObstacles();
                break;

            case DroneState.Chasing:
                seekingPosition = player.transform.position;
                FollowPlayer();
                break;

            case DroneState.Returning:
                ReturnToStart();
                break;

            case DroneState.Attacking:
                attack();
                break;

        }

        determineState();
        updateMovingSound();
    }

    private void attack()
    {

        float distanceToTarget = Vector3.Distance(droneHead.position, attackPosition);
        // Explosion check
        if (distanceToTarget <= explodeDistance)
        {
            Explode();
        }

        moveSpeed = originalSpeed * attackSpeedMultiplier;
        Vector3 targetPos = attackPosition;


        // Move
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Rotate
        Vector3 dir = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 5f
        );
    }

    void Explode()
    {

        // Spawn visual effect
        GameObject effect = Instantiate(explosionEffect, droneHead.position, Quaternion.identity);
        Destroy(effect, 2f);

        // spawn sound FX
        SoundFXManager.Instance.PlaySoundFXClip_1Time(explosionFX, transform, 0.5f);

        // Affect nearby objects
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.AddExplosionForce(explosionForce, droneHead.position, explosionRadius);
            }

            // You can damage player here
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player hit!");
            }
        }

        Debug.Log("Drone exploded!");

        // Destroy drone
        Destroy(gameObject);
    }

 

    void determineState()
    {

        Vector3 flatForward = transform.forward;
        flatForward.y = 0f;
        flatForward.Normalize();

        RaycastHit hit;
        bool obstacleAhead = Physics.Raycast(
            droneHead.position,
            flatForward,
            out hit,
            obstacleCheckDistance,
            obstacleMask
        );

        float distance = Vector3.Distance(droneHead.position, player.position);
        if(currentState == DroneState.Idle && distance > detectionRange) // No reaction, stay calm
        {
            // Stay Idle
        }
        else if (currentState != DroneState.Attacking && obstacleAhead || (currentState == DroneState.Avoiding && droneHead.position.y < (obstacleTopY + avoidOffset))) // avoid obstacle
        {
            // get its height
            if (currentState != DroneState.Avoiding)
            {
                obstacleTopY = hit.collider.bounds.max.y;
                currentState = DroneState.Avoiding;
            }
            Debug.Log("Obstacle ahead!");
        }
        else if (distance <= attackRange)
        { // attack player 
            if (currentState != DroneState.Attacking) // first time attack triggers
            {
                float predictionTime = 0.8f;
                Vector3 playerVelocity = player.GetComponent<Rigidbody>() != null
                    ? player.GetComponent<Rigidbody>().linearVelocity
                    : Vector3.zero;

                attackPosition = player.position + playerVelocity * predictionTime;
                attackPosition.y = 0f;
                seekingPosition = attackPosition;
                currentState = DroneState.Attacking;

            }

            Debug.Log("Attacking!");
        }
        else if (distance <= detectionRange)
        {  // chase the player
            // It will follow the player for some time
            if (currentState != DroneState.Chasing)
            {
                // chase
                detectionRange += 5;
                currentState = DroneState.Chasing;
            }
            else
            {
                moveSpeed += (moveSpeed * speedIncreseRatio * Time.deltaTime);
                moveSpeed = Mathf.Min(moveSpeed, 2 * originalSpeed);
                
            }
            seekingPosition = player.position;
        }
        else if(transform.position != startPosition)
        {
            if(currentState != DroneState.Returning)
            {
                moveSpeed = originalSpeed;
                detectionRange = originalRange;
                seekingPosition = startPosition;
                currentState = DroneState.Returning;
            }
        } else
        {
            currentState = DroneState.Idle;
        }

    }
    void AvoidObstacles()
    {
        
        Debug.Log("Going up");
        // Move strictly in GLOBAL Y (ignores rotation)
        transform.position += Vector3.up * ( moveSpeed * Time.deltaTime);
    }



    void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(droneHead.position, player.position);
        Vector3 targetPos = player.position + Vector3.up * heightOffset;

        // Move
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // Rotate
        Vector3 dir = targetPos - transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 5f
        );

    }

    void ReturnToStart()
    {
        // Move towards the starting point
        transform.position = Vector3.MoveTowards(
            transform.position,
            startPosition,
            moveSpeed * Time.deltaTime
        );

        // Rotate
        Vector3 dir = seekingPosition - transform.position;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 5f
        );

        // Reset rotation if it returned to its start point
        if (startPosition == transform.position)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
}