using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemRewardButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    public Item heldItem { get; private set; }
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetHeldItem(Item item)
    {
        gameObject.SetActive(true);
        icon.sprite = item.icon;
        nameText.text = item.partName;
        heldItem = item;
    }

    private void OnClick()
    {
        Player.inventory.Add(heldItem);
        ItemRewardsMenu.pendingRewards.Remove(heldItem);
    }
}
