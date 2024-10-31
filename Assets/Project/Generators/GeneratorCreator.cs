using Balancy;

public class GeneratorCreator 
{
    private readonly GeneratorManager _backgroundGeneratorInfo;

    private GeneratorData _currentData;
    private GeneratorMultiples _currentGeneratorMultipleData;

    public GeneratorCreator(GeneratorManager backgroundGeneratorInfo)
    {
        _backgroundGeneratorInfo = backgroundGeneratorInfo;
        _backgroundGeneratorInfo.ClearDictionary();
    }
    public void CreateGenerators()
    {
        var generatorInfo = DataEditor.Generators;
        foreach (var generator in generatorInfo)
        {
            _currentData = new GeneratorData(generator.Name, generator.Background, generator.Id);
            _currentGeneratorMultipleData = new GeneratorMultiples();
            _currentGeneratorMultipleData.UnlockCost = generator.UnlockCost;
            _currentGeneratorMultipleData.MaxLvls = generator.Lvls;
            _currentGeneratorMultipleData.UpdateCostMultiple = generator.UpdateCosts;
            _currentGeneratorMultipleData.BaseIncome = generator.BaseIncome;
            _currentGeneratorMultipleData.IncomeMultiple = generator.Income;
            _currentGeneratorMultipleData.GenerateTime = generator.GenerateTimeS;

            _currentData.SetIconName(generator.IconName);
            _currentData.SetGeneratorData(_currentGeneratorMultipleData);
            _backgroundGeneratorInfo.CreateNewGenerator(_currentData);
        }
    }
}
