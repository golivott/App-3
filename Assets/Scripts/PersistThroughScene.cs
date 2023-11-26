using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistThroughScene : MonoBehaviour
{
    public Vector3 startPos;
    public string spawnScene;
    public MeshRenderer[] renderers = { };
    
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        startPos = transform.position;
        spawnScene = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject);
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }
        else if (spawnScene == scene.name)
        {
            transform.position = startPos;
            MeshRenderer thisR = GetComponent<MeshRenderer>();
            if (thisR) thisR.enabled = true;
            if (renderers.Length > 0)
            {
                foreach (MeshRenderer r in renderers)
                {
                    r.enabled = true;
                }
            }
        }
        else
        {
            transform.position = startPos + Vector3.down * 10000f;
            MeshRenderer thisR = GetComponent<MeshRenderer>();
            if (thisR) thisR.enabled = false;
            if (renderers.Length > 0)
            {
                foreach (MeshRenderer r in renderers)
                {
                    r.enabled = false;
                }
            }
        }
    }
}
