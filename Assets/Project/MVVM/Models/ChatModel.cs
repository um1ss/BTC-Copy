using System;
using System.Threading;
using Balancy.Data;
using Balancy.Data.SmartObjects;
using Cysharp.Threading.Tasks;
using LustTicTitsToe;
using Sirenix.Utilities;
using UnityEngine;
using VContainer;

public class ChatModel : AbstractModel<int>, ILoadingOperation
{
    
    public event Action<IGenerator> onGeneratorOpenFirstTime;
    
    [Inject]
    GeneratorManager generatorManager;

    public bool IsIndicatorAvailable => LustTicTitsToe.PlayerData.Instance.HasNewChat;
    public bool IsAvailable => LustTicTitsToe.PlayerData.Instance.HasChats;

    public async UniTask Load()
    {
        await LustTicTitsToe.PlayerData.Instance.Initialize();
        ChatData.Instance.Initialize();
        
        generatorManager.GetGenerators().ForEach(g => {
            if (g.CurrentLvl > 0)
                OnActivateGenerator(g.Name);
            g.OnActivateGenerate += OnActivateGenerator;
        });
    }
    
    void OnActivateGenerator(string generator) {
        LustTicTitsToe.PlayerData.Instance.OnActivateGenerator(generator);
    }

    public override void SetDelegate(string name, Action<int> del) { }

    public override void ChangeValue(string name, int newValue) { }
}