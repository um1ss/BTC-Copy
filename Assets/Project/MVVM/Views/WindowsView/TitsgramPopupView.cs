using UnityEngine;
using UnityEngine.UI;

public class TitsgramPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _window;

    [SerializeField] private Button _closeWindowButton;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
        };
    }
}
