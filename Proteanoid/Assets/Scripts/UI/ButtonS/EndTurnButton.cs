using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    private Button button;

    //Start instead of awake to give the Player loader time to create an instance of the player.
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Player.instance.EndTurn);
    }
}
