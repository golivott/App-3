using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector]public float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float sneakSpeed;
    public bool isSneaking;
    public bool canSprint;
    public bool isSprinting;
    public float stamina;
    public float staminaRegenRate;
    public float staminaConsumeRate;
    public bool canRegenStamina;
    private float fov;
    private Coroutine regenCoroutine;
    private Coroutine sprintCoroutine;
    
    [Header("Slopes")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    [Header("Drag")]
    public float groundDrag;
    public float airDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        moveSpeed = 0;
        canJump = true;
        isSneaking = false;
        isSprinting = false;
        canSprint = true;
        canRegenStamina = true;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        GetInput();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
        
        // Stamina regen tick
        if (stamina < 100f && !isSprinting && canSprint && canRegenStamina)
        {
            stamina += staminaRegenRate * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0, 100f);
        }
            
        // Stamina consume
        if (isSprinting)
        {
            canRegenStamina = false;
            stamina -= staminaConsumeRate * Time.deltaTime;
            if (stamina <= 0)
            {
                canSprint = false;
                if (sprintCoroutine != null)
                {
                    StopCoroutine(sprintCoroutine);
                }
                sprintCoroutine = StartCoroutine(EnableSprintAfterDelay(2f));
            }
            stamina = Mathf.Clamp(stamina, 0, 100f);
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
            }
            regenCoroutine = StartCoroutine(EnableRegenAfterDelay(2f));
        }
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (playerHeight * 0.5f + 0.3f));
    }
    
    IEnumerator EnableSprintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canSprint = true;
    }

    IEnumerator EnableRegenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canRegenStamina = true;
    }
    
    private void GetInput()
    {
        if (Player.Instance.health > 0)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            
            // Set player isSneaking for sneak zones
            if (Input.GetButton("Sneak")) Player.Instance.isSneaking = true;
            else Player.Instance.isSneaking = false;

            // Set movement is sneaking
            if (Input.GetButton("Sneak") && grounded) isSneaking = true;
            else isSneaking = false;

            if (Input.GetButton("Sprint") && !isSneaking && canSprint) isSprinting = true;
            else isSprinting = false;
        
            // when to jump
            if(Input.GetButton("Jump") && canJump && grounded)
            {
                canJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void MovePlayer()
    {
        if (isSneaking) moveSpeed = sneakSpeed;
        else if (isSprinting) moveSpeed = sprintSpeed;
        else moveSpeed = walkSpeed;

        moveSpeed *= Player.Instance.moveSpeedMulitpier;
        
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope 
        if (OnSlope())
        {
            // counter gravity to prevent sliding
            Vector3 gravity = Vector3.ProjectOnPlane(-Physics.gravity, slopeHit.normal);
            rb.AddForce(gravity, ForceMode.Force);
        }
        
        // on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * Player.Instance.jumpMulitpier, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        canJump = true;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

}