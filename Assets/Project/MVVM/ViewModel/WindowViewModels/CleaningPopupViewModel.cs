using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using VContainer;

public class CleaningPopupViewModel : AbstractWindowViewModel<CleaningPopupView>, IWindow
{
    [Inject] private CleaningModel _cleaningModel;
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Cleaning;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.CleaningWindow);
    }

    public void Show()
    {
        if (_view == null)
        {
            CreateView();
        }
        _view.InitTimer(_cleaningModel);
    }

    protected override void OpenAnotherWindowView(WindowTypes type)
    {
        OnAnotherWindowOpen?.Invoke(type);
        DestroyView();
    }

    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.StartCleaningButtonName, MaybeActivateCleaningAndClose);
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main));
    }

    private void MaybeActivateCleaningAndClose() {
        if (_cleaningModel.TryToActivateCleaning()) {
            OpenAnotherWindowView(WindowTypes.Main);
        }
    }
}