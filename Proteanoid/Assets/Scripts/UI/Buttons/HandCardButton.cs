using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CardButtonBehaviour))]
[RequireComponent(typeof(CardDescriptionCreator))]
[RequireComponent(typeof(Button))]
public class HandCardButton : MonoBehaviour, IPointerDownHandler
{
    public Card heldCard;
    public CardButtonBehaviour behaviour { get; private set; }

    public CardDescriptionCreator descriptionCreator { get; private set; }

    private Button button;

    private void Awake()
    {
        behaviour = GetComponent<CardButtonBehaviour>();
        button = GetComponent<Button>();
        descriptionCreator = GetComponent<CardDescriptionCreator>();
    }

    public void SetTargetTransform(Transform t)
    {
        behaviour.SetTargetTransform(t);
    }
    public void OnClick()
    {
        if (Player.mana >= heldCard.manaCost)
        {
            StartCoroutine(heldCard.OnSelect(behaviour));
            CardManager.Instance.SetHeldCardButton(this);
        }
        else
            Debug.Log("Can't play " + heldCard.name + " because you don't have enough mana.");
    }
    public void SetHeldCard(Card card)
    {
        heldCard = card;

        if (card == null)
        {
            behaviour.SetTargetTransformActive(false);
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        card.SetCardButton(this);
        descriptionCreator.SetDescription(card);
    }

    public void SetInteractable(bool b)
    {
        button.interactable = b;
    }

    //This can't be done through button.onClick because it needs to activate when the mouse button goes down, not when it goes down and back up again.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.interactable)
            OnClick();
    }
}
