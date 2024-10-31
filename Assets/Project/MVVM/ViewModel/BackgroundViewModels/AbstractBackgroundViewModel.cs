using System;
using Balancy.Models.BigTitShop;
using Balancy;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public abstract class AbstractBackgroundViewModel<T> where T : AbstractBackgroundView
{
    private GeneratorCreator _generatorCreator;

    [Inject] protected IAssetProvider _assetProvider;
    [Inject] protected SoundAudioManager _soundAudioManager;

    [Inject] protected GeneratorManager _generatorManager; 

    [Inject] protected MainValuesModel _mainValuesModel; 
    [Inject] protected BackgroundsModel _model; 
    [Inject] private CleaningModel _cleaningModel;

    protected T _view;

    protected abstract string ViewAssetName { get; }

    protected async UniTask LoadAndCreateView()
    {
        var backgroundPrefab = await _assetProvider.Load<GameObject>(ViewAssetName);
        _view = GameObject.Instantiate(backgroundPrefab).GetComponent<T>();
        _view.CreateDictionaries();

        SubscribeView();

        _generatorCreator = new(_generatorManager);
        _generatorCreator.CreateGenerators();

        _view.Initialize(_generatorManager);

        _generatorManager.CheckGeneratorsStatus();
    }
    protected abstract void SubscribeView();
    protected virtual void DestroyView()
    {
        _view.DestroyView();
        _assetProvider.Unload(ViewAssetName);
        _view = null;
    }
    protected void CollectChips(string generatorName) {
        var earnedChips = _generatorManager.GetGenerator(generatorName).GetEarnedChips();
        var cleaningValue = _cleaningModel.GetCleaningValue();
        var count = (int) Math.Round(earnedChips * cleaningValue);
        
        if (count != 0)
        {
            var chip = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CHIP.UnnyId);
            LiveOps.Profile.Inventories.Currencies.AddItems(chip, count); // поменять
            _generatorManager.RestartGenerator(generatorName);
        }
    }
    protected void OnGirlClick(string generatorName, bool isOneGirl)
    {
        CollectChips(generatorName);
        if (isOneGirl)
        {
            _soundAudioManager.PlayOneGirlClip();
        } else
        {
            _soundAudioManager.PlayTwoGirlClip();
        }
    }
}
