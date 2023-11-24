using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGold : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DungeonManager.Instance.currGold += 1;
            Destroy(transform.parent.gameObject);
        }
    }
}
