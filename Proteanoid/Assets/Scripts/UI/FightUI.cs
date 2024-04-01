using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUI : Singleton<FightUI>
{
    private CanvasGroup group;
    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }
    public void SetActive(bool a)
    {
        group.interactable = a;
        group.blocksRaycasts = a;
    }
}
