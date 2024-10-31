using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class LoadingScreenProvider 
{
    [Inject] private IAssetProvider _assetProviderService;

    public async UniTask LoadAndDestroy(ILoadingOperation loadingOperation)
    {
        var operations = new Queue<ILoadingOperation>();
        operations.Enqueue(loadingOperation);
        await LoadAndDestroy(operations);
    }

    public async UniTask LoadAndDestroy(Queue<ILoadingOperation> loadingOperations)
    {
        var canvasPrefab = await _assetProviderService.Load<GameObject>(AssetsConstants.LoadingScreen);
        var loadingScreen = Object.Instantiate(canvasPrefab).GetComponent<LoadScreen>();
        await loadingScreen.Load(loadingOperations);
        Object.Destroy(loadingScreen.gameObject);
        _assetProviderService.Unload(AssetsConstants.LoadingScreen);
    }
}
