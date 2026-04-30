using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    private bool isGrounded = true;
    private Animator animator;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentSpeed = moveSpeed;
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
        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            animator.SetBool("isRunning", !animator.GetBool("isRunning"));
            if(currentSpeed == runSpeed) currentSpeed = moveSpeed;
            else currentSpeed = runSpeed;
        }

        // Jump (press once)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }
        
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
