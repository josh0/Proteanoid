using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private List<HandCardButton> handButtons;
    [SerializeField] private List<Transform> handButtonTargets;

    [SerializeField] private List<Card> testCards;

    private List<Card> drawPile = new();
    [SerializeField] private Transform drawPileTransform;
    private List<Card> discardPile = new();
    [SerializeField] private Transform discardPileTransform;
    private List<Card> hand = new();

    [SerializeField] private CanvasGroup handCanvasGroup;

    public HandCardButton heldCardButton { get; private set; }
    private void Start()
    {
        foreach (Card c in testCards)
        {
            Card newCard = Instantiate(c);
            newCard.OnCreate();
            Player.instance.AddCardToDeck(newCard);
        }
        UpdateHandButtonTargets();
        SetNullCardsInactive();
    }

    /// <summary>
    /// Clears the hand, draw pile, and discard pile, and puts all cards from the player's deck into the draw pile.
    /// </summary>
    public void ResetCards()
    {
        hand.Clear();
        foreach (HandCardButton button in handButtons)
            button.SetHeldCard(null);

        drawPile.Clear();
        discardPile.Clear();

        drawPile.AddRange(Player.instance.deck);
        ShuffleList(drawPile);
    }

    private void UpdateHandButtonTargets()
    {
        for(int i=0; i<handButtons.Count; i++)
        {
            handButtons[i].SetTargetTransform(handButtonTargets[i]);
        }
    }
    /// <summary>
    /// Plays a card, reducing mana equal to that card's cost. Should be called AFTER OnSelectCard().</summary>
    /// <param name="cardToPlay">The selected card.</param>
    /// <returns>Whether or not the card was successfully played.</returns>
    public bool PlayCard(Card cardToPlay)
    {
        if (cardToPlay.manaCost > Player.mana)
            return false;
        Player.instance.AddMana(-cardToPlay.manaCost);
        StartCoroutine(cardToPlay.OnPlay());
        return true;
    }
    public void DrawInnateCards()
    {
        foreach (Card card in new List<Card>(drawPile))
        {
            if (card.keywords.Contains(Card.Keywords.innate))
                DrawCard(card);
        }
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
        foreach(HandCardButton button in handButtons)
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
        foreach (HandCardButton button in handButtons)
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
            Debug.Log("Your hand is full. Adding card to discard pile.");
            AddCardToDiscardPile(card);
        }
    }

    private bool SetAvailableHandButtonAs(Card card)
    {
        int i = 0;
        foreach (HandCardButton button in handButtons)
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
        foreach (HandCardButton button in handButtons)
            if (button.heldCard == card)
            {
                button.SetHeldCard(null);
                hand.Remove(card);
                SetNullCardsInactive();
                return;
            }
        Debug.LogWarning("Tried to remove " + card.name + " from hand, but it wasn't there.");
    }

    private void ShuffleDiscardPileIntoDrawPile()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(drawPile);
    }

    public void SetHeldCardButton(HandCardButton b)
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
