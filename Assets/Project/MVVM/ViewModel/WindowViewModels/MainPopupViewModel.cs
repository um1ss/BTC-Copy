using Cysharp.Threading.Tasks;
using Project.Core;
using Project.Tutorial;
using UnityEngine.Events;
using VContainer;

public class MainPopupViewModel : AbstractWindowViewModel<MainPopupView>, IWindow
{
    public event UnityAction<WindowTypes> OnAnotherWindowOpen;

    public WindowTypes Type => WindowTypes.Main;

    [Inject] private MainValuesModel _model;
    [Inject] private TutorialCtl _tutorialCtl;
    [Inject] private CleaningModel _cleaningModel;
    [Inject] private MissionsModel _missionsModel;
    [Inject] private ChatModel _chatModel;
    [Inject] private AppRoutines _appRoutines;

    public void Show()
    {
        if (_view != null)
        {
            _view.ShowView();
        }
        else
        {
            CreateView();
            _view.StartRoutines(_appRoutines, _cleaningModel, _missionsModel, _chatModel);
        }
        
        _tutorialCtl.MaybeStartFirstTutorial();
        //SetViewText(AppConstants.ChipsTextsName, Balancy.LiveOps.Profile.Inventories.Currencies.Config.);

    }
    protected override void OpenAnotherWindowView(WindowTypes type)
    {
        OnAnotherWindowOpen?.Invoke(type);
        Hide();
    }
    private void SetViewText(string textName, float value)
    {
        string text = value.ToString();
        int numberCountInText = 3;
        int iterationCount = numberCountInText - text.Length;
        for (int i = 0; i < iterationCount; i++)
        {
            text = "0" + text;
        }
        _view.SetText(textName, text);
    }
    public async UniTask Initialize()
    {
        await LoadView(AssetsConstants.MainButtons);
    }
    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.ChatButtonName, () => OpenAnotherWindowView(WindowTypes.Chat));
        _view.SubscribeButton(AppConstants.CleaningButtonName, () => OpenAnotherWindowView(WindowTypes.Cleaning));
        _view.SubscribeButton(AppConstants.CollectionsButtonName, () => OpenAnotherWindowView(WindowTypes.Collections), true);
        _view.SubscribeButton(AppConstants.MissionsButtonName, () => OpenAnotherWindowView(WindowTypes.Missions), true);
        _view.SubscribeButton(AppConstants.TitsGramButtonName, () => OpenAnotherWindowView(WindowTypes.Instagram));
        _view.SubscribeButton(AppConstants.SlotsButtonName, () => OpenAnotherWindowView(WindowTypes.Slots));
        _view.SubscribeButton(AppConstants.ShopButtonName, () => OpenAnotherWindowView(WindowTypes.Shop));
        _view.SubscribeButton(AppConstants.LevelsButtonName, () => OpenAnotherWindowView(WindowTypes.Levels));
        _view.SubscribeButton(AppConstants.SettingsButtonName, () => OpenAnotherWindowView(WindowTypes.Settings));

        _view.SubscribeButton(AppConstants.AddChipsButtonName, () => OpenAnotherWindowView(WindowTypes.Shop));
        _view.SubscribeButton(AppConstants.AddCondomsButtonName, () => OpenAnotherWindowView(WindowTypes.Shop));
        _view.SubscribeButton(AppConstants.AddEnergyButtonName, () => OpenAnotherWindowView(WindowTypes.Shop));
        _view.SubscribeButton(AppConstants.AddLikeButtonName, () => OpenAnotherWindowView(WindowTypes.Shop));

        _model.SetDelegate(AppConstants.ChipsTextsName, (value) => SetViewText(AppConstants.ChipsTextsName, value));
        _model.SetDelegate(AppConstants.EnergyTextsName, (value) => SetViewText(AppConstants.EnergyTextsName, value));
        _model.SetDelegate(AppConstants.CondomTextsName, (value) => SetViewText(AppConstants.CondomTextsName, value));
        _model.SetDelegate(AppConstants.LikeTextsName, (value) => SetViewText(AppConstants.LikeTextsName, value));
    }
    protected override void CreateView()
    {
        base.CreateView();
        _model.Initialize();
    }
    private void Hide()
    {
        _view.HideView();
    }
    protected override void DestroyView()
    {
        base.DestroyView();
        _model.DisposeValues();
    }
}