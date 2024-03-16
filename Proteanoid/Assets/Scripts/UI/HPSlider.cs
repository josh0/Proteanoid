using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HPSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI blockText;
    [SerializeField] private Image blockImage;
    private Slider slider;

    [SerializeField] private List<ActionDescription> effectDescriptions;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetBlockVal(int val)
    {
        if (blockImage == null)
            return;
        blockText.text = val.ToString();
        if (val == 0)
            blockImage.gameObject.SetActive(false);
        else
            blockImage.gameObject.SetActive(true);
    }
    public void SetMaxHPVal(int val)
    {
        slider.maxValue = val;
    }
    public void SetHPVal(int val)
    {
        hpText.text = val.ToString();
        slider.value = val;
    }

    public void SetStatusEffectDescriptions(List<StatusEffect> effects)
    {
        for (int descIndex = 0; descIndex < effectDescriptions.Count; descIndex++)
        {
            if (effects.Count >= descIndex + 1)
                effectDescriptions[descIndex].SetDescription(effects[descIndex]);
            else
                effectDescriptions[descIndex].gameObject.SetActive(false);
        }
    }
}
