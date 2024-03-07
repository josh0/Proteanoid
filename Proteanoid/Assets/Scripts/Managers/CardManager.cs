using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : Singleton<CardManager>
{
    [SerializeField] private List<CardButton> handButtons;
    [SerializeField] private List<Transform> handButtonTargets;

    private List<Card> drawPile = new();
    [SerializeField] private Transform drawPileTransform;
    private List<Card> discardPile = new();
    [SerializeField] private Transform discardPileTransform;
    private List<Card> hand = new();

    [SerializeField] private Animator handAnimator;
    private void Start()
    {
        drawPile.AddRange(Player.instance.deck);
        ShuffleList(drawPile);
        UpdateHandButtons();
    }

    public void DrawNewHand()
    {
        const int cardsToDraw = 5;
        StartCoroutine(DrawMultipleCardsRoutine(cardsToDraw));
    }

    public void SetCardsInteractable(bool interactable)
    {
        handAnimator.SetBool("isDisplayed", interactable);
    }

    public void UpdateCardInteractability()
    {
        foreach(CardButton button in handButtons)
        {
            if (button.heldCard.manaCost >= Player.mana)
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
        for(int i=hand.Count-1; i >= 0; i--)
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

        if (SetAvailableHandButtonAs(drawPile[^1]))
        {
            hand.Add(drawPile[^1]);
            drawPile.Remove(drawPile[^1]);
        }
        else
            Debug.Log("There is no room in your hand.");

        UpdateHandButtons();
    }

    public void AddCardToDiscardPile(Card card)
    {
        discardPile.Add(card);
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
                button.SetTargetTransform(handButtonTargets[i].transform);

                handButtonTargets[i].gameObject.SetActive(true);
                return true;
            }
            i++;
        }
        return false;
    }

    public void DiscardCard(Card card)
    {
        hand.Remove(card);
        discardPile.Add(card);
        
        UpdateHandButtons();
    }

    private void ShuffleDiscardPileIntoDrawPile()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        ShuffleList(drawPile);
    }

    public void UpdateHandButtons()
    {
        for (int i = 0; i < handButtons.Count; i++)
        {
            if (hand.Count >= i + 1)
                handButtons[i].SetHeldCard(hand[i]);
            else
                handButtons[i].SetHeldCard(null);
        }
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
