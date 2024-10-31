using Balancy.Models;
using Cysharp.Threading.Tasks;
using VContainer;

public class VaginaBViewModel : AbstractBackgroundViewModel<VaginaBView>, IBackground
{
    [Inject] private CleaningModel cleaningModel;
    public BackgroundType Type => BackgroundType.Vagina;

    protected override string ViewAssetName => AssetsConstants.VaginaBackground;

    public void Close() {
        DestroyView();
        cleaningModel.OnGirlsSpeedChanged -= _view.UpdateGirlsSpeed;
    }

    public async UniTask Show() {
        await LoadAndCreateView();
        cleaningModel.OnGirlsSpeedChanged += _view.UpdateGirlsSpeed;
    }

    protected override void SubscribeView() {
        _view.SubscribeButton(AppConstants.SlotMaschinesCollect, () => OnGirlClick(AppConstants.SlotMaschines, true), true);
        _view.SubscribeButton(AppConstants.BarCollect, () => OnGirlClick(AppConstants.Bar, true));
        _view.SubscribeButton(AppConstants.DjCollect, () => OnGirlClick(AppConstants.Dj, false));
        _view.SubscribeButton(AppConstants.SofaCollect, () => OnGirlClick(AppConstants.Sofa, false));
        _view.SubscribeButton(AppConstants.LunchCollect, () => OnGirlClick(AppConstants.Lunch, false));
        _view.SubscribeButton(AppConstants.RouletteCollect, () => OnGirlClick(AppConstants.Roulette, false));
        _view.SubscribeButton(AppConstants.PokerTableCollect, () => OnGirlClick(AppConstants.PokerTable, false));
        _view.SubscribeButton(AppConstants.LunchLeftCollect, () => OnGirlClick(AppConstants.LunchLeft, false));
        _view.SubscribeButton(AppConstants.LunchRightCollect, () => OnGirlClick(AppConstants.LunchRight, false));
        _view.SubscribeButton(AppConstants.VendingCollect, () => OnGirlClick(AppConstants.Vending, true));
    }
}