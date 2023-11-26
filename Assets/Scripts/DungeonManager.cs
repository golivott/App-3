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
    public GameObject spawnPos;
    public bool playerSpawned;
    public GameObject player;
    public string previousScene;
    public List<GameObject> guardians;
    
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
    public float doubleAngerChance;

    [Header("Hazard")] 
    public float hazardBlock;
    public float hazardInterval;
    private float nextHazardTime;
    [Range(0f,1f)]public float hazardChance;
    public static List<Hazard> hazards = new List<Hazard>();
    
    [Header("Treasure")]
    public float currGold;
    public float currGems;
    public float goldQueue;
    public float gemQueue;
    public float treasureSpawnInterval;
    private float nextTreasureTime;
    public GameObject gold;
    public GameObject gem;
    public GameObject key;
    [Range(0f,1f)]public float keyChance;
    public static List<TreasureSpawner> treasureSpawners = new List<TreasureSpawner>();

    [Header("Cards")] 
    public float cardDrawInterval;
    public float dungeonRageInterval;
    public bool recklessCharge;
    public bool nimbleLooting;
    public float sneakStepGems = 0;
    public bool speedRunner;
    public int eyesOnThePrizeBuff;
    public bool gemRain;
    public bool fuzzyBunnySlippers;
    public int deepDiverLevel2;
    public int deepDiverLevel3;

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
        nextTreasureTime = Time.fixedTime + treasureSpawnInterval;
        nextHazardTime = Time.fixedTime + nextHazardTime;
    }

    private void Update()
    {
        if (Time.fixedTime >= nextTreasureTime)
        {
            SpawnTreasure();
            nextTreasureTime = Time.fixedTime + treasureSpawnInterval;
        }
        
        if (Time.fixedTime >= nextHazardTime)
        {
            TriggerHazards();
            nextHazardTime = Time.fixedTime + nextHazardTime;
        }

        if (Player.Instance)
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

        if (fuzzyBunnySlippers && unlockExit) Player.Instance.moveSpeedMulitpier = 1f;
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

            while (deepDiverLevel2 > 0 && scene.name == "Level1")
            {
                GameObject pos = GameObject.Find("Level2Exit");
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(gem, pos.transform.position, gem.transform.rotation);
                }

                deepDiverLevel2--;
            }
            
            while (deepDiverLevel3 > 0 && scene.name == "Level2")
            {
                GameObject pos = GameObject.Find("Level3Exit");
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(gem, pos.transform.position, gem.transform.rotation);
                }

                deepDiverLevel3--;
            }
            
            // Speed Runner Buff
            if (scene.name == "Level3" && speedRunner)
            {
                for (int i = 0; i < 8; i++)
                {
                    Instantiate(gem, exitPos.transform.position, gem.transform.rotation);
                }
            }
        }
        
        guardians = GameObject.FindGameObjectsWithTag("Guardian").ToList();
    }

    private void SpawnSpirit()
    {
        if (spiritCount < maxSpirits)
        {
            Instantiate(spirit, Player.Instance.player.transform.position + (Random.insideUnitSphere * 10f),
                Quaternion.identity);
        }
    }

    public void TriggerHazards()
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

    public void AddAngerBlock(float amount)
    {
        angerBlock += amount;
    }
    
    public void AddHazardBlock(float amount)
    {
        hazardBlock += amount;
    }

    public void AddGold(float amount)
    {
        goldQueue += amount;
    }

    public void AddGems(float amount)
    {
        gemQueue += amount + (gemRain ? amount : 0f); // doubled if gem rain true
    }

    public void AddAnger(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            float rng = doubleAngerChance == 0 ? 1 : Random.value;

            if (angerBlock > 0)
            {
                if(nimbleLooting) AddGold(2);
                angerBlock -= rng < doubleAngerChance ? 2 : 1;
            }
            else
            {
                currAnger += rng < doubleAngerChance ? 2 : 1;
            }
            
            if (currAnger > maxAnger)
            {
                SpawnSpirit();
            }
        }
    }

    public void LootScootBuff() { StartCoroutine(LootScoot()); }
    private IEnumerator LootScoot()
    {
        Player.Instance.moveSpeedMulitpier *= 1.5f;
        yield return new WaitForSecondsRealtime(15f);
        Player.Instance.moveSpeedMulitpier /= 1.5f;
    }
    public void SecondWindBuff() { StartCoroutine(SecondWind()); }
    private IEnumerator SecondWind()
    {
        Player.Instance.moveSpeedMulitpier *= 1.5f;
        Player.Instance.healthRegenMultiplier *= 1.5f;
        yield return new WaitForSecondsRealtime(15f);
        Player.Instance.moveSpeedMulitpier /= 1.5f;
        Player.Instance.healthRegenMultiplier /= 1.5f;
    }
    public void GuardianAngelBuff() { StartCoroutine(GuardianAngel()); }
    private IEnumerator GuardianAngel()
    {
        foreach (GameObject guardian in guardians)
        {
            guardian.GetComponent<GuardianController>().enabled = false;
        }
        yield return new WaitForSecondsRealtime(15f);
        foreach (GameObject guardian in guardians)
        {
            guardian.GetComponent<GuardianController>().enabled = true;
        }
    }
    public void BoundingStridesBuff() { StartCoroutine(BoundingStrides()); }
    private IEnumerator BoundingStrides()
    {
        Player.Instance.jumpMulitpier *= 1.5f;
        yield return new WaitForSecondsRealtime(120f);
        Player.Instance.jumpMulitpier /= 1.5f;
    }
    
    public void RecklessChargeBuff() { StartCoroutine(RecklessCharge()); }
    private IEnumerator RecklessCharge()
    {
        recklessCharge = true;
        yield return new WaitForSecondsRealtime(10f);
        recklessCharge = false;
    }

    public void SprintBuff() { StartCoroutine(Sprint()); }
    private IEnumerator Sprint()
    {
        Player.Instance.moveSpeedMulitpier *= 1.5f;
        yield return new WaitForSecondsRealtime(60f);
        Player.Instance.moveSpeedMulitpier /= 1.5f;
    }
    public void NimbleLootingBuff() {}
    private IEnumerator NimbleLooting()
    {
        nimbleLooting = true;
        if (angerBlock > 0) yield return null;
        nimbleLooting = false;
    }

    public void QuickStepBuff() { StartCoroutine(QuickStep()); }
    private IEnumerator QuickStep()
    {
        Player.Instance.moveSpeedMulitpier *= 1.5f;
        yield return new WaitForSecondsRealtime(15f);
        Player.Instance.moveSpeedMulitpier /= 1.5f;
    }
    public void SneakStepBuff() { if (sneakStepGems < 6) sneakStepGems += 2; }
    public void SpeedRunnerBuff() { StartCoroutine(SpeedRunner()); }
    private IEnumerator SpeedRunner()
    {
        speedRunner = true;
        yield return new WaitForSecondsRealtime(300f);
        speedRunner = false;
    }

    public void GemRainBuff() { StartCoroutine(GemRain()); }

    private IEnumerator GemRain()
    {
        gemRain = true;
        yield return new WaitForSecondsRealtime(cardDrawInterval + 1f); // card draw 1
        yield return new WaitForSecondsRealtime(cardDrawInterval + 1f); // card draw 2
        yield return new WaitForSecondsRealtime(cardDrawInterval + 1f); // card draw 3
        gemRain = false;
    }

    public void SilentRunnerBuff() { StartCoroutine(SilentRunner()); }
    private IEnumerator SilentRunner()
    {
        while (true)
        {
            while(Mathf.Round(Player.Instance.moveSpeedMulitpier * 100f) / 100f > 1f)
            {
                yield return new WaitForSecondsRealtime(14.5f);

                // Check if player still has move speed buff
                if (Mathf.Round(Player.Instance.moveSpeedMulitpier * 100f) / 100f > 1f)
                {
                    // Decrease player's anger by 1 if it's greater than 0
                    if (Random.value < 0.5f)
                    {
                        AddAngerBlock(1);
                    }
                }
            }
            
            yield return null;
        }
    }

    public void FuzzyBunnySlippersBuff() { StartCoroutine(FuzzyBunnySlippers()); }
    
    private IEnumerator FuzzyBunnySlippers()
    {
        fuzzyBunnySlippers = true;
        while (true)
        {
            yield return new WaitForSecondsRealtime(60f * 6f);
            AddAngerBlock(4);
            yield return null;
        }
    }

    public void DeepDiverBuff()
    {
        deepDiverLevel2 += 1;
        deepDiverLevel3 += 1;
    }
}
