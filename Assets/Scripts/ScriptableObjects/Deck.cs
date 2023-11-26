using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

public enum AddCardOutputs
{
    DeckAtMaxSize,
    TooManyOfCard,
    Success,
}

[CreateAssetMenu(menuName = "Deck")]
public class Deck : ScriptableObject
{
    public int maxCards;
    public SerializedDictionary<Card,int> deck;
    public SerializedDictionary<Card,int> stockpile;

    public AddCardOutputs AddCard(Card card)
    {
        // Check if deck is too big
        int cardsInDeck = 0;
        foreach (int n in deck.Values)
        {
            cardsInDeck += n;
        }
        if (cardsInDeck == maxCards) return AddCardOutputs.DeckAtMaxSize;
        
        // Check if deck has too many of that card
        if (deck.ContainsKey(card))
        {
            int numOfThisCard = deck[card];
            if (numOfThisCard == card.maxInDeck) return AddCardOutputs.TooManyOfCard;
        }

        // Add card
        if (deck.TryGetValue(card, out int num))
        {
            deck[card] = num + 1;
        }
        else
        {
            deck[card] = 1;
        }
        stockpile[card] -= 1;
        return AddCardOutputs.Success;
    }
    
    public void RemoveCard(Card card)
    {
        deck.TryGetValue(card, out int num);
        deck[card] = num - 1;
        StoreCard(card);
        if (deck[card] == 0) deck.Remove(card);
    }

    public void StoreCard(Card card, int n = 1)
    {
        if (n == 0)
        {
            stockpile[card] = n;
            return;
        }
        
        if (stockpile.TryGetValue(card, out int num))
        {
            stockpile[card] = num + n;
        }
        else
        {
            stockpile[card] = n;
        }
    }

    public List<Card> GetDeckAsList()
    {
        List<Card> cards = new List<Card>();

        foreach (KeyValuePair<Card, int> entry in deck)
        {
            for (int i = 0; i < entry.Value; i++)
            {
                cards.Add(entry.Key);
            }
        }
        
        return cards;
    }
    
    public static List<DeckWithList.CardCount> ConvertDictionaryToList(SerializedDictionary<Card,int> dictionary)
    {
        List<DeckWithList.CardCount> list = new List<DeckWithList.CardCount>();

        foreach (KeyValuePair<Card, int> entry in dictionary)
        {
            DeckWithList.CardCount cardCount;
            cardCount.card = entry.Key;
            cardCount.count = entry.Value;
            list.Add(cardCount);
        }

        return list;
    }
    
    public static SerializedDictionary<Card,int> ConvertListToDictionary(List<DeckWithList.CardCount> list)
    {
        SerializedDictionary<Card,int> dictionary = new SerializedDictionary<Card,int>();

        foreach (DeckWithList.CardCount cardCount in list)
        {
            dictionary[cardCount.card] = cardCount.count;
        }

        return dictionary;
    }

    public static void SaveDeck()
    {
        Deck deck = Resources.Load<Deck>("Deck/Deck");
        DeckWithList deckWithList = Resources.Load<DeckWithList>("DeckWithList");
        deckWithList.maxCards = deck.maxCards;
        deckWithList.deck = ConvertDictionaryToList(deck.deck);
        deckWithList.stockpile = ConvertDictionaryToList(deck.stockpile);
        
        string path = System.IO.Path.Combine(Application.persistentDataPath, "deck.json");
        string deckJson = JsonUtility.ToJson(deckWithList);
        Debug.Log("Saving:\n" + deckJson + "\nat:\n" + path);
        System.IO.File.WriteAllText(path,deckJson);
    }

    public static Deck LoadDeck()
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "deck.json");
        try
        {
            string deckJson = System.IO.File.ReadAllText(path);
            Debug.Log("Loading:\n" + deckJson + "\nat:\n" + path);
            DeckWithList savedDeck = Resources.Load<DeckWithList>("DeckWithList");
            JsonUtility.FromJsonOverwrite(deckJson, savedDeck);
            Deck deck = Resources.Load<Deck>("Deck/Deck");
            deck.maxCards = savedDeck.maxCards;
            deck.deck = ConvertListToDictionary(savedDeck.deck);
            deck.stockpile = ConvertListToDictionary(savedDeck.stockpile);
            return deck;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return null;
    }
}


