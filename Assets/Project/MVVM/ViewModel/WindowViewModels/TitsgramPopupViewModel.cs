using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class TitsgramPopupViewModel : AbstractWindowViewModel<TitsgramPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Instagram;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.TitisgramWindow);
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
    }
}
