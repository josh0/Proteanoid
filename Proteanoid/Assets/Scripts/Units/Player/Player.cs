using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Player : Unit
{
    public static int mana { get; private set; }
    private int maxMana = 3;
    [SerializeField] private TextMeshProUGUI manaText;

    [SerializeField] private CanvasGroup uiCanvasGroup;

    private Weapon equippedWeapon;

    public List<Card> deck;
    [SerializeField] private List<Card> testCards;
    private bool isTakingTurn;

    public static Player instance;
    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        equippedWeapon = ScriptableObject.CreateInstance<Weapon>();
    }
    private void Start()
    {
        foreach(Card card in testCards)
        {
            AddCardToDeck(card);
        }
    }
    public override IEnumerator TurnRoutine()
    {
        uiCanvasGroup.interactable = true;
        uiCanvasGroup.blocksRaycasts = true;

        isTakingTurn = true;

        CardManager.Instance.DrawNewHand();

        while(isTakingTurn)
        {
            yield return null;
        }

        CardManager.Instance.DiscardHand();

        uiCanvasGroup.interactable = false;
        uiCanvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        FightManager.OnRoundStart += RefillMana;
    }

    private void OnDisable()
    {
        FightManager.OnRoundStart -= RefillMana;
    }

    public void EndTurn() =>
        isTakingTurn = false;

    public void AddCardToDeck(Card card)
    {
        deck.Add(Instantiate(card));
    }

    /// <summary>
    /// Plays a card, reducing mana equal to that card's cost. Should be called AFTER OnSelectCard().</summary>
    /// <param name="cardToPlay">The selected card.</param>
    /// <returns>Whether or not the card was successfully played.</returns>
    public bool PlayCard(Card cardToPlay)
    {
        Card card = equippedWeapon.GetModifiedCard(cardToPlay);
        AddMana(-card.manaCost);
        StartCoroutine(card.OnPlay());
        return true;
    }

    public void AddMana(int amount)
    {
        mana += amount;
        manaText.text = mana.ToString() + "/" + maxMana;
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
