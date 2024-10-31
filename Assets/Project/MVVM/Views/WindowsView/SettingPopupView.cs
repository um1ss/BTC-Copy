using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingPopupView : AbstractWindowView
{
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _supportButton;

    [SerializeField] private TextMeshProUGUI _userIDText;

    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    private Dictionary<string, Slider> _sliders;
    private Dictionary<string, TextMeshProUGUI> _texts;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
            { AppConstants.SupportButtonName, _supportButton },
        };
        _texts = new()
        {
            { AppConstants.UserIDValueName, _userIDText },
        };
        _sliders = new()
        {
            { AppConstants.SoundValueName, _soundSlider },
            { AppConstants.MusicValueName, _musicSlider },
        };
    }
    public void SubscribeSlider(string toggleName, UnityAction<float> action)
    {
        if (_sliders.TryGetValue(toggleName, out var toggle))
        {
            toggle.onValueChanged.AddListener(action);
        }
    }
    public void SetText(string textName, string value)
    {
        if (_texts.TryGetValue(textName, out var text))
        {
            text.text = value;
        }
    }
    public void SetSliderValue(string sliderName, float value)
    {
        if (_sliders.TryGetValue(sliderName, out var slider))
        {
            slider.value = value;
        }
    }
}
