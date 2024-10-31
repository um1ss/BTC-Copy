using Balancy.Models;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class CollectionSpritesProvider
{
    public UnityAction OnLoadNewSprites;

    private AssetLabelReference _vaginaCardsRef;
    private AssetLabelReference _anusCardsRef;
    private AssetLabelReference _penisCardsRef;

    private Dictionary<BackgroundType, AssetLabelReference> _labels;

    public CollectionSpritesProvider(AssetLabelReference vaginaC, AssetLabelReference anusC, AssetLabelReference penisC)
    {
        _vaginaCardsRef = vaginaC;
        _anusCardsRef = anusC;
        _penisCardsRef = penisC;

        _labels = new()
        {
            { BackgroundType.Vagina, _vaginaCardsRef },
            { BackgroundType.Anus, _anusCardsRef },
            { BackgroundType.Penis, _penisCardsRef },
        };
    }

    public async UniTask<List<Sprite>> LoadNewCardSprites(BackgroundType type)
    {
        var list = new List<Sprite>();
        if (_labels.TryGetValue(type, out var label))
        {
            await Addressables.LoadAssetsAsync<Sprite>(label, (sprite) => list.Add(sprite));
        }
        return list;
    }
}
