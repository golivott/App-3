using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardCost
{
    Infer,
    Gem,
    Gold
}

public class CardObject : MonoBehaviour
{
    public GameObject title;
    public GameObject description;
    public GameObject cost;
    public GameObject gemImage;
    public GameObject goldImage;
    public GameObject costBG;
    public Card card;
    
    public bool isHovering = false;
    public bool isInteractable = false;
    public bool canClickToMove = false;
    private Vector3 groundPos;
    private BoardController board;

    private void Start()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            board = hit.collider.GetComponent<BoardController>();
            if (board) break;
        }
    }

    private void OnMouseOver()
    {
        isHovering = true;
    }
    
    private void OnMouseExit()
    {
        isHovering = false;
    }


    private void OnMouseDown()
    {
        if (canClickToMove)
        {
            DeckBuilderController.selectedCard = gameObject;
        }
    }

    private void OnMouseDrag()
    {
        if (canClickToMove)
        {
            Vector3 newWorldPosition = new Vector3(board.currentMousePos.x, 0.1f + 1f, board.currentMousePos.z);
            Vector3 difference = newWorldPosition - transform.position;
            GetComponent<Rigidbody>().velocity = 10f * difference;
        }
    }

    private void FixedUpdate()
    {
        Ray groundRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundRay, out RaycastHit groundRayHit, 50f))
        {
            groundPos = groundRayHit.point;
        }
        
        if (!canClickToMove)
        {
            if (transform.position.y < groundPos.y + 1f) isInteractable = true;
        
            if (isInteractable)
            {
                if (isHovering)
                {
                    GetComponent<Rigidbody>().useGravity = false;
                    transform.position = new Vector3(transform.position.x, groundPos.y + 1f, transform.position.z);
                    if (!CardSelectionBox.cards.Contains(gameObject))
                    {
                        CardSelectionBox.cards.Add(gameObject);
                    }
                }
                else
                {
                    GetComponent<Rigidbody>().useGravity = true;
                    CardSelectionBox.cards.Remove(gameObject);
                }
            }
        }
    }

    public void SetCardContent(Card card, CardCost costType = CardCost.Infer, bool hideCost = false, bool canClickToMove = false)
    {
        this.canClickToMove = canClickToMove;
        this.card = card;
        title.name = card.cardName;
        description.name = card.description;
        if (costType == CardCost.Infer)
        {
            costType = card.cardRarity == CardRarity.Normal ? CardCost.Gold : CardCost.Gem;
        }

        if (costType == CardCost.Gem)
        {
            cost.GetComponent<TextMeshPro>().text = card.gemCost.ToString();
            gemImage.SetActive(true);
            goldImage.SetActive(false);
        }
        else
        {
            cost.GetComponent<TextMeshPro>().text = card.goldCost.ToString();
            gemImage.SetActive(false);
            goldImage.SetActive(true);
        }

        if (hideCost)
        {
            costBG.SetActive(false);
        }

        LanguageController.UpdateTextLanguage();
    }
}
