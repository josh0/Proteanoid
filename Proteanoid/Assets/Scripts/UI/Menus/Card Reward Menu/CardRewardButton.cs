using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Adds a card to the player's deck, then closes the Card Reward Menu. <br />
/// Should only be used in the Card Reward Menu.
/// </summary>
[RequireComponent(typeof(CardDescriptionCreator))]
[RequireComponent(typeof(Button))]
public class CardRewardButton : MonoBehaviour, IPointerDownHandler
{
    private CardDescriptionCreator cardDescriptionCreator;
    private Button button;

    public Card heldCard;

    private void Awake()
    {
        cardDescriptionCreator = GetComponent<CardDescriptionCreator>();
        button = GetComponent<Button>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            OnClick();
    }

    private void OnClick()
    {
        Player.instance.AddCardToDeck(heldCard);
        CardRewardMenu.Instance.CloseMenu();
    }
}
