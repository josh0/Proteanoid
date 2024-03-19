using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CardButtonBehaviour))]
[RequireComponent(typeof(Button))]
public class CardButton : MonoBehaviour, IPointerDownHandler
{
    public Card heldCard;
    public CardButtonBehaviour behaviour { get; private set; }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private List<ActionDescription> actionDescriptions;
    [SerializeField] private Image retainIcon;
    [SerializeField] private Image innateIcon;
    [SerializeField] private Image exhaustIcon;
    [SerializeField] private Image fleetingIcon;

    private Button button;

    private void Awake()
    {
        behaviour = GetComponent<CardButtonBehaviour>();
        button = GetComponent<Button>();
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

        UpdateKeywordIcons();
    }

    private void UpdateKeywordIcons()
    {
        retainIcon.gameObject.SetActive(heldCard.keywords.Contains(Card.Keywords.retain));
        innateIcon.gameObject.SetActive(heldCard.keywords.Contains(Card.Keywords.innate));
        exhaustIcon.gameObject.SetActive(heldCard.keywords.Contains(Card.Keywords.exhaust));
        fleetingIcon.gameObject.SetActive(heldCard.keywords.Contains(Card.Keywords.fleeting));
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
