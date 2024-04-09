using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDescriptionCreator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private List<ActionDescription> actionDescriptions;
    [SerializeField] private Image retainIcon;
    [SerializeField] private Image innateIcon;
    [SerializeField] private Image exhaustIcon;
    [SerializeField] private Image fleetingIcon;
    public void SetDescription(Card card)
    {
        nameText.text = CardNameGenerator.GetName(card.actions);
        costText.text = card.manaCost.ToString();

        for (int descIndex = 0; descIndex < actionDescriptions.Count; descIndex++)
        {
            if (card.actions.Count >= descIndex + 1)
                actionDescriptions[descIndex].SetDescription(card.actions[descIndex]);
            else
                actionDescriptions[descIndex].gameObject.SetActive(false);
        }

        UpdateKeywordIcons(card);
    }

    private void UpdateKeywordIcons(Card card)
    {
        retainIcon.gameObject.SetActive(card.keywords.Contains(Card.Keywords.retain));
        innateIcon.gameObject.SetActive(card.keywords.Contains(Card.Keywords.innate));
        exhaustIcon.gameObject.SetActive(card.keywords.Contains(Card.Keywords.exhaust));
        fleetingIcon.gameObject.SetActive(card.keywords.Contains(Card.Keywords.fleeting));
    }
}
