using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButton("Interact"))
        {
            transform.parent.GetComponent<ArtifactController>().Pickup();
        }
    }
}
