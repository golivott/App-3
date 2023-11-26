
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DeckWithList")]
[System.Serializable]
public class DeckWithList : ScriptableObject
{
    public int maxCards;
    public List<CardCount> deck;
    public List<CardCount> stockpile;

    [System.Serializable]
    public struct CardCount
    {
        public Card card;
        public int count;
    }
}