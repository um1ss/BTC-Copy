using Balancy.Models;
using Cysharp.Threading.Tasks;
public class LoadBackground : ILoadingOperation
{
    private BackgroundType _backgroundType;
    private UIBackgroundManager _backgroundProvider;
    private CollectionSpritesProvider _collectionSpritesProvider;
    private GeneratorManager _generatorManager;

    public LoadBackground(BackgroundType backType, UIBackgroundManager backProvider, CollectionSpritesProvider spritesModel, GeneratorManager generatorManager)
    {
        _collectionSpritesProvider = spritesModel;
        _backgroundType = backType;
        _backgroundProvider = backProvider;
        _generatorManager = generatorManager;
    }
    public async UniTask Load()
    {
        await _backgroundProvider.OpenBackground(_backgroundType);
        var list = await _collectionSpritesProvider.LoadNewCardSprites(_backgroundType);
        _generatorManager.SetSpritesToGenerators(list);
    }
}
