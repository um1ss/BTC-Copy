using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public interface IAssetProvider 
{
    public UniTask<T> Load<T>(AssetReferenceT<T> assetReference) where T : Object;
    public UniTask<IList<T>> Load<T>(IEnumerable<AssetReferenceT<T>> assetReferences) where T : Object;
    public UniTask<T> Load<T>(string address) where T : Object;
    public void Unload(AssetReference assetReference);
    public void Unload(IEnumerable<AssetReference> assetReference);
    public void Unload(string assetGUID);
    public void CleanUp();
}
