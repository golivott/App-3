using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritController : MonoBehaviour
{
    public Rigidbody rb;
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    private Vector3 targetPos;
    private Vector3 targetDir;
    private bool reachedTarget;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = Player.Instance.gameObject.transform.position;
        targetDir = (targetPos - transform.position).normalized;
        reachedTarget = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Update targetPos if the ghost has stopped or is too far from the player
        if ((rb.velocity.magnitude < 0.1f || Vector3.Distance(targetPos, transform.position) > 100) && reachedTarget)
        {
            rb.velocity = Vector3.zero;
            targetPos = Player.Instance.gameObject.transform.position;
            targetDir = (targetPos - transform.position).normalized;
            reachedTarget = false;
        }

        // Check if the ghost should start decelerating
        if (!reachedTarget && Vector3.Distance(targetPos, transform.position) < 0.5f)
        {
            reachedTarget = true;
        }

        // Apply force towards the target
        if (reachedTarget)
        {
            rb.AddForce(-rb.velocity.normalized * deceleration * Vector3.Distance(targetPos, transform.position), ForceMode.Force);
        }
        else
        {
            rb.AddForce(targetDir * acceleration, ForceMode.Force);
        }

        // Clamp the velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        // Determine rotate toward player
        Vector3 targetLookDir = Player.Instance.gameObject.transform.position - transform.position;
        float singleStep = 5f * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetLookDir, singleStep, 0.0f);
        Debug.DrawRay(transform.position, newDirection, Color.red);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
