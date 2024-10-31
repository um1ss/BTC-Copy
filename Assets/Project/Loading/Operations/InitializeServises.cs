using Balancy.Data;
using Cysharp.Threading.Tasks;

public class InitializeServises : ILoadingOperation
{
    private GeneratorManager _generaorManager;
    private MusicAudioManager _musicManager;
    private SoundAudioManager _soundManager;
    private SettingsModel _settingsModel;

    private SoundSettingsData _settingData;

    public InitializeServises(GeneratorManager generaorManager, MusicAudioManager musicManager, SoundAudioManager soundManager, SettingsModel settingsModel)
    {
        _generaorManager = generaorManager;
        _musicManager = musicManager;
        _soundManager = soundManager;
        _settingsModel = settingsModel;
    }

    public async UniTask Load()
    {
        await _generaorManager.InitManager();

        var loaded = false;
        SmartStorage.LoadSmartObject<SoundSettingsData>(response => {
            _settingData = response.Data;
            loaded = true;
        });
        await UniTask.WaitUntil(() => loaded);

        _settingsModel.SetVolumes(_settingData.SoundSettings.SoundVolume, _settingData.SoundSettings.MusicVolume);

        _musicManager.Init(_settingData);
        _soundManager.Init(_settingData);
    }
}
