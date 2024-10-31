using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using VContainer;

public class ShopPopupViewModel : AbstractWindowViewModel<ShopPopupViewTest>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Shop;

    [Inject] private MainValuesModel _mainValuesModel;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.ShopWindow);
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

    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main));
        //_view.SubscribeButton(AppConstants.ChipsButtonSection, () => _view.OpenAnotherSection(AppConstants.ChipsSection));
        //_view.SubscribeButton(AppConstants.GirlsButtonSection, () => _view.OpenAnotherSection(AppConstants.GirlsSection));
        //_view.SubscribeButton(AppConstants.EnergyButtonsSection, () => _view.OpenAnotherSection(AppConstants.EnergySection));
        //_view.SubscribeButton(AppConstants.CondomsButtonsSection, () => _view.OpenAnotherSection(AppConstants.CondomsSection));
    }
}
