using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CardButtonBehaviour))]
public class CardButton : MonoBehaviour, IPointerClickHandler
{
    public Card heldCard;
    private CardButtonBehaviour behaviour;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private List<ActionDescription> actionDescriptions;

    private void Awake()
    {
        behaviour = GetComponent<CardButtonBehaviour>();
    }

    public void SetTargetTransform(Transform t)
    {
        behaviour.SetTargetTransform(t);
    }
    public void OnClick()
    {
        if (Player.mana >= heldCard.manaCost)
        {
            StartCoroutine(heldCard.OnSelect());
        }
        else
            Debug.Log("Can't play " + heldCard.name + " because you don't have enough mana.");
    }

    public void UpdateInteractability()
    {

    }
    public void SetHeldCard(Card card)
    {
        heldCard = card;

        if (card == null)
        {
            behaviour.DisableTargetTransform();
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        UpdateCardText();
    }

    public void UpdateCardText()
    {
        nameText.text = heldCard.cardName;
        UpdateDescription();
        costText.text = heldCard.manaCost.ToString();
    }
    private void UpdateDescription()
    {
        for (int descIndex = 0; descIndex < actionDescriptions.Count; descIndex++)
        {
            if (heldCard.actions.Count >= descIndex + 1)
                actionDescriptions[descIndex].SetDescription(heldCard.actions[descIndex]);
            else
                actionDescriptions[descIndex].gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
