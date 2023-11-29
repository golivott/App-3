using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilderController : MonoBehaviour
{
    public static GameObject selectedCard;
    public Vector3 selectedCardStartPos;
    public GameObject emptyCardPrefab;
    public GameObject divider;
    public Deck deck;
    private Vector3 deckStartPos = new Vector3(-13f, 1f, 6f);
    private Vector3 stockpileStartPos = new Vector3(2.5f, 1f, 6f);
    private Vector3 offsets = new Vector3(3.5f, 1f, 5f);

    private void Start()
    {
        deck = Deck.LoadDeck();
        SortCards();
    }

    public void ReturnToMainMenu()
    {
        Deck.SaveDeck();
        SceneManager.LoadScene("MainMenu");
    }

    private void Update()
    {
        if (selectedCard)
        {
            if (Input.GetButtonDown("Interact"))
            {
                divider.GetComponent<Collider>().enabled = false;
                selectedCardStartPos = selectedCard.transform.position;
            }

            if (Input.GetButtonUp("Interact"))
            {
                divider.GetComponent<Collider>().enabled = true;
                bool startedInDeck = selectedCardStartPos.x < 0;
                // its in deck
                if (selectedCard.transform.position.x < 0 && !startedInDeck)
                {
                    AddCardOutputs output = deck.AddCard(selectedCard.GetComponent<CardObject>().card);
                    if (output != AddCardOutputs.Success)
                    {
                        selectedCard.transform.position = selectedCardStartPos;
                    }
                }
                else if(selectedCard.transform.position.x >= 0 && startedInDeck) // its in stockpile
                {
                    deck.RemoveCard(selectedCard.GetComponent<CardObject>().card);
                }
            }
        }
    }

    public void SortCards()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (var card in cards)
        {
            Destroy(card);
        }

        if (deck.deck.Count > 0)
        {
            int cardNum = 0;
            foreach (var card in deck.deck)
            {
                Vector3 spawnPos = deckStartPos;
                spawnPos.x += offsets.x * (cardNum % 4);
                spawnPos.y += offsets.y * cardNum * 0.5f;
                spawnPos.z -= offsets.z * Mathf.Floor(cardNum / 4f);
                for (int i = 0; i < card.Value; i++)
                {
                    spawnPos.y += 2f * i;
                    GameObject cardObj = Instantiate(emptyCardPrefab, spawnPos, emptyCardPrefab.transform.rotation);
                    cardObj.GetComponent<CardObject>().SetCardContent(card.Key, CardCost.Gem, true, true);
                }

                cardNum++;
            }
        }

        if (deck.stockpile.Count > 0)
        {
            int cardNum = 0;
            foreach (var card in deck.stockpile)
            {
                Vector3 spawnPos = stockpileStartPos;
                spawnPos.x += offsets.x * (cardNum % 4);
                spawnPos.y += offsets.y * cardNum;
                spawnPos.z -= offsets.z * Mathf.Floor(cardNum / 4f);
                for (int i = 0; i < card.Value; i++)
                {
                    spawnPos.y += 1f * i;
                    GameObject cardObj = Instantiate(emptyCardPrefab, spawnPos, emptyCardPrefab.transform.rotation);
                    cardObj.GetComponent<CardObject>().SetCardContent(card.Key, CardCost.Gem, true, true);
                }

                cardNum++;
            }
        }
    }
}
