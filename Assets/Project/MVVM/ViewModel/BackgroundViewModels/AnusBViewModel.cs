using Balancy.Models;
using Cysharp.Threading.Tasks;

public class AnusBViewModel : AbstractBackgroundViewModel<AnusBView>, IBackground
{
    public BackgroundType Type => BackgroundType.Anus;

    protected override string ViewAssetName => AssetsConstants.AnusBackground;

    public void Close()
    {
        DestroyView();
    }
    public async UniTask Show()
    {
        await LoadAndCreateView();
    }
    protected override void SubscribeView()
    {
        _view.SubscribeButton(AppConstants.TuchbleCollect, () => OnGirlClick(AppConstants.Tuchble, false));
        _view.SubscribeButton(AppConstants.RouletteAnusCollect, () => OnGirlClick(AppConstants.RouletteAnus, false));
        _view.SubscribeButton(AppConstants.DanceAnusCollect, () => OnGirlClick(AppConstants.DanceAnus, true));
        _view.SubscribeButton(AppConstants.SofaFapAnusCollect, () => OnGirlClick(AppConstants.SofaFapAnus, false));
        _view.SubscribeButton(AppConstants.SlutsCollect, () => OnGirlClick(AppConstants.Sluts, false));
        _view.SubscribeButton(AppConstants.ChillAnusCollect, () => OnGirlClick(AppConstants.ChillAnus, false));
        _view.SubscribeButton(AppConstants.VendingPussyCollect, () => OnGirlClick(AppConstants.VendingPussy, true));
        _view.SubscribeButton(AppConstants.LoopenisCollect, () => OnGirlClick(AppConstants.Loopenis, true));
        _view.SubscribeButton(AppConstants.MachineCollect, () => OnGirlClick(AppConstants.Machine, false));
        _view.SubscribeButton(AppConstants.FistingAnusCollect, () => OnGirlClick(AppConstants.FistingAnus, true));
    }
}
