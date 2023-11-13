using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance { get; private set; }

    [Header("Anger")]
    public float currAnger;
    public float maxAnger;

    [Header("Treasure")]
    public float currGold;
    public float currGems;
    public float goldQueue;
    public float gemQueue;
    public float treasureSpawnInterval;
    private List<GameObject> treasureSpawns;

    [Header("Game Speed")] 
    public float cardDrawInterval;
    private float lastTick;
    private float ticks;
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
        treasureSpawns = GameObject.FindGameObjectsWithTag("TreasureSpawner").ToList();
    }

    private void Update()
    {
        CalculateTick();
        
        if(ticks>=cardDrawInterval) DrawCard();
        if(ticks>=treasureSpawnInterval) SpawnTreasure();
    }
    
    // Increment ticks every 1 second
    private void CalculateTick()
    {
        if (lastTick - Time.deltaTime > 1f)
        {
            lastTick = Time.deltaTime;
            ticks++;
        }
    }
    
    private void SpawnSpirit() {}
    private void SpawnTreasure() {}
    public void DrawCard() {}
    public void BurnCard() {}
    private void ShuffleDeck() {}
 }
