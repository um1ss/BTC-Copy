using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class ChatViewModel : AbstractWindowViewModel<ChatPopupView>, IWindow {
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Chat;

    public async UniTask Initialize() {
        await LoadView(AssetsConstants.ChatWindow);
    }

    public void Show() {
        if (_view == null) 
            CreateView();
        _view.ShowView();
    }

    protected override void OpenAnotherWindowView(WindowTypes type) {
        OnAnotherWindowOpen?.Invoke(type);
        _view.HideView();
    }

    private void OnShopRequired() {
        OnAnotherWindowOpen?.Invoke(WindowTypes.Shop);
        _view.HideView();
    }

    protected override void SubscribeView() {
        _view.OnShopRequired += OnShopRequired;
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main));
    }
    
    private void UnsubscribeView() {
        _view.OnShopRequired += OnShopRequired;
    }

    protected override void DestroyView() {
        base.DestroyView();
        UnsubscribeView();
    }
}