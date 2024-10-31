using UnityEngine;

public class VaginaBView : AbstractBackgroundView
{
    [Header("Generators")]
    [SerializeField] protected GeneratorButton _slotMaschines;
    [SerializeField] protected GeneratorButton _bar;
    [SerializeField] protected GeneratorButton _dj;
    [SerializeField] protected GeneratorButton _sofa;
    [SerializeField] protected GeneratorButton _lunch;
    [SerializeField] protected GeneratorButton _roulette;
    [SerializeField] protected GeneratorButton _pokerTable;
    [SerializeField] protected GeneratorButton _lunchLeft;
    [SerializeField] protected GeneratorButton _lunchRight;
    [SerializeField] protected GeneratorButton _vending;

    public override void CreateDictionaries()
    {
        _generatorButtons = new () 
        {
            {AppConstants.SlotMaschinesCollect, _slotMaschines },
            {AppConstants.BarCollect, _bar },
            {AppConstants.DjCollect, _dj },
            {AppConstants.SofaCollect, _sofa },
            {AppConstants.LunchCollect, _lunch },
            {AppConstants.RouletteCollect, _roulette },
            {AppConstants.PokerTableCollect, _pokerTable },
            {AppConstants.LunchLeftCollect, _lunchLeft },
            {AppConstants.LunchRightCollect, _lunchRight },
            {AppConstants.VendingCollect, _vending }
        };
    }

    public override void Initialize(GeneratorManager generatorManager)
    {
        _slotMaschines.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.SlotMaschines));
        _bar.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Bar));
        _dj.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Dj));
        _sofa.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Sofa));
        _lunch.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Lunch));
        _roulette.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Roulette));
        _pokerTable.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.PokerTable));
        _lunchLeft.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.LunchLeft));
        _lunchRight.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.LunchRight));
        _vending.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Vending));
    }

    public void UpdateGirlsSpeed(float speed) {
        foreach (var (key, generatorButton) in _generatorButtons) {
            generatorButton.GirlAnimation.timeScale = speed;
        }
    }
}

