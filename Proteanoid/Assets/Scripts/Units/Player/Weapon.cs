using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string menuName;
    public int durability { get; private set; }

    [SerializeField] private int manaModifier;
    [SerializeField] private int damageModifier;
    [SerializeField] private int blockModifier;

    /// <summary>Removes 1 durability, then returns the given card with the stat modifiers of the weapon.</summary>
    /// <param name="baseCard">The card the player is trying to play, before being modified by the equipped weapon.</param>
    /// <returns>The card, modified </returns>
    public Card GetModifiedCard(Card baseCard)
    {
        if (durability > 0)
            durability -= 1;
        else 
            return baseCard;

        Card modifiedCard = Instantiate(baseCard);
        modifiedCard.manaCost = Mathf.Max(modifiedCard.manaCost + manaModifier, 0);

        foreach (UnitAction action in modifiedCard.actions)
        {
            action.damageModifier = damageModifier;
            action.blockModifier = blockModifier;
        }

        return modifiedCard;
    }
}
