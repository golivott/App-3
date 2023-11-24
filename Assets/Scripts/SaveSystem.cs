using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    
    Dictionary<string, SaveableObject> saveableObjects = new Dictionary<string, SaveableObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SaveScene()
    {
        foreach (var saveableObject in FindObjectsOfType<SaveableObject>())
        {
            saveableObject.SaveState();
            saveableObjects[saveableObject.id] = saveableObject;
        }
    }

    public void LoadScene()
    {
        foreach (var saveableObject in FindObjectsOfType<SaveableObject>())
        {
            if (saveableObjects.TryGetValue(saveableObject.id, out var savedObject))
            {
                saveableObject.position = savedObject.position;
                saveableObject.triggered = savedObject.triggered;
                saveableObject.LoadState();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadScene();
    }
}
