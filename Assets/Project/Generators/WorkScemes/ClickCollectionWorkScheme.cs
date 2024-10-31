using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class ClickCollectionWorkScheme : IGeneratorWorkScheme
{
    private CancellationTokenSource _cancellationTokenSource;
    private float _startEarnTime;
    private readonly float _updateBarDelay = 0.4f;

    public bool IsFarming { get; private set; }

    public async UniTaskVoid GeneratorFarmAsync(UnityAction<float> tickAction, UnityAction endFarmAction, int farmDuration)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _startEarnTime = Time.time;
        IsFarming = true;
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            var delta = (Time.time - _startEarnTime) / farmDuration;
            if (delta >= 1)
            {
                endFarmAction?.Invoke();
                StopGenerate();
            }
            tickAction?.Invoke(delta);
            await UniTask.Delay(TimeSpan.FromSeconds(_updateBarDelay), cancellationToken: _cancellationTokenSource.Token);
        }
    }
    public void StopGenerate()
    {
        IsFarming = false;
        _cancellationTokenSource.Cancel();
    }
}
