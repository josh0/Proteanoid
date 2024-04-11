using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AssimilateItemButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private ItemRewardButton connectedItemRewardButton;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AssimilateConnectedReward);
    }

    private void AssimilateConnectedReward()
    {
        StartCoroutine(AssimilateConnectedRewardRoutine());
    }

    private IEnumerator AssimilateConnectedRewardRoutine()
    {
        ItemRewardsMenu.Instance.CloseMenu();
        CardRewardMenu.Instance.GenerateRewards(connectedItemRewardButton.heldItem.possibleCardRewards);
        CardRewardMenu.Instance.OpenMenu();
        while (CardRewardMenu.Instance.isMenuOpen)
        {
            yield return null;
        }
        ItemRewardsMenu.Instance.OpenMenu();
        ItemRewardsMenu.pendingRewards.Remove(connectedItemRewardButton.heldItem);
    }
}
