using System.Collections.Generic;
using System.Threading;
using Balancy.Models;
using Cysharp.Threading.Tasks;
using LustTicTitsToe;
using Project.Tutorial;
using VContainer;
using VContainer.Unity;

public class AppStart : IAsyncStartable
{
    [Inject] private LoadingScreenProvider _loadingScreenProvider;
    [Inject] private UIBackgroundManager _backgroundManager;
    [Inject] private UIWindowManager _uIManager;
    [Inject] private CollectionSpritesProvider _collectionSpritesModel;
    [Inject] private GeneratorManager _generatorManager;

    [Inject] private MusicAudioManager _musicManager;
    [Inject] private SoundAudioManager _soundManager;
    [Inject] private SettingsModel _settingsModel;
    [Inject] private MissionsModel _missionsModel;
    [Inject] private TutorialCtl _tutorialCtl;
    [Inject] private ChatModel _chatModel;
    [Inject] private CleaningModel _cleaningModel;
    
    [Inject] private DeveloperConsol _developerConsol;

    public async UniTask StartAsync(CancellationToken cancellation = default)
    {
        var loadingOperations = new Queue<ILoadingOperation>();
        loadingOperations.Enqueue(new SDKInitialithation());
        loadingOperations.Enqueue(_cleaningModel);
        loadingOperations.Enqueue(new InitializeServises(_generatorManager, _musicManager, _soundManager, _settingsModel));
        
        loadingOperations.Enqueue(new LoadAudio(_musicManager, true));
        loadingOperations.Enqueue(new LoadMainScene());
        loadingOperations.Enqueue(new LoadBackground(BackgroundType.Vagina, _backgroundManager, _collectionSpritesModel, _generatorManager));
        
        loadingOperations.Enqueue(_missionsModel);
        loadingOperations.Enqueue(_tutorialCtl);
        loadingOperations.Enqueue(new InitializeWindows(_uIManager));
        loadingOperations.Enqueue(_chatModel);
        
        await _loadingScreenProvider.LoadAndDestroy(loadingOperations);

        _missionsModel.Init(_generatorManager);
        
        _developerConsol.LoadData();
        
        _cleaningModel.Init(_generatorManager);
    }
}