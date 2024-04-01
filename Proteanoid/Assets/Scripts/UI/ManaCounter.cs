using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaCounter : Singleton<ManaCounter>
{
    [SerializeField] private TextMeshProUGUI manaText;
    public void UpdateText()
    {
        manaText.text = Player.mana.ToString();
    }
}
