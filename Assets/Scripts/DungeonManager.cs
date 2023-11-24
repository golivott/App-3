using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    public GameObject spirit;
    public float maxSpirits;
    public float spiritCount;

    [Header("Gamestate")] 
    public bool unlockExit;
    public int lowestLevelUnlocked;
    public int currLevel;
    public static int difficulty;
    private GameObject spawnPos;
    private bool playerSpawned;
    public GameObject player;
    public string previousScene;
    
    [Header("Artifact")]
    public Vector3 artifactLocation;
    public bool artifactSpawned;
    public GameObject artifactController;
    private GameObject artifact;
    
    [Header("Anger")]
    public float currAnger;
    public float angerBlock;
    public float maxAnger;
    public static List<AngerGenerator> angerGenerators = new List<AngerGenerator>();
    
    [Header("Hazard")] 
    public float hazardBlock;
    public float hazardInterval;
    [Range(0f,1f)]public float hazardChance;
    public static List<Hazard> hazards = new List<Hazard>();
    
    [Header("Treasure")]
    public float currGold;
    public float currGems;
    public float goldQueue;
    public float gemQueue;
    public float treasureSpawnInterval;
    public GameObject gold;
    public GameObject gem;
    public GameObject key;
    [Range(0f,1f)]public float keyChance;
    public static List<TreasureSpawner> treasureSpawners = new List<TreasureSpawner>();

    [Header("Cards")] 
    public float cardDrawInterval;
    
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currAnger = 0;
        currGold = 0;
        currGems = 0;
        currLevel = 1;
        lowestLevelUnlocked = 1;
        InvokeRepeating("SpawnTreasure", treasureSpawnInterval, treasureSpawnInterval);
        InvokeRepeating("TriggerHazards", hazardInterval, hazardInterval);
    }

    private void Update()
    {
        // Compass direction
        if (unlockExit)
        {
            switch (currLevel)
            {
                case 1:
                    Player.Instance.compassTarget = FindObjectOfType<ExitDoor>().transform.position;
                    break;
                case 2:
                    Player.Instance.compassTarget = GameObject.Find("Level1Entrance").transform.position;
                    break;
                case 3:
                    Player.Instance.compassTarget = GameObject.Find("Level2Entrance").transform.position;
                    break;
            }
        }
        else
        {
            switch (currLevel)
            {
                case 1:
                    Player.Instance.compassTarget = GameObject.Find("Level2Entrance").transform.position;
                    break;
                case 2:
                    Player.Instance.compassTarget = GameObject.Find("Level3Entrance").transform.position;
                    break;
            }

            if (currLevel == difficulty) Player.Instance.compassTarget = artifactLocation;
        }
        
        // Handle artifact spawning 
        if (!artifactSpawned && currLevel == difficulty)
        {
            artifact = Instantiate(artifactController);
            artifactSpawned = true;
        }

        if (!unlockExit) // artifact gets nuked when the exit is unlocked
        {
            if (artifactSpawned && currLevel == difficulty)
            {
                artifact.SetActive(true);
            }

            if (artifactSpawned && currLevel != difficulty)
            {
                artifact.SetActive(false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SaveSystem.Instance.LoadScene();
        if (!playerSpawned)
        {
            spawnPos = GameObject.Find("SpawnPos");
            if (spawnPos)
            {
                Instantiate(player, spawnPos.transform.position, spawnPos.transform.rotation);
                playerSpawned = true;
            }
        }
        else
        {
            print("Teleporting to " + previousScene + "Exit");
            GameObject exitPos = GameObject.Find(previousScene + "Exit");
            if (exitPos)
            {
                print(exitPos.transform.position);
                Player.Instance.player.transform.position = exitPos.transform.position;
                Player.Instance.gameObject.GetComponentInChildren<PlayerCam>().yRotation = exitPos.transform.eulerAngles.y;
            }
        }
    }

    private void SpawnSpirit()
    {
        if (spiritCount < maxSpirits)
        {
            Instantiate(spirit, Player.Instance.player.transform.position + (Random.insideUnitSphere * 10f),
                Quaternion.identity);
        }
    }

    private void TriggerHazards()
    {
        Hazard hazard = hazards[Random.Range(0, hazards.Count)];
        
        float rng = Random.value;
        if (rng < hazardChance)
        {
            if (hazardBlock > 0)
            {
                hazardBlock--;
            }
            else
            {
                hazard.Trigger();
            }
        }
    }

    private void SpawnTreasure()
    {
        if (gemQueue > 0)
        {
            TreasureSpawner spawner = treasureSpawners[Random.Range(0, treasureSpawners.Count)];
            spawner.SpawnTreasure(gem);
            gemQueue--;
        }
        
        if (goldQueue > 0)
        {
            // Spawn gold
            TreasureSpawner spawner = treasureSpawners[Random.Range(0, treasureSpawners.Count)];
            spawner.SpawnTreasure(gold);
            if(Random.value < keyChance && currLevel == lowestLevelUnlocked) spawner.SpawnTreasure(key);
            goldQueue--;
        }
    }

    public void AddAnger(float amount)
    {
        if (angerBlock > amount)
        {
            angerBlock -= amount;
        }
        else if (angerBlock > 0)
        {
            float angerToAdd = angerBlock - amount;
            angerBlock = 0;
            currAnger += angerToAdd;
        }
        else
        {
            currAnger += amount;
        }

        for (int i = 0; i < amount; i++)
        {
            if (currAnger > maxAnger)
            {
                SpawnSpirit();
            }
        }
    }
 }
