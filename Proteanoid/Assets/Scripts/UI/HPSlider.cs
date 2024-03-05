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
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    public void SetBlockVal(int val)
    {
        blockText.text = val.ToString();
        if (val == 0)
            blockImage.gameObject.SetActive(false);
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
}
