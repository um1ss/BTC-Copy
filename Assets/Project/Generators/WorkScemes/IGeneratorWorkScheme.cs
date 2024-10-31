using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public interface IGeneratorWorkScheme 
{
    bool IsFarming { get; }
    UniTaskVoid GeneratorFarmAsync(UnityAction<float> tickAction, UnityAction endFarmAction, int farmDuration);
    void StopGenerate();
}
