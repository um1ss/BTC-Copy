using Balancy.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

public class GeneratorManager 
{
    public UnityAction<IGenerator> OnAddNewGenerator;

    private Dictionary<string, Generator> _data = new();
    private GeneratorProgressData _progressData;
    private SmartList<GeneratorProgressInfo> _generatorProfilerDataList;

    [Inject] private BackgroundsModel _backgroundsModel;
    [Inject] private CleaningModel _cleaningModel;

    public async UniTask InitManager()
    {
        var loaded = false;
        SmartStorage.LoadSmartObject<GeneratorProgressData>(response => {
            _progressData = response.Data;
            loaded = true;
        });
        await UniTask.WaitUntil(() => loaded);
        _generatorProfilerDataList = _progressData.GeneratorProgress.GeneratorProgressList;
    }
    public void CreateNewGenerator(GeneratorData data)
    {
        var generatorData = _generatorProfilerDataList.ToList().Find(info => info.GeneratorName == data.GeneratorName);

        if (generatorData == null)
        {
            var newGen = new GeneratorProgressInfo { GeneratorName = data.GeneratorName };
            _generatorProfilerDataList.Add(newGen);
            generatorData = newGen;
        }

        int lvl = generatorData.GeneratorLvl;
        IGeneratorWorkScheme workScheme = lvl < 10 ? new ClickCollectionWorkScheme() : new AutoCollectionWorkScheme();
        Generator generator = new Generator(data, data.GeneratorName, workScheme, lvl);

        if (_data.ContainsKey(data.GeneratorName))
        {
            _data[data.GeneratorName] = generator;
        }
        else
        {
            _data.Add(data.GeneratorName, generator);
        }

        OnAddNewGenerator?.Invoke(generator);
    }
    public void CheckGeneratorsStatus()
    {
        foreach (var generator in _data)
        {
            if (generator.Value.CurrentLvl > 0)
            {
                generator.Value.ActivateGenerator(_cleaningModel);
            }
        }
    }
    public IGenerator GetGenerator(string generatorName)
    {
        if (_data.TryGetValue(generatorName, out var generator)) return generator;
        return new Generator(null, "None", null, 0);
    }
    public IGenerator GetGeneratorById(int id)
    {
        return _data.Values.FirstOrDefault(generator => generator.Id == id);
    }
    public IEnumerable<IGenerator> GetGenerators()
    {
        var generators = _data.Values.ToList();
        return generators.FindAll(gen => gen.Background == _backgroundsModel.CurrentBackgroundType);
    }
    public void GeneratorLvlUp(string generatorName)
    {
        if (_data.TryGetValue(generatorName, out var generator))
        {
            generator.LvlUp();
            if (generator.CurrentLvl == 10)
            {
                generator.SetWorkScheme(new AutoCollectionWorkScheme());
            } else if (generator.CurrentLvl == 1)
            {
                generator.ActivateGenerator(_cleaningModel);
            }
            var generatorData = _generatorProfilerDataList.ToList().Find(info => info.GeneratorName == generatorName);
            generatorData.GeneratorLvl = generator.CurrentLvl;
        }
    }
    public void GeneratorSetLvl(string generatorName, int lvl)
    {
        if (_data.TryGetValue(generatorName, out var generator))
        {
            generator.DeactivateGenerator();

            generator.SetLvl(lvl);
            if (generator.CurrentLvl >= 10)
            {
                generator.SetWorkScheme(new AutoCollectionWorkScheme());
            }
            else
            {
                generator.SetWorkScheme(new ClickCollectionWorkScheme());
            }

            if (generator.CurrentLvl > 0)
            {
                generator.ActivateGenerator(_cleaningModel);
            }

            var generatorData = _generatorProfilerDataList.ToList().Find(info => info.GeneratorName == generatorName);
            generatorData.GeneratorLvl = generator.CurrentLvl;
        }
    }
    public void RestartGenerator(string generatorName)
    {
        if (_data.TryGetValue(generatorName, out var generator))
        {
            generator.StartFarm(_cleaningModel);
        }
    }
    public void SetSpritesToGenerators(List<Sprite> sprites)
    {
        foreach (var sprite in sprites)
        {
            foreach (var generator in _data)
            {
                if (sprite.name == generator.Value.IconName)
                {
                    generator.Value.SetIcon(sprite);
                    continue;
                }
            }
        }
    }
    public void ClearDictionary()
    {
        _data.Clear();
    }

    public bool IsAllGeneratorsOpened() {
        return GetGenerators().All(generator => generator.CurrentLvl > 0);
    }
}
