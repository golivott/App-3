using System;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [HideInInspector]public float moveSpeed;
    public float runSpeed;
    public float walkSpeed;
    bool isWalking;
    
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
    bool grounded;

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
        isWalking = false;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, groundLayer);

        GetInput();
        //SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = airDrag;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * (playerHeight * 0.5f + 0.3f));
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Walk") && grounded) isWalking = true;
        else isWalking = false;
        
        // when to jump
        if(Input.GetButton("Jump") && canJump && grounded)
        {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        if (isWalking) moveSpeed = walkSpeed;
        else moveSpeed = runSpeed;

        moveSpeed = moveSpeed * Player.Instance.moveSpeedMulitpier;
        
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