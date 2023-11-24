using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectKey : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DungeonManager.Instance.lowestLevelUnlocked = DungeonManager.Instance.currLevel + 1;
            Destroy(transform.parent.gameObject);
        }
    }
}
