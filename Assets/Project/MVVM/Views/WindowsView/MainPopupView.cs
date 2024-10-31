using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _buttonsParent;

    [Space]
    [SerializeField] private Button _chatButton;
    [SerializeField] private Button _cleaningButton;
    [SerializeField] private Button _collectionsButtons;
    [SerializeField] private Button _missionsButton;
    [SerializeField] private Button _titsGramButton;
    [SerializeField] private Button _slotsButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _levelsButton;
    [SerializeField] private Button _settingsButton;

    [Space]
    [SerializeField] private Button _chipShopButton;
    [SerializeField] private Button _condomShopButton;
    [SerializeField] private Button _energyShopButton;
    [SerializeField] private Button _likeShopButton;
    
    [Space]
    [Header("Indicators")]
    [SerializeField] private GameObject _collectionsIndicator;
    [SerializeField] private GameObject _missionsIndicator;
    [SerializeField] private GameObject _cleaningIndicator;
    [SerializeField] private GameObject _chatsIndicator;

    [Space]
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _chipsText;
    [SerializeField] private TextMeshProUGUI _condomText;
    [SerializeField] private TextMeshProUGUI _likesText;
    [SerializeField] private TextMeshProUGUI _energyText;
    
    [Space]
    [Header("Timers")]
    [SerializeField] private GameObject _cleaningTimer;
    [SerializeField] private TMP_Text _cleaningTimerTxt;
    
    private Dictionary<string, TextMeshProUGUI> _texts;
    private Dictionary<string, GameObject> _indicators;

    private AppRoutines appRoutines;
    private CleaningModel cleaningModel;
    private MissionsModel missionsModel;
    private ChatModel chatModel;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.ChatButtonName, _chatButton },
            { AppConstants.CleaningButtonName, _cleaningButton },
            { AppConstants.CollectionsButtonName, _collectionsButtons },
            { AppConstants.MissionsButtonName, _missionsButton },
            { AppConstants.TitsGramButtonName, _titsGramButton },
            { AppConstants.SlotsButtonName, _slotsButton },
            { AppConstants.ShopButtonName, _shopButton },
            { AppConstants.LevelsButtonName, _levelsButton },
            { AppConstants.SettingsButtonName, _settingsButton },

            { AppConstants.AddChipsButtonName, _chipShopButton },
            { AppConstants.AddCondomsButtonName, _condomShopButton },
            { AppConstants.AddEnergyButtonName, _energyShopButton },
            { AppConstants.AddLikeButtonName, _likeShopButton },

        };

        _texts = new()
        {
            { AppConstants.ChipsTextsName, _chipsText },
            { AppConstants.CondomTextsName, _condomText },
            { AppConstants.LikeTextsName, _likesText },
            { AppConstants.EnergyTextsName, _energyText }
        };
        
        _indicators = new()
        {
            { AppConstants.CollectionsIndicator, _collectionsIndicator },
            { AppConstants.MissionsIndicator, _missionsIndicator },
            { AppConstants.CleaningIndicator, _cleaningIndicator },
            { AppConstants.ChatIndicator, _chatsIndicator },
            { AppConstants.ChatButtonName, _chatButton.gameObject },
        };
    }

    public void SetText(string textName, string value)
    {
        if (_texts.TryGetValue(textName, out var text))
        {
            text.text = IntFormatConverter.FormatInt(value);
        }
    }
    
    public void ShowView()
    {
        _buttonsParent.gameObject.SetActive(true);
    }
    
    public void HideView()
    {
        _buttonsParent.gameObject.SetActive(false);
    }
    
    public void StartRoutines(AppRoutines appRoutines, CleaningModel cleaningModel, MissionsModel missionsModel, ChatModel chatModel) {
        this.appRoutines = appRoutines;
        this.cleaningModel = cleaningModel;
        this.missionsModel = missionsModel;
        this.chatModel = chatModel;
        
        InvokeRepeating(nameof(PeriodicallyUpdate), 0.1f, 0.5f);
    }

    private async UniTask PeriodicallyUpdate() {
        await appRoutines.ViewPeriodicallyUpdate();
        UpdateIndicators();
        UpdateCleaningTimer();
    }
    
    private void UpdateIndicators() {
        UpdateIndicator(AppConstants.CollectionsIndicator, false);
        UpdateIndicator(AppConstants.MissionsIndicator, missionsModel.IsIndicatorAvailable);
        UpdateIndicator(AppConstants.CleaningIndicator, cleaningModel.IsIndicatorAvailable);
        UpdateIndicator(AppConstants.ChatIndicator, chatModel.IsIndicatorAvailable);
        UpdateIndicator(AppConstants.ChatButtonName, chatModel.IsAvailable);
    }
    
    private void UpdateIndicator(string indicatorName, bool isEnabled) {
        if (_indicators.TryGetValue(indicatorName, out var indicator)) {
            if (indicator.activeSelf != isEnabled) {
                indicator.SetActive(isEnabled);
            }
        }
    }
    
    private void UpdateCleaningTimer() {
        if (cleaningModel.TryGetTimerEndValue(out var endTime)) {
            if (!_cleaningTimer.activeSelf) {
                _cleaningTimer.SetActive(true);
            }

            var now = DateTimeOffset.Now;
            _cleaningTimerTxt.text = TimeFormatUtils.GetTimerTextShort(now, endTime, () => string.Empty);
        }
        else {
            if (_cleaningTimer.activeSelf) {
                _cleaningTimer.SetActive(false);
            }
        }
    }
}