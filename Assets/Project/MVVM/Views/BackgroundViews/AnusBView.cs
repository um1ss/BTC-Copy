using UnityEngine;

public class AnusBView : AbstractBackgroundView
{
    [Header("Generators")]
    [SerializeField] protected GeneratorButton _tuchble;
    [SerializeField] protected GeneratorButton _roulette;
    [SerializeField] protected GeneratorButton _dance;
    [SerializeField] protected GeneratorButton _sofaFap;
    [SerializeField] protected GeneratorButton _sluts;
    [SerializeField] protected GeneratorButton _chill;
    [SerializeField] protected GeneratorButton _vendingPussy;
    [SerializeField] protected GeneratorButton _loopenis;
    [SerializeField] protected GeneratorButton _machine;
    [SerializeField] protected GeneratorButton _fisting;

    public override void CreateDictionaries()
    {
        _generatorButtons = new()
        {
            {AppConstants.TuchbleCollect, _tuchble },
            {AppConstants.RouletteAnusCollect, _roulette },
            {AppConstants.DanceAnusCollect, _dance },
            {AppConstants.SofaFapAnusCollect, _sofaFap },
            {AppConstants.SlutsCollect, _sluts },
            {AppConstants.ChillAnusCollect, _chill },
            {AppConstants.VendingPussyCollect, _vendingPussy },
            {AppConstants.LoopenisCollect, _loopenis },
            {AppConstants.MachineCollect, _machine },
            {AppConstants.FistingAnusCollect, _fisting }
        };
    }

    public override void Initialize(GeneratorManager generatorManager)
    {
        _tuchble.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Tuchble));
        _roulette.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.RouletteAnus));
        _dance.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.DanceAnus));
        _sofaFap.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.SofaFapAnus));
        _sluts.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Sluts));
        _chill.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.ChillAnus));
        _vendingPussy.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.VendingPussy));
        _loopenis.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Loopenis));
        _machine.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Machine));
        _fisting.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.FistingAnus));
    }
}
