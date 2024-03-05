using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlayArea : Singleton<CardPlayArea>, IPointerEnterHandler, IPointerExitHandler
{
    private bool isMouseOverArea;
    private bool isMouseUp;
    private void OnMouseUp()
    {
        Debug.Log("Mouse up");
    }
    /// <summary>
    /// Waits for the player to release left click over this object, then plays the given card. If the player right clicks, instead de-select the given card.
    /// </summary>
    /// <param name="selectedCard">The card calling this coroutine.</param>
    public IEnumerator WaitForMouseUp(Card selectedCard)
    {
        while(true)
        {
            yield return null;

            //On right click, de-select the card and cancel this coroutine.
            if (Input.GetMouseButtonDown(1)) {
                selectedCard.OnDeselect();
                break;
            }

            //On mouse up over this area, play the selected card.
            if (isMouseOverArea && Input.GetMouseButtonUp(0))
            {
                selectedCard.OnPlay();
                break;
            }
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
