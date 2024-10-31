using UnityEngine;
using UnityEngine.UI;

public class SupportPopupView : AbstractWindowView
{
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _openURLLink;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
            { AppConstants.OpenLinkButton, _openURLLink }
        };
    }
}
