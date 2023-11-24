using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorCloseHazard : Hazard
{
    public Vector3 startPos;
    public GameObject closePos;
    public float speed;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (triggered)
        {
            transform.position = Vector3.Lerp(transform.position, closePos.transform.position, Time.deltaTime * speed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * speed);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = false;
        }
    }

    public override void Trigger()
    {
        triggered = true;
    }
}
