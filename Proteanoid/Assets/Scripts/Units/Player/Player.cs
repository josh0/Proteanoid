using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Unit
{
    public static int mana { get; private set; }
    private int maxMana = 3;
    [SerializeField] private TextMeshProUGUI manaText;

    public List<Card> deck = new();
    private bool isTakingTurn;

    public static Player instance;
    public override IEnumerator TurnRoutine()
    {
        FightUI.Instance.SetActive(true);

        isTakingTurn = true;

        CardManager.Instance.DrawNewHand();

        while(isTakingTurn)
        {
            yield return null;
        }

        CardManager.Instance.DiscardHand();

        FightUI.Instance.SetActive(false);
    }

    public override void OnStartTurn()
    {
        base.OnStartTurn();
        RefillMana();
    }

    public void EndTurn() =>
        isTakingTurn = false;

    public void AddCardToDeck(Card card)
    {
        Card newCard = Instantiate(card);
        newCard.OnCreate();
        deck.Add(newCard);
    }

    public void AddMana(int amount)
    {
        mana += amount;
        ManaCounter.Instance.UpdateText();
        CardManager.Instance.UpdateCardInteractability();
    }

    public void RefillMana()
    {
        mana = maxMana;
        AddMana(0);
    }

    protected override void Die()
    {
        
    }
}
