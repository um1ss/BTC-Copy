using UnityEngine.Events;
using VContainer;
using Cysharp.Threading.Tasks;

public class SettingPopupViewModel : AbstractWindowViewModel<SettingPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;
    public WindowTypes Type => WindowTypes.Settings;

    [Inject] private SettingsModel _model;

    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.SettingsWindow);
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
    private void SetViewSLiderValue(string textName, float value)
    {
        _view.SetSliderValue(textName, value);
    }
    private void SetViewText(string textName, string value)
    {
        string text = "User ID:" + value;
        _view.SetText(textName, text);
    }
    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.CloseWindowButtonName, () => OpenAnotherWindowView(WindowTypes.Main));
        _view.SubscribeButton(AppConstants.SupportButtonName, () => OpenAnotherWindowView(WindowTypes.Support));

        _view.SubscribeSlider(AppConstants.SoundValueName, (value) => _model.ChangeValue(AppConstants.SoundValueName, value));
        _view.SubscribeSlider(AppConstants.MusicValueName, (value) => _model.ChangeValue(AppConstants.MusicValueName, value));
    }
    protected override void CreateView()
    {
        base.CreateView();
        SetViewSLiderValue(AppConstants.SoundValueName, _model.GetValue(AppConstants.SoundValueName));
        SetViewSLiderValue(AppConstants.MusicValueName, _model.GetValue(AppConstants.MusicValueName));
        SetViewText(AppConstants.UserIDValueName, _model.GetUserId());
    }
}
