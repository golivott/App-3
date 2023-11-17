using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerGuardianCollision : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<GuardianController>().moveToPlayer();
        }
    }
}
