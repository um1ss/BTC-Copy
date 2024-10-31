using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopupView : AbstractWindowView
{
    [SerializeField] private RectTransform _window;

    [Space]
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _sectionChipsButton;
    [SerializeField] private Button _sectionGirlsButton;
    [SerializeField] private Button _sectionEnergyButtons;
    [SerializeField] private Button _sectionCondomsButtons;

    [Space]
    [SerializeField] private GameObject _chipsSection;
    [SerializeField] private GameObject _girlsSection;
    [SerializeField] private GameObject _energySection;
    [SerializeField] private GameObject _condomsSection;

    private Dictionary<string, GameObject> _sections;

    public override void Initialize()
    {
        _sections = new()
        {
            { AppConstants.ChipsSection, _chipsSection},
            { AppConstants.GirlsSection, _girlsSection},
            { AppConstants.EnergySection, _energySection },
            { AppConstants.CondomsSection, _condomsSection},
        };
        _buttons = new()
        {
            { AppConstants.CloseWindowButtonName, _closeWindowButton },
            { AppConstants.ChipsButtonSection, _sectionChipsButton },
            { AppConstants.GirlsButtonSection, _sectionGirlsButton },
            { AppConstants.EnergyButtonsSection, _sectionEnergyButtons },
            { AppConstants.CondomsButtonsSection, _sectionCondomsButtons },
        };
    }

    public void OpenAnotherSection(string otherSection)
    {
        foreach (var section in _sections)
        {
            if (section.Key == otherSection)
            {
                section.Value.SetActive(true);
            }
            else
            {
                section.Value.SetActive(false);
            }
        }
    }
}
