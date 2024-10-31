using UnityEngine;
using UnityEngine.UI;

public class SlotmachinePopupView : AbstractWindowView
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
