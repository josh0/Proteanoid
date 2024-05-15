using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardsMenu : Menu<ItemRewardsMenu>
{
    [HideInInspector] public static List<Item> pendingRewards = new();
    [SerializeField] private Button skipButton;
    [SerializeField] private List<ItemRewardButton> rewardButtons;

    private void Start()
    {
        skipButton.onClick.AddListener(ClearRewards);
    }

    private void ClearRewards()
    {
        pendingRewards.Clear();
        UpdateRewardButtons();
        CloseMenu();
    }

    private void UpdateRewardButtons()
    {
        for(int i=0; i<rewardButtons.Count; i++)
        {
            if (pendingRewards.Count > i)
                rewardButtons[i].SetHeldItem(pendingRewards[i]);
            else
                rewardButtons[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator GetRewardsRoutine()
    {
        OpenMenu();
        while(pendingRewards.Count > 0)
        {
            UpdateRewardButtons();
            yield return null;
        }
        CloseMenu();
    }
}
