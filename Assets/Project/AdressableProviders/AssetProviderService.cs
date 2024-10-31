using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetProviderService : IAssetProvider
{
    private readonly Dictionary<string, AsyncOperationHandle> completedCache = new();

    public AssetProviderService()
    {
        Addressables.InitializeAsync();
    }

    public async UniTask<T> Load<T>(AssetReferenceT<T> assetReference) where T : Object
    {
        if (completedCache.TryGetValue(assetReference.AssetGUID, out var completedHandle))
            return completedHandle.Result as T;

        return await RunWithCacheOnComplete(
            Addressables.LoadAssetAsync<T>(assetReference),
            assetReference.AssetGUID
        );
    }

    public async UniTask<IList<T>> Load<T>(IEnumerable<AssetReferenceT<T>> assetReferences) where T : Object
    {
        var tasks = assetReferences.Select(Load);
        return await UniTask.WhenAll(tasks);
    }

    public async UniTask<T> Load<T>(string address) where T : Object
    {
        if (completedCache.TryGetValue(address, out var completedHandle)) return completedHandle.Result as T;

        return await RunWithCacheOnComplete(
            Addressables.LoadAssetAsync<T>(address),
            address
        );
    }

    public void Unload(AssetReference assetReference)
    {
        if (completedCache.TryGetValue(assetReference.AssetGUID, out var completedHandle))
        {
            Addressables.Release(completedHandle);
            completedCache.Remove(assetReference.AssetGUID);
        }
    }
    public void Unload(string assetGUID)
    {
        if (completedCache.TryGetValue(assetGUID, out var completedHandle))
        {
            Addressables.Release(completedHandle);
            completedCache.Remove(assetGUID);
        }
    }

    public void Unload(IEnumerable<AssetReference> assetReferences)
    {
        assetReferences.ForEach(Unload);
    }

    public void CleanUp()
    {
        completedCache.Clear();
    }

    private async UniTask<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey)
        where T : class
    {
        handle.Completed += completedHandle => completedCache[cacheKey] = completedHandle;
        return await handle.ToUniTask();
    }
}
