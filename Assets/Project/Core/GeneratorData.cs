using Balancy.Models;

public class GeneratorData
{
    public int GeneratorId {  get; private set; }
    public string GeneratorName {  get; private set; }

    public string IconName { get; private set; }

    public BackgroundType BType { get; private set; }

    public GeneratorMultiples GeneratorParametres { get; private set; }
    
    public GeneratorData(string generatorName, BackgroundType type, int id = 0) {
        GeneratorName = generatorName;
        BType = type;
        GeneratorId = id;
    }
    public void SetGeneratorData(GeneratorMultiples data)
    {
        GeneratorParametres = data;
    }
    public void SetIconName(string iconName)
    {
        IconName = iconName;
    }
}
public struct GeneratorMultiples
{
    public int MaxLvls;
    public long UnlockCost;
    public int BaseIncome;
    public int GenerateTime;

    public float IncomeMultiple;
    public float UpdateCostMultiple;
}
