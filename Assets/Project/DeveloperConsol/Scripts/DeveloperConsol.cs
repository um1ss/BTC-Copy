using Balancy;
using Balancy.Models.BigTitShop;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class DeveloperConsol : MonoBehaviour
{
    [Inject] protected GeneratorManager _generatorManager;
    [Inject] protected MainValuesModel _mainModel;

    // Values
    [SerializeField] private TMP_InputField _chips;
    [SerializeField] private TMP_InputField _condoms;
    [SerializeField] private TMP_InputField _energy;

    [SerializeField] private List<GeneratorField> _fields;
    [SerializeField] private GameObject _consolObj;
    [SerializeField] private GameObject _openButton;

    private BigTitsItem _chipItem;
    private BigTitsItem _condomsItem;
    private BigTitsItem _energyItem;

    public bool _isEnableConsol;

    private void Awake()
    {
        _chips.onEndEdit.AddListener((value) => ChangeMainValue(_chipItem, value));
        _condoms.onEndEdit.AddListener((value) => ChangeMainValue(_condomsItem, value));
        _energy.onEndEdit.AddListener((value) => ChangeMainValue(_energyItem, value));

        foreach (var field in _fields)
        {
            field.Init(_generatorManager);
        }
        _consolObj.SetActive(false);
        _openButton.SetActive(_isEnableConsol);
    }
    public void LoadData()
    {
        _chipItem = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CONDOM.UnnyId);
        _condomsItem = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CONDOM.UnnyId);
        _energyItem = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().ENERGY.UnnyId);
    }
    private void ChangeMainValue(BigTitsItem item, string value)
    {
        if (int.TryParse(value, out int result))
        {
            LiveOps.Profile.Inventories.Currencies.AddItems(item, result);
        }
    }
}
