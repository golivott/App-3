using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance { get; private set; }

    public List<Card> deck;
    private float nextDrawTime;
    private float nextDungeonRageTime;

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
    }

    private void Start()
    {
        deck = Deck.LoadDeck().GetDeckAsList();
        ShuffleDeck();

        // Check if any priority cards are in the deck
        List<CardType> priorityCards = new List<CardType>
        {
            CardType.SuitUp, 
            CardType.SpeedRunner, 
            CardType.SilentRunner,
            CardType.FuzzyBunnySlippers
        };
        foreach (CardType priorityCard in priorityCards)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i].cardType == priorityCard)
                {
                    Card drawnCard = deck[i];
                    deck.RemoveAt(i);
                    drawnCard.Play();
                }
            }
        }
        
        nextDrawTime = Time.fixedTime + 10f;
        nextDungeonRageTime = Time.fixedTime + DungeonManager.Instance.dungeonRageInterval;
    }

    private void Update()
    {
        if (Time.fixedTime >= nextDrawTime)
        {
            DrawCard();
            nextDrawTime = Time.fixedTime + DungeonManager.Instance.cardDrawInterval;
        }
        
        if (Time.fixedTime >= nextDungeonRageTime)
        {
            AddDungeonRageCard(1);
            nextDungeonRageTime = Time.fixedTime + DungeonManager.Instance.dungeonRageInterval;
        }
    }

    public void ShuffleDeck()
    {
        System.Random rng = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (deck[k], deck[n]) = (deck[n], deck[k]);
        }
    }

    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            Card drawnCard = deck[0];
            deck.RemoveAt(0);
            this.Invoke(() =>
            {
                Player.Instance.ShowCard(drawnCard);
                drawnCard.Play();
            }, 5f);
        }
    }
    
    public void ChangeInterval(float newInterval)
    {
        CancelInvoke("DrawCard");
        DungeonManager.Instance.cardDrawInterval = newInterval;
        InvokeRepeating("DrawCard", 0f, DungeonManager.Instance.cardDrawInterval);
    }

    public void BurnCard()
    {
        if (deck.Count > 0)
        {
            deck.RemoveAt(0);
        }
    }

    public void AddDungeonRageCard(int amount)
    {
        Card rage = Resources.Load<Card>("Cards/DungeonRage");
        for (int i = 0; i < amount; i++)
        {
            deck.Add(rage);
        }
        ShuffleDeck();
    }
}
