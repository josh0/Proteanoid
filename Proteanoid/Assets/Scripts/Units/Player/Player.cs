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
    [SerializeField] private Animator sigilCircleAnimator;

    private Weapon equippedWeapon = ScriptableObject.CreateInstance<Weapon>();

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
    /// Plays a card, reducing mana equal to that card's cost.</summary>
    /// <param name="card"></param>
    /// <returns>Whether or not the card was successfully played.</returns>
    public bool PlayCard(Card cardToPlay)
    {
        Card card = equippedWeapon.GetModifiedCard(cardToPlay);
        AddMana(-card.manaCost);
        return true;
    }
    private List<Unit> GetTargetsFromActionTargetType(UnitAction.TargetType type)
    {
        switch (type)
        {
            case UnitAction.TargetType.randomEnemy:
            case UnitAction.TargetType.enemy:
            case UnitAction.TargetType.allEnemies:
                return FightManager.enemies.OfType<Unit>().ToList<Unit>();

            case UnitAction.TargetType.self:
                return new List<Unit> { this };
            default:
                Debug.LogWarning("Unknown Target Type: " + type);
                return new List<Unit> { };
        }
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
