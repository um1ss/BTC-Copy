using UnityEngine.Events;
using VContainer;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Balancy.Models;

public class LocationPopupViewModel : AbstractWindowViewModel<LocationPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Levels;

    [Inject] private LoadingScreenProvider _loadingScreenProvider;
    [Inject] private UIBackgroundManager _backgroundManager;
    [Inject] private CollectionSpritesProvider _collectionSpritesModel;
    [Inject] private GeneratorManager _generatorManager;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.LocationsWindow);
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
        }
    }
    private async void OpenNewBackground(BackgroundType type)
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new LoadBackground(type, _backgroundManager, _collectionSpritesModel, _generatorManager));
        await _loadingScreenProvider.LoadAndDestroy(loadingOperations);
        OpenAnotherWindowView(WindowTypes.Main);
    }

    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main));

        _view.SubscribeButton(AppConstants.VaginaButtonName, () => OpenNewBackground(BackgroundType.Vagina));
        _view.SubscribeButton(AppConstants.PenisButtonName, () => OpenNewBackground(BackgroundType.Penis));
        _view.SubscribeButton(AppConstants.AnusButtonName, () => OpenNewBackground(BackgroundType.Anus));
    }
}
