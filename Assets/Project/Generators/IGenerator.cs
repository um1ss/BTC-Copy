using UnityEngine.Events;
using UnityEngine;

public interface IGenerator
{
    event UnityAction<int> OnEndGenerate;
    event UnityAction OnStartGenerateCycleGenerate;
    event UnityAction<float> OnGenerateTick;
    event UnityAction<int> OnLvlChange;
    event UnityAction<string> OnActivateGenerate;
    event UnityAction OnDeactivateGenerate;

    int CurrentLvl {  get; }
    int LvlCount { get; }
    int Income { get; }
    long UpdateCost { get; }
    string Name { get; }
    public int Id { get; }
    string IconName { get; }
    Sprite Icon { get; }

    int GetEarnedChips();
    void SetLvl(int lvl);
}
