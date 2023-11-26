using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGem : MonoBehaviour
{
    public AudioSource spawnSound;

    private void Start()
    {
        spawnSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DungeonManager.Instance.currGems += 1;
            Destroy(transform.parent.gameObject);
        }
    }
}
