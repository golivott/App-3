using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSpawn : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
