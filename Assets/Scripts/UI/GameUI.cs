using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        UpdateHPSlider(1);//HPSlider를 꽉 채워준다.
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.value = percentage;//이 percentage는 currentHealth/MaxHP로 설정된다.
    }

    public void UpdateWaveText(int wave)
    {
        waveText.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}