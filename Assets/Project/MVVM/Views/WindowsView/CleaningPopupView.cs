using System;
using Balancy.Localization;
using Project.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CleaningPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _window;

    [SerializeField] private Button _startCleaningButton;
    [SerializeField] private TMP_Text _startCleaningBtnTxt;
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private GameObject _timer;
    [SerializeField] private TMP_Text _timerTxt;
    [SerializeField] private TMP_Text _descriptionTxt;
    [SerializeField] private TMP_Text _headerTxt;

    private CleaningModel _cleaningModel;

    public override void Initialize() {
        _buttons = new() {
            {AppConstants.StartCleaningButtonName, _startCleaningButton},
            {AppConstants.CloseWindowButtonName, _closeWindowButton},
        };
        
        _startCleaningBtnTxt.text = Manager.Get("DEFAULT/LetsGo");
        _headerTxt.text = Manager.Get("DEFAULT/CleaningHeader");
        _descriptionTxt.text = Manager.Get("DEFAULT/CleaningDescription");
    }

    public void InitTimer(CleaningModel cleaningModel) {
        _cleaningModel = cleaningModel;
        PeriodicallyUpdate();
        InvokeRepeating(nameof(PeriodicallyUpdate), 0, 0.5f);
    }

    private void PeriodicallyUpdate() {
        UpdateBtn();
        UpdateTimer();
    }

    private void UpdateBtn() {
        var canActivate = _cleaningModel.CanActivateCleaning();
        if (canActivate != _startCleaningButton.interactable) {
            _startCleaningButton.interactable = canActivate;
        }
    }

    private void UpdateTimer() {
        if (_cleaningModel.TryGetTimerEndValue(out var endTime)) {
            if (!_timer.activeSelf) {
                _timer.SetActive(true);
            }

            var now = DateTimeOffset.Now;
            _timerTxt.text = TimeFormatUtils.GetTimerTextShort(now, endTime, () => string.Empty);
        }
        else {
            if (_timer.activeSelf) {
                _timer.SetActive(false);
            }
        }
    }
}