using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlayArea : Singleton<CardPlayArea>, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private bool isMouseOverArea;
    /// <summary>
    /// Waits for the player to release left click over this object, then plays the given card. If the player right clicks, instead de-select the given card.
    /// </summary>
    /// <param name="selectedCard">The card calling this coroutine.</param>
    public IEnumerator WaitForMouseUp(Card selectedCard, CardButtonBehaviour button)
    {
        while(true)
        {
            button.targetPos = Camera.main.ScreenToWorldPoint(new(Input.mousePosition.x, Input.mousePosition.y, 20));

            //On right click, de-select the card and cancel this coroutine.
            if (Input.GetMouseButtonDown(1)) {
                selectedCard.OnDeselect();
                break;
            }

            //On mouse up over this area, play the selected card.
            if (isMouseOverArea && Input.GetMouseButtonUp(0))
            {
                CardManager.Instance.PlayCard(selectedCard);
                break;
            }
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverArea = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverArea = false;
    }
}
