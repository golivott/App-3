using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GoldShopController : MonoBehaviour
{
    public float currGold;
    public GameObject emptyCardPrefab;
    public List<Card> normalCards;
    public List<Card> goodCards;

    public Deck deck;
    
    public TextMeshProUGUI goldCount;

    private Vector3[] normalCardSpawns = new[]
    {
        new Vector3(-7.5f, 9, 0),
        new Vector3(-4f, 9, 0),
        new Vector3(-0.5f, 9, 0),
        new Vector3(3f, 9, 0),
    };

    private Vector3 goodCardSpawn = new Vector3(7.5f, 9,0);
    private GameObject goodCard;
    private void Start()
    {
        deck = Deck.LoadDeck();
        
        currGold = PlayerPrefs.GetFloat("Gold", 0);
        
        List<Card> cards = Resources.LoadAll<Card>("Cards").ToList();
        normalCards = cards.FindAll((card) => card.cardRarity == CardRarity.Normal);
        goodCards = cards.FindAll((card) => card.cardRarity == CardRarity.Good);

        for (int i = 0; i < 4; i++)
        {
            GameObject card = Instantiate(emptyCardPrefab, normalCardSpawns[i], emptyCardPrefab.transform.rotation);
            card.GetComponent<CardObject>().SetCardContent(normalCards[i]);
        }

        int rng = PlayerPrefs.GetInt("GoodCard", 0);
        goodCard = Instantiate(emptyCardPrefab, goodCardSpawn, emptyCardPrefab.transform.rotation);
        goodCard.GetComponent<CardObject>().SetCardContent(goodCards[rng], CardCost.Gold);
    }

    public void RerollGood()
    {
        if (currGold >= 5)
        {
            currGold -= 5;
            Destroy(goodCard);
            int rng = Random.Range(0, goodCards.Count);
            PlayerPrefs.SetInt("GoodCard", rng);
            PlayerPrefs.Save();
            goodCard = Instantiate(emptyCardPrefab, goodCardSpawn, emptyCardPrefab.transform.rotation);
            goodCard.GetComponent<CardObject>().SetCardContent(goodCards[rng], CardCost.Gold);
        }
    }

    private void Update()
    {
        goldCount.text = currGold.ToString();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currGold += 50;
        }
        
        if (Input.GetButtonDown("Interact") )
        {
            List<GameObject> cardsSelected = CardSelectionBox.cards;
            Card selectedCard = cardsSelected.Count > 0 ? cardsSelected[^1].GetComponent<CardObject>().card : null;
            
            if (selectedCard && currGold >= selectedCard.goldCost )
            {
                int numCards = deck.stockpile.ContainsKey(selectedCard) ? deck.stockpile[selectedCard] : 0;

                if (numCards < selectedCard.maxInDeck)
                {
                    currGold -= selectedCard.goldCost;
                    if (deck.stockpile.ContainsKey(selectedCard))
                    {
                        deck.stockpile[selectedCard] += 1;
                    }
                    else
                    {
                        deck.stockpile[selectedCard] = 1;
                    }

                    if (selectedCard.cardRarity == CardRarity.Normal)
                    {
                        int i = normalCards.FindIndex((card) => card == selectedCard);
                        GameObject card = Instantiate(emptyCardPrefab, normalCardSpawns[i], emptyCardPrefab.transform.rotation);
                        card.GetComponent<CardObject>().SetCardContent(normalCards[i]);
                    }

                    if (selectedCard.cardRarity == CardRarity.Good)
                    {
                        int rng = Random.Range(0, goodCards.Count);
                        PlayerPrefs.SetInt("GoodCard", rng);
                        PlayerPrefs.Save();
                        goodCard = Instantiate(emptyCardPrefab, goodCardSpawn, emptyCardPrefab.transform.rotation);
                        goodCard.GetComponent<CardObject>().SetCardContent(goodCards[rng], CardCost.Gold);
                    }

                    GameObject lastObject = cardsSelected[^1];
                    CardSelectionBox.cards.Remove(cardsSelected[^1]);
                    Destroy(lastObject);
                }
            }
        }
    }
    public void ReturnToMainMenu()
    {
        PlayerPrefs.SetFloat("Gold", currGold);
        PlayerPrefs.Save();
        Deck.SaveDeck();
        SceneManager.LoadScene("MainMenu");
    }
}
