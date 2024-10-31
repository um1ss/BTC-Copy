using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class BackgroundProvider 
{
    [Inject] private IAssetProvider _assetProviderService;

    private AbstractBackground _previosBackground;

    public async UniTask LoadBackground(string backgroundName)
    {
        var backgroundPrefab = await _assetProviderService.Load<GameObject>(backgroundName);
        var background = Object.Instantiate(backgroundPrefab).GetComponent<AbstractBackground>();

        if (_previosBackground != null)
        {
            string name = _previosBackground.Name;
            _previosBackground.Destroy();
            _assetProviderService.Unload(name);
        }
        _previosBackground = background;
    }
}
