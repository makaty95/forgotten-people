using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;

    [Header("Foot steps")]
    [SerializeField] AudioClip walkClip;
    [SerializeField] AudioClip runClip;
    [SerializeField] float walkStepRate = 0.5f;
    [SerializeField] float runStepRate = 0.3f;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask obstacleMask;

float stepTimer;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Animator animator;
    private float currentSpeed;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Auto movement
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // Movement (hold key)
        if (Keyboard.current.dKey.isPressed)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f); // rotate right
        }

        if (Keyboard.current.aKey.isPressed)
        {
            transform.Rotate(0f, -rotationSpeed * Time.deltaTime, 0f); // rotate left
        }

        // toggle run animation (Just for testing)
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            animator.SetBool("isRunning", true);
            isRunning = true;
            currentSpeed = runSpeed;

        }
        else if(Keyboard.current.wKey.wasReleasedThisFrame)
        {
            animator.SetBool("isRunning", false);
            isRunning = false;
            currentSpeed = walkSpeed;
        }

        // Jump (press once)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        HandleFootsteps();
        
    }

    bool IsGrounded()
    {
        LayerMask environmentMask = groundMask | obstacleMask;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance, environmentMask);
    }


    // foot steps
    void HandleFootsteps()
    {
        if (!IsGrounded()) return;

        stepTimer -= Time.deltaTime;

        float rate = isRunning ? runStepRate : walkStepRate;

        if (stepTimer <= 0f)
        {
            PlayFootstep(isRunning);
            stepTimer = rate;
        }
    }

    void PlayFootstep(bool running)
    {
        AudioClip clip = running ? runClip : walkClip;

        SoundFXManager.Instance.PlaySoundFXClip_1Time(
            clip,
            transform,
            0.5f
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }
}
