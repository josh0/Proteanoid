using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private List<CardButton> handButtons;
    [SerializeField] private List<Transform> handButtonTargets;

    [SerializeField] private List<Card> drawPile = new();
    [SerializeField] private Transform drawPileTransform;
    [SerializeField] private List<Card> discardPile = new();
    [SerializeField] private Transform discardPileTransform;
    [SerializeField] private List<Card> hand = new();

    [SerializeField] private CanvasGroup handCanvasGroup;

    public CardButton heldCardButton { get; private set; }
    private void Start()
    {
        drawPile.AddRange(Player.instance.deck);
        foreach (Card card in drawPile)
            card.OnCreate();
        ShuffleList(drawPile);
        UpdateHandButtonTargets();
        SetNullCardsInactive();
    }

    private void OnEnable()
    {
        FightManager.OnFightStart += DrawInnateCards;
    }

    private void OnDisable()
    {
        FightManager.OnFightStart -= DrawInnateCards;
    }

    private void UpdateHandButtonTargets()
    {
        for(int i=0; i<handButtons.Count; i++)
        {
            handButtons[i].SetTargetTransform(handButtonTargets[i]);
        }
    }

    private void DrawInnateCards()
    {
        foreach (Card card in new List<Card>(drawPile))
            if (card.keywords.Contains(Card.Keywords.innate))
                DrawCard(card);
    }

    public void DrawNewHand()
    {
        const int cardsToDraw = 5;
        StartCoroutine(DrawMultipleCardsRoutine(cardsToDraw));
    }

    /// <summary>
    /// Sets the hand's animator's "isDisplayed" to a given bool. <br />
    /// If the given bool is true, also set the heldCardButton to null.
    /// </summary>
    public void SetCardsInteractable(bool interactable)
    {
        handCanvasGroup.interactable = interactable;
        handCanvasGroup.blocksRaycasts = interactable;
        if (interactable)
            SetHeldCardButton(null);
    }

    public void UpdateCardInteractability()
    {
        foreach(CardButton button in handButtons)
        {
            if (button.heldCard != null && button.heldCard.manaCost > Player.mana)
                button.SetInteractable(false);
            else
                button.SetInteractable(true);
        }
    }

    //Draws a card a given number of times with a delay between each draw for visual clarity.
    private IEnumerator DrawMultipleCardsRoutine(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void DiscardHand()
    {
        for (int i = hand.Count - 1; i >= 0; i--)
            if (!hand[i].keywords.Contains(Card.Keywords.retain))
                DiscardCard(hand[i]);
    }

    public void DrawCard()
    {
        if (drawPile.Count == 0)
            ShuffleDiscardPileIntoDrawPile();

        if (drawPile.Count == 0)
        {
            Debug.Log("You have no more cards.");
            return;
        }

        DrawCard(drawPile[^1]);
    }

    public void DrawCard(Card card)
    {
        if (SetAvailableHandButtonAs(card))
        {
            hand.Add(card);
            drawPile.Remove(card);
        }
        else
            Debug.Log("There is no room in your hand.");
    }
    
    private void SetNullCardsInactive()
    {
        foreach (CardButton button in handButtons)
            if (button.heldCard == null)
            {
                button.behaviour.SetTargetTransformActive(false);
                button.gameObject.SetActive(false);
            }
    }

    public void AddCardToDiscardPile(Card card)
    {
        discardPile.Add(card);
    }

    public void AddCardToHand(Card card)
    {
        if (SetAvailableHandButtonAs(card))
            hand.Add(card);
        else
        {
            Debug.Log("Your hand is full. Adding " + card.cardName + " to discard pile.");
            AddCardToDiscardPile(card);
        }
    }

    private bool SetAvailableHandButtonAs(Card card)
    {
        int i = 0;
        foreach (CardButton button in handButtons)
        {
            if (button.heldCard == null)
            {
                button.gameObject.SetActive(true);
                button.SetHeldCard(card);

                button.transform.position = drawPileTransform.position;
                button.behaviour.ResetSiblingIndex();

                handButtonTargets[i].gameObject.SetActive(true);
                return true;
            }
            i++;
        }
        return false;
    }

    /// <summary>
    /// Removes a card from the player's hand and adds it to the discard pile.
    /// </summary>
    /// <param name="card">The card to be discarded</param>
    public void DiscardCard(Card card)
    {
        RemoveCardFromHand(card);
        discardPile.Add(card);
    }

    /// <summary>
    /// Removes a card from the player's hand (Will stay in the player's deck.)
    /// </summary>
    /// <param name="card">The card to exhaust.</param>
    public void ExhaustCard(Card card)
    {
        RemoveCardFromHand(card);
    }

    private void RemoveCardFromHand(Card card)
    {
        foreach (CardButton button in handButtons)
            if (button.heldCard == card)
            {
                button.SetHeldCard(null);
                hand.Remove(card);
                SetNullCardsInactive();
                return;
            }
        Debug.LogWarning("Tried to remove " + card.cardName + " from hand, but it wasn't there.");
    }

    private void ShuffleDiscardPileIntoDrawPile()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(drawPile);
    }

    public void SetHeldCardButton(CardButton b)
    {
        heldCardButton = b;
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
