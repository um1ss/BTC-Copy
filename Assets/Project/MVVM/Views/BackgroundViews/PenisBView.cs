using UnityEngine;

public class PenisBView : AbstractBackgroundView
{
    [Header("Generators")]
    [SerializeField] protected GeneratorButton _barPussy;
    [SerializeField] protected GeneratorButton _analis;
    [SerializeField] protected GeneratorButton _dance;
    [SerializeField] protected GeneratorButton _sofaFap;
    [SerializeField] protected GeneratorButton _penisSofa;
    [SerializeField] protected GeneratorButton _chill;
    [SerializeField] protected GeneratorButton _lounge;
    [SerializeField] protected GeneratorButton _tarot;
    [SerializeField] protected GeneratorButton _ring;
    [SerializeField] protected GeneratorButton _fisting;

    public override void CreateDictionaries()
    {
        _generatorButtons = new()
        {
            {AppConstants.BarPussyCollect, _barPussy },
            {AppConstants.AnalisCollect, _analis },
            {AppConstants.DanceCollect, _dance },
            {AppConstants.SofaFapCollect, _sofaFap },
            {AppConstants.PenisSofaCollect, _penisSofa },
            {AppConstants.ChillCollect, _chill },
            {AppConstants.LoungeCollect, _lounge },
            {AppConstants.TarotCollect, _tarot },
            {AppConstants.FightBitchCollect, _ring },
            {AppConstants.FistingCollect, _fisting }
        };
    }

    public override void Initialize(GeneratorManager generatorManager)
    {
        _barPussy.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.BarPussy));
        _analis.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Analis));
        _dance.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Dance));
        _sofaFap.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.SofaFap));
        _penisSofa.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.PenisSofa));
        _chill.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Chill));
        _lounge.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Lounge));
        _tarot.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Tarot));
        _ring.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.FightBitch));
        _fisting.SubscribeGenerator(generatorManager.GetGenerator(AppConstants.Fisting));
    }
}
