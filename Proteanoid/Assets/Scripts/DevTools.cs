using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    [SerializeReference] private Card testCard;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CardManager.Instance.DrawCard();
        if (Input.GetKeyDown(KeyCode.Q))
            Player.instance.AddMana(5);
    }
}
