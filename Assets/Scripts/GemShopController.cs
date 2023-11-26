using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemShopController : MonoBehaviour
{
    public float currGems;
    public GameObject emptyCardPrefab;
    public List<Card> goodCards;

    public Deck deck;
    
    public TextMeshProUGUI gemCount;

    private Vector3[] cardSpawns = new[]
    {
        new Vector3(-7f, 9, 2.5f),
        new Vector3(-3.5f, 9, 2.5f),
        new Vector3(-0f, 9, 2.5f),
        new Vector3(3.5f, 9, 2.5f),
        new Vector3(7f, 9, 2.5f),
        new Vector3(-0f, 9, -2.5f),
        new Vector3(-3.5f, 9, -2.5f),
        new Vector3(3.5f, 9, -2.5f),
    };
    private void Start()
    {
        deck = Deck.LoadDeck();

        currGems = DungeonManager.Instance ? DungeonManager.Instance.currGems : 0f;
        
        List<Card> cards = Resources.LoadAll<Card>("Cards").ToList();
        goodCards = cards.FindAll((card) => card.cardRarity == CardRarity.Good);

        int cardsToSpawn = 5 + (DungeonManager.Instance ? DungeonManager.Instance.eyesOnThePrizeBuff : 0);
        for (int i = 0; i < cardsToSpawn; i++)
        {
            int rng = Random.Range(0, goodCards.Count);
            GameObject card = Instantiate(emptyCardPrefab, cardSpawns[i], emptyCardPrefab.transform.rotation);
            card.GetComponent<CardObject>().SetCardContent(goodCards[rng]);
        }
    }

    private void Update()
    {
        gemCount.text = currGems.ToString();

        if (Input.GetButtonDown("Interact") )
        {
            List<GameObject> cardsSelected = CardSelectionBox.cards;
            Card selectedCard = cardsSelected.Count > 0 ? cardsSelected[^1].GetComponent<CardObject>().card : null;
            
            if (selectedCard && currGems >= selectedCard.gemCost)
            {
                currGems -= selectedCard.gemCost;
                if (deck.stockpile.ContainsKey(selectedCard))
                {
                    deck.stockpile[selectedCard] += 1;
                }
                else
                {
                    deck.stockpile[selectedCard] = 1;
                }

                GameObject lastObject = cardsSelected[^1];
                CardSelectionBox.cards.Remove(cardsSelected[^1]);
                Destroy(lastObject);
            }
        }
    }
    public void ReturnToMainMenu()
    {
        float currGold = PlayerPrefs.GetFloat("Gold", 0f);
        PlayerPrefs.SetFloat("Gold", currGold + DungeonManager.Instance.currGold);
        PlayerPrefs.Save();
        Deck.SaveDeck();

        GameObject dungeonManager = GameObject.FindGameObjectWithTag("DungeonManager");
        if (dungeonManager != null)
        {
            while (dungeonManager.transform.parent != null)
            {
                dungeonManager = dungeonManager.transform.parent.gameObject;
            }
            Destroy(dungeonManager);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
