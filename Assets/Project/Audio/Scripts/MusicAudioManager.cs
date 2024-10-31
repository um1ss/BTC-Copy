using Cysharp.Threading.Tasks;
using Balancy.Data;
using UnityEngine;

public class MusicAudioManager : AbstractAudioManager
{
    [SerializeField] private AssetReferenceAudioClip _mainClipReference;

    private AudioClip _currentClip;
    public override void Init(SoundSettingsData data)
    {
        base.Init(data);
        _settingsModel.SetDelegate(AppConstants.MusicValueName, (value) =>
        {
            _source.volume = value;
            _settingData.SoundSettings.MusicVolume = value;
        });
        _source.volume = _settingData.SoundSettings.MusicVolume;
    }
    public async UniTask LoadAndPlayMainClip(bool isLoop)
    {
        var clip = await _mainClipReference.LoadAssetAsync();
        _currentClip = clip;
        PlayClip(_currentClip);
        IsLoop(isLoop);
        await UniTask.Yield();
    }
}
