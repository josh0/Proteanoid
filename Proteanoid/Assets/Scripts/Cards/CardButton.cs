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
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Color redCardColor;
    [SerializeField] private Color greenCardColor;
    [SerializeField] private Color blueCardColor;

    private Image image;

    private void Awake()
    {
        behaviour = GetComponent<CardButtonBehaviour>();
        image = GetComponent<Image>();
    }

    public void SetTargetTransform(Transform t)
    {
        behaviour.SetTargetTransform(t);
    }
    public void OnClick()
    {
        if (Player.mana >= heldCard.manaCost)
        {
            if (Player.instance.PlayCard(heldCard))
                SetHeldCard(null);
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

        switch (card.type)
        {
            case Card.CardType.red:
                image.color = redCardColor;
                break;
            case Card.CardType.green:
                image.color = greenCardColor;
                break;
            case Card.CardType.blue:
                image.color = blueCardColor;
                break;
        }
    }

    public void UpdateCardText()
    {
        nameText.text = heldCard.cardName;
        descText.text = GetDescriptionText();
        costText.text = heldCard.manaCost.ToString();
    }

    private string GetDescriptionText()
    {
        return heldCard.cardDescription.Replace("[damage]", heldCard.damage.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
