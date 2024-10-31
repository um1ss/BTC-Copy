using Balancy.Data;
using UnityEngine;

public class SoundAudioManager : AbstractAudioManager
{
    [SerializeField] private AudioClip _buttonClickClip;
    [SerializeField] private AudioClip _oneGirlClickClip;
    [SerializeField] private AudioClip _twoGirlClickClip;
    public override void Init(SoundSettingsData data)
    {
        base.Init(data);
        _settingsModel.SetDelegate(AppConstants.SoundValueName, (value) => 
        {
            _source.volume = value;
            _settingData.SoundSettings.SoundVolume = value;
        });
        _source.volume = _settingData.SoundSettings.SoundVolume;
    }

    public void PlayButtonClip()
    {
        _source.PlayOneShot(_buttonClickClip); 
    }
    public void PlayOneGirlClip()
    {
        _source.Stop();
        _source.PlayOneShot(_oneGirlClickClip);
    }
    public void PlayTwoGirlClip()
    {
        _source.Stop();
        _source.PlayOneShot(_twoGirlClickClip);
    }
}
