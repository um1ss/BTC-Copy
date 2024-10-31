using Balancy.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

[RequireComponent(typeof(AudioSource))]
public abstract class AbstractAudioManager : MonoBehaviour
{
    protected SoundSettingsData _settingData;

    [Inject] protected SettingsModel _settingsModel;
    [Inject] private IAssetProvider _assetProvider;

    protected AudioSource _source;

    private float _currentTime;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }
    public void StopAudio()
    {
        _source.Stop();
        _source.clip = null;
    }
    protected void PlayClip(AudioClip clip)
    {
        _source.mute = false;
        _source.clip = clip;
        _source.Play();
    }
    protected void IsLoop(bool loop)
    {
        _source.loop = loop;
    }
    public void SwitchAudioPause()
    {
        if (_source.isPlaying)
        {
            _currentTime = _source.time;
            _source.Pause();
        }
        else
        {
            _source.time = _currentTime;
            _source.Play();
        }
    }
    public virtual void Init(SoundSettingsData data)
    {
        _settingData = data;
    }
}

[System.Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}
