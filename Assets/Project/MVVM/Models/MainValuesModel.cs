using Balancy;
using Balancy.Data.SmartObjects;
using Balancy.Models.BigTitShop;
using R3;
using System;
using Project.Core;
using VContainer;

public class MainValuesModel : AbstractModel<long>
{
    private Inventory _currencyInv;
    public MainValuesModel()
    {
        _values.Add(AppConstants.ChipsTextsName, new ReactiveProperty<long>());
        _values.Add(AppConstants.CondomTextsName, new ReactiveProperty<long>());
        _values.Add(AppConstants.EnergyTextsName, new ReactiveProperty<long>());
        _values.Add(AppConstants.LikeTextsName, new ReactiveProperty<long>());
    }
    public override void SetDelegate(string name, Action<long> del)
    {
        if (_values.TryGetValue(name, out var property))
        {
            if (_disposes.ContainsKey(name))
            {
                _disposes[name] = property.Subscribe(del);
            }
            else
            {
                _disposes.Add(name, property.Subscribe(del));
            }
        }
    }
    public override void ChangeValue(string name, long newValue)
    {
        if (_values.TryGetValue(name, out var property))
        {
            property.Value += newValue;
        }
    }
    public override void Initialize()
    {
        _currencyInv = LiveOps.Profile.Inventories.Currencies;
        _currencyInv.OnItemWasRemoved += RemoveItemValue;
        _currencyInv.OnNewItemWasAdded += AddItemValue;

        var condom = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CONDOM.UnnyId);
        var energy = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().ENERGY.UnnyId);
        var chip = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CHIP.UnnyId);
        var condomValue = _currencyInv.GetTotalAmountOfItems(condom);
        var energyValue = _currencyInv.GetTotalAmountOfItems(energy);
        var chipValue = _currencyInv.GetTotalAmountOfItems(chip);

        ChangeValue(AppConstants.CondomTextsName, condomValue);
        ChangeValue(AppConstants.EnergyTextsName, energyValue);
        ChangeValue(AppConstants.ChipsTextsName, chipValue);
    }
    private void AddItemValue(Balancy.Models.SmartObjects.Item item, int count, int slotIndex)
    {
        ChangeValue(item.Name.Value, count);
    }
    private void RemoveItemValue(Balancy.Models.SmartObjects.Item item, int count, int slotIndex)
    {
        ChangeValue(item.Name.Value, -count);
    }
    public void DisposeValues()
    {
        foreach (var dispose in _disposes)
        {
            dispose.Value.Dispose();
        }
    }
}
