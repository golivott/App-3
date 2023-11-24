using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtifactController : MonoBehaviour
{
    private Artifact artifact;

    public void Start()
    {
        // Pick a location
        List<GameObject> artifactSpawns = GameObject.FindGameObjectsWithTag("ArtifactSpawn").ToList();
        DungeonManager.Instance.artifactLocation = artifactSpawns[Random.Range(0, artifactSpawns.Count)].transform.position;
        
        // Pick an artifact
        List<Artifact> artifacts = Resources.LoadAll<Artifact>("Artifacts/"+DungeonManager.difficulty).ToList();
        artifact = artifacts[Random.Range(0, artifacts.Count)];

        // Spawn artifact;
        transform.position = DungeonManager.Instance.artifactLocation;
        GameObject model = Instantiate(artifact.model, transform);
        model.transform.position += new Vector3(0,artifact.model.GetComponent<MeshCollider>().bounds.extents.y,0);
        model.GetComponent<MeshCollider>().enabled = false;
        DontDestroyOnLoad(gameObject);
    }

    public void Pickup()
    {
        DungeonManager.Instance.currGems += artifact.gemValue;
        DungeonManager.Instance.AddAnger(3);
        DungeonManager.Instance.unlockExit = true;
        Destroy(gameObject);
    }
}
