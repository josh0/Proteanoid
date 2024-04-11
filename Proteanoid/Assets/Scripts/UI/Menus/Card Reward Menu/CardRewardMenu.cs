using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardMenu : Menu<CardRewardMenu>
{
    [SerializeField] private List<CardRewardButton> rewardButtons;
    public bool isWaitingForRewardChoice = false;

    /// <summary>
    /// For each reward button, pick a random card from a given list, then display that card to be selected. <br />
    /// The same reward cannot be displayed twice. If there are no more rewards, only display a limited amount of rewards.
    /// </summary>
    /// <param name="cards">The list of possible cards to choose from.</param>
    public void GenerateRewards(List<Card> cards)
    {
        List<Card> possibleRewards = new(cards);
        foreach(CardRewardButton button in rewardButtons)
        {
            if (possibleRewards.Count > 0)
            {
                button.gameObject.SetActive(true);
                int index = Random.Range(0, possibleRewards.Count);
                Card newCard = Instantiate(possibleRewards[index]);
                newCard.OnCreate();
                button.SetHeldCard(newCard);
                possibleRewards.RemoveAt(index);
            }
            else
                button.gameObject.SetActive(false);
        }
    }
}
