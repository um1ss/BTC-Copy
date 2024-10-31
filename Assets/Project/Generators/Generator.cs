using System;
using Balancy.Models;
using UnityEngine;
using UnityEngine.Events;

public class Generator : IGenerator
{

    public event UnityAction<int> OnEndGenerate;

    public event UnityAction<float> OnGenerateTick;

    public event UnityAction OnStartGenerateCycleGenerate;

    public event UnityAction<string> OnActivateGenerate;

    public event UnityAction<int> OnLvlChange;

    public event UnityAction OnDeactivateGenerate;

    private int _earnedChips;
    private int _currentLvl;
    private int _income;
    private int _incomeMultiple;
    private long _updateCost;

    public int CurrentLvl 
    {  
        get 
        { 
            return _currentLvl; 
        }
        private set
        {
            if (value > -1)
            {
                _currentLvl = value;
                CalculateLvlUpChanges();
                OnLvlChange?.Invoke(_currentLvl);
            }
        }
    }

    private GeneratorData _data;

    private IGeneratorWorkScheme _workScheme;

    public string Name {  get; private set; }
    public int Id => _data.GeneratorId;
    public int Income => _income;
    public long UpdateCost => _updateCost;
    public int GenerateTime => _data.GeneratorParametres.GenerateTime;
    public int LvlCount => _data.GeneratorParametres.MaxLvls; 
    public string IconName => _data.IconName;
    public BackgroundType Background => _data.BType;
    public Sprite Icon {  get; private set; }

    public Generator(GeneratorData data, string name, IGeneratorWorkScheme workScheme, int lvl)
    {
        if (data == null)
        {
            return;
        }
        _data = data;
        _workScheme = workScheme;
        Name = name;

        _income = _data.GeneratorParametres.BaseIncome;
        if (lvl == 0)
        {
            _updateCost = _data.GeneratorParametres.UnlockCost;
        } else
        {
            for (int i = 0; i < lvl; i++)
            {
                CalculateLvlUpChanges();
            }
        }
        CurrentLvl = lvl;
    }
    public void SetIcon(Sprite icon)
    {
        Icon = icon;
    }
    public void SetWorkScheme(IGeneratorWorkScheme workScheme)
    {
        _workScheme = workScheme;
    }
    public void LvlUp()
    {
        CurrentLvl++;
    }
    public void SetLvl(int lvl)
    {
        CurrentLvl = lvl;
    }
    public void ActivateGenerator(CleaningModel cleaningModel)
    {
        OnActivateGenerate?.Invoke(Name);
        StartFarm(cleaningModel);
    }
    public void StartFarm(CleaningModel cleaningModel)
    {
        if (_workScheme != null && !_workScheme.IsFarming)
        {
            OnStartGenerateCycleGenerate?.Invoke();
            _workScheme.GeneratorFarmAsync((delta) => OnGenerateTick?.Invoke(delta), () => OnEndGenerateCycle(cleaningModel), GenerateTime);
        }
    }
    public int GetEarnedChips()
    {
        int returnValue = _earnedChips;
        _earnedChips = 0;
        return returnValue;
    }
    private void OnEndGenerateCycle(CleaningModel cleaningModel)
    {
        _earnedChips += Income;
        var earnedChips = (int) Math.Round(_earnedChips * cleaningModel.GetCleaningValue());
        OnEndGenerate?.Invoke(earnedChips);
    }
    private void CalculateLvlUpChanges()
    {
        _income = _data.GeneratorParametres.BaseIncome;
        _updateCost = _data.GeneratorParametres.UnlockCost;
        for (int i = 0; i < CurrentLvl; i++)
        {
            _income = Mathf.CeilToInt(_income * _data.GeneratorParametres.IncomeMultiple);
            _updateCost = Mathf.CeilToInt(_income * _data.GeneratorParametres.UpdateCostMultiple);
        }
    }
    public void DeactivateGenerator()
    {
        _workScheme.StopGenerate();
        OnDeactivateGenerate?.Invoke();
    }
}
