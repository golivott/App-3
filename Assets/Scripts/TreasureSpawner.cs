using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreasureSpawner : MonoBehaviour
{
    // Add this spawner to the dungeon manager
    private void OnEnable() => DungeonManager.treasureSpawners.Add(this);
    private void OnDisable() => DungeonManager.treasureSpawners.Remove(this);

    public void SpawnTreasure(GameObject treasurePrefab)
    {
        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        randomOffset.y = 0;
        GameObject treasure = Instantiate(treasurePrefab, transform.position + randomOffset, treasurePrefab.transform.rotation);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
    #endif
}
