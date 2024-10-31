using Balancy.Models;
using Cysharp.Threading.Tasks;

public class PenisBViewModel : AbstractBackgroundViewModel<PenisBView>, IBackground
{
    public BackgroundType Type => BackgroundType.Penis;

    protected override string ViewAssetName => AssetsConstants.PenisBackground;

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
        _view.SubscribeButton(AppConstants.BarPussyCollect, () => OnGirlClick(AppConstants.BarPussy, true));
        _view.SubscribeButton(AppConstants.AnalisCollect, () => OnGirlClick(AppConstants.Analis, true));
        _view.SubscribeButton(AppConstants.DanceCollect, () => OnGirlClick(AppConstants.Dance, true));
        _view.SubscribeButton(AppConstants.SofaFapCollect, () => OnGirlClick(AppConstants.SofaFap, true));
        _view.SubscribeButton(AppConstants.PenisSofaCollect, () => OnGirlClick(AppConstants.PenisSofa, false));
        _view.SubscribeButton(AppConstants.ChillCollect, () => OnGirlClick(AppConstants.Chill, false));
        _view.SubscribeButton(AppConstants.LoungeCollect, () => OnGirlClick(AppConstants.Lounge, true));
        _view.SubscribeButton(AppConstants.TarotCollect, () => OnGirlClick(AppConstants.Tarot, true));
        _view.SubscribeButton(AppConstants.FightBitchCollect, () => OnGirlClick(AppConstants.FightBitch, false));
        _view.SubscribeButton(AppConstants.FistingCollect, () => OnGirlClick(AppConstants.Fisting, false));
    }
}
