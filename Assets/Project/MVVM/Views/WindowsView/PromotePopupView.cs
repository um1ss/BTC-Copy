using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotePopupView : AbstractWindowView
{
    [Header("Buttons")] [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _promoteButton;

    public override void Initialize()
    {
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
            { AppConstants.PromoteButton, _promoteButton }
        };
    }
}