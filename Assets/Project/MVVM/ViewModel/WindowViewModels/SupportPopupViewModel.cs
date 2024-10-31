using Cysharp.Threading.Tasks;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class SupportPopupViewModel : AbstractWindowViewModel<SupportPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;

    public WindowTypes Type => WindowTypes.Support;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.SupportWindow);
    }

    public void Show()
    {
        if (_view == null)
        {
            CreateView();
        }
    }

    protected override void OpenAnotherWindowView(WindowTypes type)
    {
        OnAnotherWindowOpen?.Invoke(type);
        DestroyView();
    }

    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Settings));
        _view.SubscribeButton(AppConstants.OpenLinkButton, OpenURL);
    }

    [DllImport("__Internal")]
    private static extern void OpenURLInAnotherWindow(string url);
    private void OpenURL()
    {
        #if UNITY_WEBGL
            OpenURLInAnotherWindow(AppConstants.DISCORD_SUPPORT_LINK);
        #else
            Application.OpenURL(AppConstants.DISCORD_SUPPORT_LINK);
        #endif
    }
}
