using Project.Core;
using Project.Tutorial;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private MusicAudioManager _audioManagerPrefab;
    [SerializeField] private SoundAudioManager _soundManagerPrefab;
    [SerializeField] private DeveloperConsol _developerConsol;

    [SerializeField] private AssetLabelReference _vaginaCardsRef;
    [SerializeField] private AssetLabelReference _anusCardsRef;
    [SerializeField] private AssetLabelReference _penisCardsRef;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    protected override void Configure(IContainerBuilder builder)
    {
        var collectionSpritesModel = new CollectionSpritesProvider(_vaginaCardsRef, _anusCardsRef, _penisCardsRef);
        builder.RegisterComponent(collectionSpritesModel);

        builder.Register<AppRoutines>(Lifetime.Singleton).AsSelf();
        
        RegisterInfrastructureServices(builder);
        RegisterWindowsMVVM(builder);
        RegisterBackgroundsMVVM(builder);

        builder.Register<UIWindowManager>(Lifetime.Singleton).AsSelf();
        builder.Register<UIBackgroundManager>(Lifetime.Singleton).AsSelf();

        builder.RegisterComponentInNewPrefab(_audioManagerPrefab, Lifetime.Singleton).UnderTransform(transform).AsSelf();
        builder.RegisterComponentInNewPrefab(_soundManagerPrefab, Lifetime.Singleton).UnderTransform(transform).AsSelf();

        builder.RegisterComponentInNewPrefab(_developerConsol, Lifetime.Singleton).UnderTransform(transform).AsSelf();

        builder.RegisterEntryPoint<AppStart>();
    }

    private static void RegisterInfrastructureServices(IContainerBuilder builder)
    {
        builder.Register<PlayerData>(Lifetime.Singleton).AsSelf();
        builder.Register<AssetProviderService>(Lifetime.Singleton).As<IAssetProvider>();
        builder.Register<SceneLoaderService>(Lifetime.Singleton).As<ISceneLoader>();
        builder.Register<LoadingScreenProvider>(Lifetime.Singleton).AsSelf();
    }
    private static void RegisterWindowsMVVM(IContainerBuilder builder)
    {
        builder.Register<MainValuesModel>(Lifetime.Singleton);
        builder.Register<SettingsModel>(Lifetime.Singleton);
        builder.Register<MissionsModel>(Lifetime.Singleton);
        builder.Register<CleaningModel>(Lifetime.Singleton);
        builder.Register<ChatModel>(Lifetime.Singleton);
        
        builder.Register<PromoteModel>(Lifetime.Singleton);
        
        builder.Register<IWindow, MainPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, ChatViewModel>(Lifetime.Scoped);
        
        builder.Register<IWindow, CleaningPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, CollectionsPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, LocationPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, MissionPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, SettingPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, ShopPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, SlotmachinePopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, TitsgramPopupViewModel>(Lifetime.Scoped);
        builder.Register<IWindow, SupportPopupViewModel>(Lifetime.Scoped);
    }
    private static void RegisterBackgroundsMVVM(IContainerBuilder builder)
    {
        builder.Register<GeneratorManager>(Lifetime.Singleton).AsSelf();

        builder.Register<BackgroundsModel>(Lifetime.Singleton);

        builder.Register<IBackground, VaginaBViewModel>(Lifetime.Scoped);
        builder.Register<IBackground, PenisBViewModel>(Lifetime.Scoped);
        builder.Register<IBackground, AnusBViewModel>(Lifetime.Scoped);
        
        builder.Register<TutorialCtl>(Lifetime.Singleton).AsSelf();
    }
}