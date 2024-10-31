using UnityEngine;
using UnityEngine.UI;

public class LocationPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _window;

    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _vaginaButton;
    [SerializeField] private Button _penisButton;
    [SerializeField] private Button _anusButton;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
            { AppConstants.VaginaButtonName, _vaginaButton },
            { AppConstants.PenisButtonName, _penisButton },
            { AppConstants.AnusButtonName, _anusButton }
        };
    }
}
