using Balancy;
using Balancy.Models.BigTitShop;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using VContainer;

public sealed class CollectionsPopupViewModel : AbstractWindowViewModel<CollectionPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Collections;

    [Inject] private MainValuesModel _mainValuesModel;
    [Inject] private GeneratorManager _generatorManager;
    [Inject] private CollectionSpritesProvider _collectionSpritesModel;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.CollectionsWindow);
    }
    protected override void OpenAnotherWindowView(WindowTypes type)
    {
        OnAnotherWindowOpen?.Invoke(type);
        DestroyView();
    }
    public void Show()
    {
        if (_view == null)
        {
            CreateView();
            var generatorsData = _generatorManager.GetGenerators();
            foreach (var generator in generatorsData)
            {
                _view.CreateCard(generator, TryUpdateGenerator); 
            }
        }
    }
    private void TryUpdateGenerator(string generatorName)
    {
        var generator = _generatorManager.GetGenerator(generatorName);
        var chips = _mainValuesModel.GetValue(AppConstants.ChipsTextsName);

        if (generator.CurrentLvl == generator.LvlCount)
        {
            return;
        }
        if (chips >= generator.UpdateCost)
        {
            var chip = DataEditor.GetModelByUnnyId<BigTitsItem>(DataEditor.IdBalancyConstants.Get().CHIP.UnnyId);
            LiveOps.Profile.Inventories.Currencies.RemoveItem(chip, (int)generator.UpdateCost); // ��������


            _generatorManager.GeneratorLvlUp(generatorName);
            _view.ChangeCardView(generatorName, $"{generator.CurrentLvl}/{generator.LvlCount}", $"{generator.UpdateCost}", (float)generator.CurrentLvl / generator.LvlCount);
        }
    }
    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main), true);
    }
    protected override void DestroyView()
    {
        var generatorsData = _generatorManager.GetGenerators();
        foreach (var generator in generatorsData)
        {
            _view.UnsubscribeCard(generator);
        }
        base.DestroyView();
    }
}
